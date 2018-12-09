using System;
using Dapper;
using Optional;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebApiTemplate.Domain.Models;

namespace WebApiTemplate.Domain.Repositories
{
    public class CustomersRepository : BaseRepository
    {
        public CustomersRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<Option<Customer>> GetCustomerByCustomerReference(Guid customerReference)
        {
            var parameters = new
            {
                customerReference
            };

            const string sql = @"
                SELECT
                    [CustomerReference], 
                    [FirstName],
                    [Surname],
                    [Status],
                    [CreatedDate],
                    [LastModifiedDate]
                FROM [dbo].[Customers] m
                WHERE m.CustomerReference = @customerReference
            ";

            using (var connection = await CreateConnection())
            {
                var dto = await connection.QueryFirstOrDefaultAsync<CustomerDto>(sql, parameters);

                if (dto == null)
                {
                    return Option.None<Customer>();
                }

                var customer = new Customer(
                    dto.CustomerReference,
                    dto.FirstName,
                    dto.Surname,
                    dto.Status,
                    dto.CreatedDate,
                    dto.ModifiedDate
                );

                return Option.Some(customer);
            }
        }

        public async Task CreateCustomer(Customer customer)
        {
            await CreateCustomers(new List<Customer> { customer });
        }

        public async Task CreateCustomers(IEnumerable<Customer> customers)
        {
            var tasks = customers.Select(customer => new {
                customerReference = customer.CustomerReference,
                firstName = customer.FirstName,
                surname = customer.Surname,
                status = customer.Status,
                createdDate = customer.CreatedDate,
                modifiedDate = customer.LastModifiedDate
            });

            const string sql = @"
                INSERT INTO [dbo].[Customers]
                (
                    [CustomerReference],
                    [FirstName],
                    [Surname],
                    [Status],
                    [CreatedDate],
                    [LastModifiedDate]
                )
                VALUES
                (
                    @customerReference,
                    @firstName,
                    @surname,
                    @status,
                    @createdDate,
                    @modifiedDate
                )
            ";

            using (var connection = await CreateConnection())
            {
                await connection.ExecuteAsync(sql, tasks);
            }
        }

        public async Task UpdateCustomer(Customer customer)
        {
            using (var connection = await CreateConnection())
            {
                var parameters = new
                {
                    customerReference = customer.CustomerReference,
                    lastModifiedDate = customer.LastModifiedDate,
                    status = (int)customer.Status,
                    name = customer.FirstName,
                    surname = customer.Surname
                };

                const string sql = @"
                    UPDATE [dbo].[Customers]
                    SET
                        [LastModifiedDate] = @lastModifiedDate,
                        [Status] = @status,
                        [FirstName] = @name,
                        [Surname] = @surname
                    WHERE CustomerReference = @customerReference
                ";

                await connection.ExecuteAsync(sql, parameters);
            }
        }

        private class CustomerDto
        {
            public Guid CustomerReference { get; set; }
            public string FirstName { get; set; }
            public string Surname { get; set; }
            public Status Status { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? ModifiedDate { get; set; }
        }
    }
}
