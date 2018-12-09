using System;
using System.Collections.Generic;
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
using WebApiTemplate.WebApi.Models.Hypermedia;

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
            var customer = customerRequestModel.ToDomainType();
            await _customersRepository.CreateCustomer(customer);
            _logger.Information($"Created customer with customer reference: {customer.CustomerReference}.");
            
            var links = HypermediaLinkBuilder.ForCustomerDiscovery(Url, customer.CustomerReference.ToString());
            var customerResponseData = new Dictionary<string, string>
                {{"customer_reference", customer.CustomerReference.ToString()}};
            var response = new CreatedResponse<CustomerDiscovery>("customer_created", links, customerResponseData);

            return Created(string.Empty, response);
        }

        [HttpPut("{customerReference}")]
        public async Task<IActionResult> Put(string customerReference, [FromBody] CustomerRequestModel customerRequestModel)
        {
            if (!Guid.TryParse(customerReference, out var guid))
            {
                _logger.Warning($"Could not parse customer reference: {customerReference}.");
                var errorResponse = new ValidationError("request_invalid", new List<string> { ErrorCodes.CustomerReferenceInvalid });
                return new Models.BadRequestObjectResult(errorResponse);
            }

            var customerOption = await _customersRepository.GetCustomerByCustomerReference(guid);
            if (!customerOption.TryUnwrap(out var customer))
            {
                _logger.Warning($"Cannot find customer with customer reference: {customerReference}.");
                return NotFound();
            }

            var customertoUpdate = customerRequestModel.ToDomainType();
            var updatedCustomer = new UpdateCustomer(
                customer.CustomerReference,
                customertoUpdate.FirstName,
                customertoUpdate.Surname,
                customertoUpdate.Status,
                customer.CreatedDate,
                customer.LastModifiedDate);

            var customerToSave = _customerService.UpdateCustomer(updatedCustomer);
            await _customersRepository.UpdateCustomer(customerToSave);
            _logger.Information($"Updated customer with customer reference: {customer.CustomerReference}.");

            var links = HypermediaLinkBuilder.ForCustomerDiscovery(Url, customer.CustomerReference.ToString());
            var response = new CreatedResponse<CustomerDiscovery>("customer_updated", links);

            return Ok(response);
        }

        [HttpGet("{customerReference}")]
        public async Task<IActionResult> Get(string customerReference)
        {
            if (!Guid.TryParse(customerReference, out var parsedCustomerReference))
                return BadRequest();

            var customerOption = await _customersRepository.GetCustomerByCustomerReference(parsedCustomerReference);

            if (customerOption.TryUnwrap(out var customer))
                return Ok(CustomerRequestModel.FromDomainType(customer));
            
            _logger.Warning($"Could not find customer with customer reference: {customerReference}.");
            return NotFound();
        }
    }
}
