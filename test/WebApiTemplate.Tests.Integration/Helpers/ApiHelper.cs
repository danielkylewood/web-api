using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using WebApiTemplate.Tests.Integration.Builders;
using WebApiTemplate.WebApi.Models;

namespace WebApiTemplate.Tests.Integration.Helpers
{
    public static class ApiHelper
    {
        public static async Task<Guid> CreateCustomer(TestServer testServer, string apiKey, CustomerRequestModel customerRequestModel = null)
        {
            if (customerRequestModel == null)
            {
                customerRequestModel =
                    new CustomerRequestModelBuilder()
                        .WithValidPropertyValues()
                        .Build();
            }

            var httpClient = CreateHttpClient(testServer, apiKey);
            var content = CreateContent(customerRequestModel);

            await httpClient.PostAsync("/customers", content);
            return Guid.Parse(customerRequestModel.ExternalCustomerReference);
        }

        public static HttpClient CreateHttpClient(TestServer testServer, string authKey)
        {
            var client = testServer.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", authKey);
            client.DefaultRequestHeaders.Add("ContentType", "application/json");

            return client;
        }

        public static StringContent CreateContent(object contentAsObj)
        {
            var json = SerializerHelper.SerializeTo(contentAsObj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return content;
        }
    }
}