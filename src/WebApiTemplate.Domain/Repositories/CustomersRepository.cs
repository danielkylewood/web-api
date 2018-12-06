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

        public async Task<Option<Customer>> GetCustomerByExternalCustomerReference(Guid externalCustomerReference)
        {
            var parameters = new
            {
                externalCustomerReference
            };

            const string sql = @"
                SELECT
                    [ExternalCustomerReference], 
                    [FirstName],
                    [Surname],
                    [Status],
                    [CreatedDate],
                    [LastModifiedDate]
                FROM [dbo].[Customers] m
                WHERE m.ExternalCustomerReference = @externalCustomerReference
            ";

            using (var connection = await CreateConnection())
            {
                var dto = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, parameters);

                if (dto == null)
                {
                    return Option.None<Customer>();
                }
                
                var customer = new Customer(
                    dto.ExternalCustomerReference,
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
            var tasks = customers.Select(account => new {
                externalCustomerReference = account.ExternalCustomerReference,
                firstName = account.FirstName,
                surname = account.Surname,
                status = account.Status,
                createdDate = account.CreatedDate,
                modifiedDate = account.LastModifiedDate
            });

            const string sql = @"
                INSERT INTO [dbo].[Customers]
                (
                    [ExternalCustomerReference],
                    [FirstName],
                    [Surname],
                    [Status],
                    [CreatedDate],
                    [LastModifiedDate]
                )
                VALUES
                (
                    @externalCustomerReference,
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
                    lastModifiedDate = customer.LastModifiedDate,
                    status = (int)customer.Status,
                    name = customer.FirstName,
                    surname = customer.Surname
                };

                const string sql = @"
                    UPDATE [dbo].[Customers]
                    SET
                        [LastModifiedDate] = @lastModifiedDate,
                        [Status] = @mandateState,
                        [FirstName],
                        [Surname]
                    WHERE MandateReference = @MandateReference
                ";

                await connection.ExecuteAsync(sql, parameters);
            }
        }
    }
}
