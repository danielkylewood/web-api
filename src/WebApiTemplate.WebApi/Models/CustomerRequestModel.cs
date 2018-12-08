using System;
using FluentValidation.Attributes;
using WebApiTemplate.Domain.Models;
using WebApiTemplate.WebApi.Validators;

namespace WebApiTemplate.WebApi.Models
{
    [Validator(typeof(CustomerValidator))]
    public class CustomerRequestModel
    {
        public string ExternalCustomerReference { get; }
        public string FirstName { get; }
        public string Surname { get; }
        public string Status { get; }

        public CustomerRequestModel(string externalCustomerReference, string firstName, string surname, string status)
        {
            ExternalCustomerReference = externalCustomerReference;
            FirstName = firstName;
            Surname = surname;
            Status = status;
        }

        public Customer ToDomainType()
        {
            var status = (Status)Enum.Parse(typeof(Status), Status, true);
            Guid.TryParse(ExternalCustomerReference, out var externalCustomerReference);

            return new Customer(
                externalCustomerReference,
                FirstName,
                Surname,
                status,
                DateTime.UtcNow, 
                null);
        }

        public static CustomerRequestModel FromDomainType(Customer customer)
        {
            return new CustomerRequestModel(
                customer.ExternalCustomerReference.ToString(),
                customer.FirstName,
                customer.Surname,
                customer.Status.ToString());
        }
    }
}
