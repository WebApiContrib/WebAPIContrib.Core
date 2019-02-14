using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WebApiContrib.Core.Tests
{
    public class ActionResultsTests
    {
        private TestServer _server;

        public ActionResultsTests()
        {
            _server = new TestServer(new WebHostBuilder()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<ActionResultsStartup>());
        }

        [Fact]
        public async Task Ok()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"/ok");
            var result = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task OkWithContent()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"/ok-with-object");
            var result = await client.SendAsync(request);
            var objectResult = JsonConvert.DeserializeObject<Item>(await result.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("test", objectResult.Name);
        }
    }
}
