using System;
using Serilog;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApiTemplate.Domain.Configuration;
using WebApiTemplate.Domain.Extensions;
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
        public async Task<IActionResult> Post([FromBody] CustomerRequest customerRequest)
        {
            Guid.TryParse(customerRequest.ExternalCustomerReference, out var customerReference);
            var customerOption = await _customersRepository.GetCustomerByExternalCustomerReference(customerReference);

            if (customerOption.TryUnwrap(out var _))
            {
                _logger.Warning($"Customer already exists with external customer reference: {customerRequest.ExternalCustomerReference}.");
                return Conflict();
            }

            var customer = customerRequest.ToDomainType();
            await _customersRepository.CreateCustomer(customer);
            _logger.Information($"Created customer with external customer reference: {customer.ExternalCustomerReference}.");
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CustomerRequest customerRequest)
        {
            Guid.TryParse(customerRequest.ExternalCustomerReference, out var customerReference);
            var customerOption = await _customersRepository.GetCustomerByExternalCustomerReference(customerReference);

            if (!customerOption.TryUnwrap(out var customer))
            {
                _logger.Warning($"Cannot find customer with external customer reference: {customerRequest.ExternalCustomerReference}.");
                return NotFound();
            }

            var customerToUpdate = _customerService.UpdateCustomer(customer);
            await _customersRepository.UpdateCustomer(customerToUpdate);
            _logger.Information($"Updated customer with external customer reference: {customer.ExternalCustomerReference}.");
            return Ok();
        }

        [HttpGet("{exteralCustomerReference:Guid}")]
        public async Task<IActionResult> Get(Guid customerExternalReference)
        {
            var customerOption = await _customersRepository.GetCustomerByExternalCustomerReference(customerExternalReference);

            if (!customerOption.TryUnwrap(out var customer))
            {
                _logger.Warning("Could not find customer.");
                return NotFound();
            }

            return Ok(CustomerRequest.FromDomainType(customer));
        }
    }
}
