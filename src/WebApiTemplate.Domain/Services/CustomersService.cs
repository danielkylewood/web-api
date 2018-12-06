using System;
using WebApiTemplate.Domain.Models;

namespace WebApiTemplate.Domain.Services
{
    public class CustomersService : ICustomersService
    {
        public Customer UpdateCustomer(Customer customer)
        {
            return new Customer(
                customer.ExternalCustomerReference,
                customer.FirstName,
                customer.Surname,
                customer.Status,
                customer.CreatedDate,
                DateTime.UtcNow);
        }
    }
}
