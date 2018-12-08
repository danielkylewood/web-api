using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using WebApiTemplate.Domain.Models;
using WebApiTemplate.WebApi.Models;
using WebApiTemplate.WebApi.Validators;

namespace WebApiTemplate.Tests.Unit
{
    public class CustomerValidatorTests
    {
        [Test, TestCaseSource(nameof(GetTests))]
        public async Task RunTests(int testId, CustomerRequestModel customer, bool expectedValid)
        {
            var sut = new CustomerValidator();
            var result = await sut.ValidateAsync(customer);

            Assert.That(result.IsValid, Is.EqualTo(expectedValid));
        }

        public static IEnumerable<object[]> GetTests()
        {
            yield return new object[]
            {
                1,
                new CustomerRequestModel(Guid.NewGuid().ToString(), "Name", "Surname", "Gold"), 
                true,
            };

            yield return new object[]
            {
                1,
                new CustomerRequestModel(Guid.NewGuid().ToString(), "Name", "Surname", "Blue"),
                false
            };
        }
    }
}
