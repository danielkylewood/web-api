using System;

namespace WebApiTemplate.Domain.Models
{
    public class Customer
    {
        public Guid ExternalCustomerReference { get; }
        public string FirstName { get; }
        public string Surname { get; }
        public Status Status { get; }
        public DateTime CreatedDate { get; }
        public DateTime? LastModifiedDate { get; }

        public Customer(Guid externalCustomerReference, string firstName, string surname, Status status, DateTime createdDate, DateTime? lastModifiedDate)
        {
            ExternalCustomerReference = externalCustomerReference;
            FirstName = firstName;
            Surname = surname;
            Status = status;
            CreatedDate = createdDate;
            LastModifiedDate = lastModifiedDate;
        }
    }
}
