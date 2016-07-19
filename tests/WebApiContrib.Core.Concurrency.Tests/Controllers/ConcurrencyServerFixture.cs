using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebApiContrib.Core.Concurrency.Extensions;
using WebApiContrib.Core.Concurrency.Tests.Models;
using Xunit;

namespace WebApiContrib.Core.Concurrency.Tests.Controllers
{
    public class ConcurrencyServerFixture
    {
        private class FakeStartup
        {
            public void ConfigureServices(IServiceCollection serviceCollection)
            {
                serviceCollection.AddConcurrency(opt => opt.UseInMemoryStorage());
                serviceCollection.AddMvc();
            }

            public void Configure(IApplicationBuilder app)
            {
                app.UseMvc();
            }
        }

        [Fact]
        public async Task When_Customer_Is_Created_Then_Etag_Is_Returned()
        {
            // ARRANGE
            var customer = new Customer
            {
                FirstName = "loki"
            };
            var server = CreateServer();
            var client = server.CreateClient();
            var requestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost/customers"),
                Content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json")
            };

            // ACT
            var response = await client.SendAsync(requestMessage);

            // ASSERT
            Assert.NotEmpty(response.Headers.GetEtag());
        }

        [Fact]
        public async Task When_Updating_Customer_And_Correct_Etag_Is_Passed_Then_NewEtag_Is_Returned()
        {
            // ARRANGE
            var customer = new Customer
            {
                FirstName = "loki"
            };
            var server = CreateServer();
            var client = server.CreateClient();
            var insertRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost/customers"),
                Content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(insertRequestMessage);
            var newCustomer = JsonConvert.DeserializeObject<Customer>(await response.Content.ReadAsStringAsync());
            var etag = response.Headers.GetEtag();
            var updateRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri("http://localhost/customers"),
                Content = new StringContent(JsonConvert.SerializeObject(newCustomer), Encoding.UTF8, "application/json")
            };
            updateRequestMessage.Headers.IfMatch.Add(new EntityTagHeaderValue(etag));

            // ARRANGE
            var updateResponse = await client.SendAsync(updateRequestMessage);

            // ASSERTS
            Assert.NotEmpty(updateResponse.Headers.GetEtag());
            Assert.True(updateResponse.Headers.GetEtag() != etag);
        }

        [Fact]
        public async Task When_Updating_Customer_And_Correct_ModifiedDate_Is_Passed_Then_NewEtag_Is_Returned()
        {
            // ARRANGE
            var customer = new Customer
            {
                FirstName = "loki"
            };
            var server = CreateServer();
            var client = server.CreateClient();
            var insertRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost/customers"),
                Content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(insertRequestMessage);
            var newCustomer = JsonConvert.DeserializeObject<Customer>(await response.Content.ReadAsStringAsync());
            var etag = response.Headers.GetEtag();
            var updateRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri("http://localhost/customers"),
                Content = new StringContent(JsonConvert.SerializeObject(newCustomer), Encoding.UTF8, "application/json")
            };
            updateRequestMessage.Headers.IfMatch.Add(EntityTagHeaderValue.Any);
            updateRequestMessage.Headers.IfUnmodifiedSince = DateTime.UtcNow.AddDays(1).ToUniversalTime();

            // ARRANGE
            var updateResponse = await client.SendAsync(updateRequestMessage);

            // ASSERTS
            Assert.NotEmpty(updateResponse.Headers.GetEtag());
            Assert.True(updateResponse.Headers.GetEtag() != etag);
        }

        [Fact]
        public async Task When_Updating_Customer_And_Old_Etag_Is_Passed_Then_Error_412_Is_Returned()
        {
            // ARRANGE
            var customer = new Customer
            {
                FirstName = "loki"
            };
            var server = CreateServer();
            var client = server.CreateClient();
            var insertRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost/customers"),
                Content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(insertRequestMessage);
            var newCustomer = JsonConvert.DeserializeObject<Customer>(await response.Content.ReadAsStringAsync());
            var etag = response.Headers.GetEtag();
            var updateRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri("http://localhost/customers"),
                Content = new StringContent(JsonConvert.SerializeObject(newCustomer), Encoding.UTF8, "application/json")
            };
            updateRequestMessage.Headers.IfMatch.Add(new EntityTagHeaderValue(etag));
            await client.SendAsync(updateRequestMessage);
            var newUpdateRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri("http://localhost/customers"),
                Content = new StringContent(JsonConvert.SerializeObject(newCustomer), Encoding.UTF8, "application/json")
            };
            newUpdateRequestMessage.Headers.IfMatch.Add(new EntityTagHeaderValue(etag));
            
            // ARRANGE
            var updateResponse = await client.SendAsync(newUpdateRequestMessage);

            // ASSERTS
            Assert.True(updateResponse.StatusCode == HttpStatusCode.PreconditionFailed);
        }

        [Fact]
        public async Task When_Updating_Customer_And_Passed_Too_Old_UnModifiedDate_Is_Passed_Then_Error_412_Is_Returned()
        {
            // ARRANGE
            var customer = new Customer
            {
                FirstName = "loki"
            };
            var server = CreateServer();
            var client = server.CreateClient();
            var insertRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost/customers"),
                Content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(insertRequestMessage);
            var newCustomer = JsonConvert.DeserializeObject<Customer>(await response.Content.ReadAsStringAsync());
            var etag = response.Headers.GetEtag();
            var updateRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri("http://localhost/customers"),
                Content = new StringContent(JsonConvert.SerializeObject(newCustomer), Encoding.UTF8, "application/json")
            };
            updateRequestMessage.Headers.IfMatch.Add(EntityTagHeaderValue.Any);
            updateRequestMessage.Headers.IfUnmodifiedSince = DateTime.UtcNow.AddDays(-2).ToUniversalTime();

            // ARRANGE
            var updateResponse = await client.SendAsync(updateRequestMessage);

            // ASSERTS
            Assert.True(updateResponse.StatusCode == HttpStatusCode.PreconditionFailed);
        }

        [Fact]
        public async Task When_Getting_Customer_For_An_Existing_Representation_Then_304_Is_Returned()
        {
            // ARRANGE
            var customer = new Customer
            {
                FirstName = "loki"
            };
            var server = CreateServer();
            var client = server.CreateClient();
            var insertRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost/customers"),
                Content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(insertRequestMessage);
            var newCustomer = JsonConvert.DeserializeObject<Customer>(await response.Content.ReadAsStringAsync());
            var etag = response.Headers.GetEtag();
            var getRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://localhost/customers/{newCustomer.CustomerId}")
            };
            getRequestMessage.Headers.IfNoneMatch.Add(new EntityTagHeaderValue(etag));

            // ARRANGE
            var updateResponse = await client.SendAsync(getRequestMessage);

            // ASSERTS
            Assert.True(updateResponse.StatusCode == HttpStatusCode.NotModified);
        }

        [Fact]
        public async Task When_Getting_Customer_And_Passed_Latest_ModificationDate_Then_304_Is_Returned()
        {
            // ARRANGE
            var customer = new Customer
            {
                FirstName = "loki"
            };
            var server = CreateServer();
            var client = server.CreateClient();
            var insertRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost/customers"),
                Content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(insertRequestMessage);
            var newCustomer = JsonConvert.DeserializeObject<Customer>(await response.Content.ReadAsStringAsync());
            var etag = response.Headers.GetEtag();
            var getRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://localhost/customers/{newCustomer.CustomerId}")
            };
            getRequestMessage.Headers.IfNoneMatch.Add(EntityTagHeaderValue.Any);
            getRequestMessage.Headers.IfModifiedSince = DateTime.UtcNow.AddDays(1).ToUniversalTime();

            // ARRANGE
            var updateResponse = await client.SendAsync(getRequestMessage);

            // ASSERTS
            Assert.True(updateResponse.StatusCode == HttpStatusCode.NotModified);
        }

        [Fact]
        public async Task When_Getting_Customer_And_Passing_Old_Representation_Then_NewOne_Is_Returned()
        {
            // ARRANGE
            var customer = new Customer
            {
                FirstName = "loki"
            };
            var server = CreateServer();
            var client = server.CreateClient();
            var insertRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost/customers"),
                Content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(insertRequestMessage);
            var newCustomer = JsonConvert.DeserializeObject<Customer>(await response.Content.ReadAsStringAsync());
            var etag = response.Headers.GetEtag();
            var getRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://localhost/customers/{newCustomer.CustomerId}")
            };
            getRequestMessage.Headers.IfNoneMatch.Add(new EntityTagHeaderValue("\"old_representation\""));

            // ARRANGE
            var getResponse = await client.SendAsync(getRequestMessage);

            // ASSERTS
            Assert.NotEmpty(getResponse.Headers.GetEtag());
        }

        [Fact]
        public async Task When_Getting_Customer_Add_Passing_Old_ModifiedSinceDateTime_Then_New_Representation_Is_Returned()
        {
            // ARRANGE
            var customer = new Customer
            {
                FirstName = "loki"
            };
            var server = CreateServer();
            var client = server.CreateClient();
            var insertRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost/customers"),
                Content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(insertRequestMessage);
            var newCustomer = JsonConvert.DeserializeObject<Customer>(await response.Content.ReadAsStringAsync());
            var etag = response.Headers.GetEtag();
            var getRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://localhost/customers/{newCustomer.CustomerId}")
            };
            getRequestMessage.Headers.IfNoneMatch.Add(EntityTagHeaderValue.Any);
            getRequestMessage.Headers.IfModifiedSince = DateTime.UtcNow.AddDays(-2).ToUniversalTime();

            // ARRANGE
            var getResponse = await client.SendAsync(getRequestMessage);

            // ASSERTS
            Assert.NotEmpty(getResponse.Headers.GetEtag());
        }

        private static TestServer CreateServer()
        {
            var builder = new WebHostBuilder()
                .UseStartup(typeof(FakeStartup));
            return new TestServer(builder);
        }
    }
}
