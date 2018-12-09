using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using WebApiTemplate.Tests.Integration.Builders;
using WebApiTemplate.Tests.Integration.Fixtures;
using WebApiTemplate.Tests.Integration.Helpers;
using WebApiTemplate.WebApi;
using WebApiTemplate.WebApi.Models;

namespace WebApiTemplate.Tests.Integration
{
    public class CreateMerchantTests
    {
        private TestServer _testServer;

        [SetUp]
        public async Task SetUp()
        {
            await Database.SetUp();
            var builder = TestWebHostBuilder.BuildTestWebHostForStartUp<Startup>();
            _testServer = new TestServer(builder);
        }

        [Test]
        public async Task Creating_Customer_With_Valid_Model_Must_Return_201_Created()
        {
            var model =
                new CustomerRequestModelBuilder()
                .WithValidPropertyValues()
                .Build();

            // Given user with valid api key
            var httpClient = ApiHelper.CreateHttpClient(_testServer, ApiKeys.Valid);
            var content = ApiHelper.CreateContent(model);

            // When I want to create a customer and the model is valid
            var response = await httpClient.PostAsync("/customers", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Then the customer is created
            Assert.That(response, Is.Not.Null);
            Assert.That(responseContent, Is.Not.Null);

            // And a 201 Created code is received
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(response.IsSuccessStatusCode, Is.True);
        }

        [Test]
        public async Task Creating_Customer_With_Invalid_Model_Must_Return_400_Bad_Request()
        {
            var model =
                new CustomerRequestModelBuilder()
                    .WithValidPropertyValues()
                    .WithExternalCustomerReference("InvalidGuid")
                    .Build();

            // Given user with valid api key
            var httpClient = ApiHelper.CreateHttpClient(_testServer, ApiKeys.Valid);
            var content = ApiHelper.CreateContent(model);

            // When I want to create a customer and the model is invalid
            var response = await httpClient.PostAsync("/customers", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Then we received a 422
            Assert.That((int)response.StatusCode, Is.EqualTo(400));
            Assert.That(response.IsSuccessStatusCode, Is.False);
            Assert.That(responseContent, Is.Not.Empty);

            // And reasons for the failure are returned
            var responseObj = SerializerHelper.DeserializeFrom<ValidationError>(responseContent);

            Assert.That(responseObj, Is.Not.Null);
            Assert.That(responseObj.ErrorType, Is.EqualTo("request_invalid"));
            Assert.That(responseObj.RequestId, Is.Not.Empty);
            Assert.That(1 == responseObj.ErrorCodes.ToList().Count, Is.True);
            Assert.That(responseObj.ErrorCodes.First(), Is.EqualTo(ErrorCodes.ExternalCustomerReferenceInvalid));
        }

        [Test]
        public async Task Creating_Customer_That_Already_Exists_Must_Return_409_Conflict()
        {
            var existingCustomer = await ApiHelper.CreateCustomer(_testServer, ApiKeys.Valid);

            var model =
                new CustomerRequestModelBuilder()
                    .WithValidPropertyValues()
                    .WithExternalCustomerReference(existingCustomer.ToString())
                    .Build();

            // Given user with valid api key
            var httpClient = ApiHelper.CreateHttpClient(_testServer, ApiKeys.Valid);
            var content = ApiHelper.CreateContent(model);

            // When I want to create a customer and the model is invalid
            var response = await httpClient.PostAsync("/customers", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Then the customer must not be created
            Assert.That(response, Is.Not.Null);
            Assert.That(responseContent, Is.Not.Null);

            // And a 409 is returned
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
            Assert.That(response.IsSuccessStatusCode, Is.False);
            Assert.That(responseContent, Is.Empty);
        }
    }
}