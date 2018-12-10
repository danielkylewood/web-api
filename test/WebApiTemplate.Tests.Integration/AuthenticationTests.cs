using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using WebApiTemplate.Tests.Integration.Builders;
using WebApiTemplate.Tests.Integration.Fixtures;
using WebApiTemplate.Tests.Integration.Helpers;
using WebApiTemplate.WebApi;

namespace WebApiTemplate.Tests.Integration
{
    public class AuthenticationTests
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
        public async Task Creating_Customer_With_Invalid_Api_Key_Must_Return_401_Unauthorized()
        {
            var model =
                new CustomerRequestModelBuilder()
                    .WithValidPropertyValues()
                    .Build();

            // Given user with invalid api key
            var httpClient = ApiHelper.CreateHttpClient(_testServer, ApiKeys.Invalid);
            var content = ApiHelper.CreateContent(model);

            // When I want to create a customer
            var response = await httpClient.PostAsync("/customers", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Then a response is received
            Assert.That(response, Is.Not.Null);
            Assert.That(responseContent, Is.Not.Null);

            // And a 401 unauthorized code is received
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            Assert.That(response.IsSuccessStatusCode, Is.False);
        }
    }
}
