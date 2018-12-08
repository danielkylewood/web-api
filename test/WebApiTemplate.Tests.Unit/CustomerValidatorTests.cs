using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using WebApiTemplate.WebApi.Models;
using WebApiTemplate.WebApi.Validators;

namespace WebApiTemplate.Tests.Unit
{
    public class CustomerValidatorTests
    {
        private const string _invalidLengthName = "dlkfjasdlkfjasdlkfjsldkfjasdlkfjsdaklfjaslkdfjaklsdfjaklsdjflaskdjflsdkjflkasdfjlksdfjlskdfj";

        [Test, TestCaseSource(nameof(GetTests))]
        public async Task RunTests(int testId, CustomerRequestModel customer, bool expectedValid, List<string> errorListExpected)
        {
            var sut = new CustomerValidator();
            var result = await sut.ValidateAsync(customer);

            Assert.That(result.IsValid, Is.EqualTo(expectedValid));
            Assert.That(result.Errors.Count, Is.EqualTo(errorListExpected.Count));
            Assert.That(errorListExpected.All(x => result.Errors.Any(e => e.ErrorCode == x)), Is.True);
        }

        public static IEnumerable<object[]> GetTests()
        {
            yield return new object[]
            {
                1,
                new CustomerRequestModel(Guid.NewGuid().ToString(), "Name", "Surname", "Gold"),
                true,
                new List<string>()
            };

            yield return new object[]
            {
                2,
                new CustomerRequestModel("InvalidGuid", "Name", "Surname", "Gold"),
                false,
                new List<string>
                {
                    ErrorCodes.ExternalCustomerReferenceInvalid
                }
            };

            yield return new object[]
            {
                3,
                new CustomerRequestModel(Guid.NewGuid().ToString(), "@InvalidName", "Surname", "Gold"),
                false,
                new List<string>
                {
                    ErrorCodes.FirstNameInvalid
                }
            };

            yield return new object[]
            {
                4,
                new CustomerRequestModel(Guid.NewGuid().ToString(), "Name", "£Surname", "Gold"),
                false,
                new List<string>
                {
                    ErrorCodes.SurnameInvalid
                }
            };

            yield return new object[]
            {
                5,
                new CustomerRequestModel(Guid.NewGuid().ToString(), "Name", "Surname", "InvalidStatus"),
                false,
                new List<string>
                {
                    ErrorCodes.StatusInvalid
                }
            };

            yield return new object[]
            {
                6,
                new CustomerRequestModel(null, _invalidLengthName, _invalidLengthName, null),
                false,
                new List<string>
                {
                    ErrorCodes.ExternalCustomerReferenceRequired,
                    ErrorCodes.FirstNameLengthExceeded,
                    ErrorCodes.SurnameLengthExceeded,
                    ErrorCodes.StatusRequired
                }
            };

            yield return new object[]
            {
                7,
                new CustomerRequestModel(Guid.NewGuid().ToString(), null, null, "Gold"),
                false,
                new List<string>
                {
                    ErrorCodes.FirstNameRequired,
                    ErrorCodes.SurnameRequired
                }
            };
        }
    }
}
