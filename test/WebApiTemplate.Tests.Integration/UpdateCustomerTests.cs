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
    public class UpdateCustomerTests
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
        public async Task Updating_Customer_With_Valid_Model_Must_Return_200_Ok()
        {
            const string updatedCustomerName = "DifferentFirstName";
            var customerReference = await ApiHelper.CreateCustomer(_testServer, ApiKeys.Valid);

            var model =
                new CustomerRequestModelBuilder()
                    .WithValidPropertyValues()
                    .WithExternalCustomerReference(customerReference.ToString())
                    .WithFirstName(updatedCustomerName)
                    .Build();

            // Given user with valid api key
            var httpClient = ApiHelper.CreateHttpClient(_testServer, ApiKeys.Valid);
            var content = ApiHelper.CreateContent(model);

            // When I want to update a customer and the model is valid
            var response = await httpClient.PutAsync("/customers", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Then the response must not be null
            Assert.That(response, Is.Not.Null);
            Assert.That(responseContent, Is.Not.Null);

            // And a 200 is returned
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.IsSuccessStatusCode, Is.True);

            // And content is updated customer
            var updatedCustomer = SerializerHelper.DeserializeFrom<CustomerRequestModel>(responseContent);
            Assert.That(updatedCustomer, Is.Not.Null);
            Assert.That(updatedCustomer.FirstName, Is.EqualTo(updatedCustomerName));
        }

        [Test]
        public async Task Updating_Customer_That_Does_Not_Exist_Must_Return_404_NotFound()
        {
            var model =
                new CustomerRequestModelBuilder()
                    .WithValidPropertyValues()
                    .Build();

            // Given user with valid api key
            var httpClient = ApiHelper.CreateHttpClient(_testServer, ApiKeys.Valid);
            var content = ApiHelper.CreateContent(model);

            // When I want to update a customer that does not exist
            var response = await httpClient.PutAsync("/customers", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Then the response must not be null
            Assert.That(response, Is.Not.Null);
            Assert.That(responseContent, Is.Not.Null);

            // And a 404 is returned
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(response.IsSuccessStatusCode, Is.False);
        }
    }
}
