using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using WebApiTemplate.Tests.Integration.Fixtures;
using WebApiTemplate.Tests.Integration.Helpers;
using WebApiTemplate.WebApi;
using WebApiTemplate.WebApi.Models;

namespace WebApiTemplate.Tests.Integration
{
    public class GetCustomerTests
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
        public async Task Getting_Existing_Customer_Must_Return_200_Ok()
        {
            var customerReference = await ApiHelper.CreateCustomer(_testServer, ApiKeys.Valid);

            // Given user with valid api key
            var httpClient = ApiHelper.CreateHttpClient(_testServer, ApiKeys.Valid);

            // When I want to get a customer
            var response = await httpClient.GetAsync($"/customers/{customerReference}");
            var responseContent = await response.Content.ReadAsStringAsync();

            // Then the response must not be null
            Assert.That(response, Is.Not.Null);
            Assert.That(responseContent, Is.Not.Null);

            // And a 200 is returned
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.IsSuccessStatusCode, Is.True);

            // And content is the customer
            var updatedCustomer = SerializerHelper.DeserializeFrom<CustomerRequestModel>(responseContent);
            Assert.That(updatedCustomer, Is.Not.Null);
        }

        [Test]
        public async Task Getting_Customer_That_Does_Not_Exist_Must_Return_404_NotFound()
        {
            // Given user with valid api key
            var httpClient = ApiHelper.CreateHttpClient(_testServer, ApiKeys.Valid);

            // When I want to get a customer that does not exist
            var response = await httpClient.GetAsync($"/customers/{Guid.NewGuid()}");
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
