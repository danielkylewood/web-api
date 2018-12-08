using System;
using Serilog;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApiTemplate.Domain.Configuration;
using WebApiTemplate.Domain.Extensions;
using WebApiTemplate.Domain.Models;
using WebApiTemplate.Domain.Repositories;
using WebApiTemplate.Domain.Services;
using WebApiTemplate.WebApi.Models;

namespace WebApiTemplate.WebApi.Controllers
{
    [Route("[controller]", Name = "Customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICustomersService _customerService;
        private readonly CustomersRepository _customersRepository;

        public CustomersController(IOptions<DatabaseOptions> databaseOptions, ICustomersService customerService, ILogger logger)
        {
            _customerService = customerService;
            _logger = logger.ForContext<CustomersController>();
            _customersRepository = new CustomersRepository(databaseOptions.Value.DatabaseConnectionString);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CustomerRequestModel customerRequestModel)
        {
            Guid.TryParse(customerRequestModel.ExternalCustomerReference, out var customerReference);
            var customerOption = await _customersRepository.GetCustomerByExternalCustomerReference(customerReference);

            if (customerOption.TryUnwrap(out var _))
            {
                _logger.Warning($"Customer already exists with external customer reference: {customerRequestModel.ExternalCustomerReference}.");
                return Conflict();
            }

            var customer = customerRequestModel.ToDomainType();
            await _customersRepository.CreateCustomer(customer);
            _logger.Information($"Created customer with external customer reference: {customer.ExternalCustomerReference}.");
            return Created($"customers/{customer.ExternalCustomerReference}", CustomerRequestModel.FromDomainType(customer));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CustomerRequestModel customerRequestModel)
        {
            Guid.TryParse(customerRequestModel.ExternalCustomerReference, out var customerReference);
            var customerOption = await _customersRepository.GetCustomerByExternalCustomerReference(customerReference);

            if (!customerOption.TryUnwrap(out var customer))
            {
                _logger.Warning($"Cannot find customer with external customer reference: {customerRequestModel.ExternalCustomerReference}.");
                return NotFound();
            }

            var customertoUpdate = customerRequestModel.ToDomainType();
            var updatedCustomer = new UpdateCustomer(
                customertoUpdate.ExternalCustomerReference,
                customertoUpdate.FirstName,
                customertoUpdate.Surname,
                customertoUpdate.Status,
                customer.CreatedDate,
                customer.LastModifiedDate);

            var customerToSave = _customerService.UpdateCustomer(updatedCustomer);
            await _customersRepository.UpdateCustomer(customerToSave);
            _logger.Information($"Updated customer with external customer reference: {customer.ExternalCustomerReference}.");
            return Ok(CustomerRequestModel.FromDomainType(customerToSave));
        }

        [HttpGet("{customerExternalReference}")]
        public async Task<IActionResult> Get(string customerExternalReference)
        {
            if (!Guid.TryParse(customerExternalReference, out var customerReference))
                return BadRequest();

            var customerOption = await _customersRepository.GetCustomerByExternalCustomerReference(customerReference);

            if (customerOption.TryUnwrap(out var customer))
                return Ok(CustomerRequestModel.FromDomainType(customer));
            
            _logger.Warning("Could not find customer.");
            return NotFound();
        }
    }
}
