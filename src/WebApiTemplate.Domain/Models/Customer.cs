using System;

namespace WebApiTemplate.Domain.Models
{
    public class Customer
    {
        public Guid CustomerReference { get; }
        public string FirstName { get; }
        public string Surname { get; }
        public Status Status { get; }
        public DateTime CreatedDate { get; }
        public DateTime? LastModifiedDate { get; }

        public Customer(Guid customerReference, string firstName, string surname, Status status, DateTime createdDate, DateTime? lastModifiedDate)
        {
            CustomerReference = customerReference;
            FirstName = firstName;
            Surname = surname;
            Status = status;
            CreatedDate = createdDate;
            LastModifiedDate = lastModifiedDate;
        }
    }
}
