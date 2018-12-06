using System;
using WebApiTemplate.Domain.Models;

namespace WebApiTemplate.WebApi.Models
{
    public class CustomerRequest
    {
        public string ExternalCustomerReference { get; }
        public string FirstName { get; }
        public string Surname { get; }
        public string Status { get; }

        public CustomerRequest(string externalCustomerReference, string firstName, string surname, string status)
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

        public static CustomerRequest FromDomainType(Customer customer)
        {
            return new CustomerRequest(
                customer.ExternalCustomerReference.ToString(),
                customer.FirstName,
                customer.Surname,
                customer.Status.ToString());
        }
    }
}
