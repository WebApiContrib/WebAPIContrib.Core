using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WebApiContrib.Core.Tests
{
    public class ParallelPipelinesTests
    {
        private TestServer _server;

        public ParallelPipelinesTests()
        {
            _server = new TestServer(new WebHostBuilder()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>());
        }

        [Theory]
        [InlineData("foo")]
        [InlineData("bar")]
        public async Task CanReachBranch(string path)
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"/{path}");
            var result = await client.SendAsync(request);
            var stringResult = await result.Content.ReadAsStringAsync();

            Assert.Equal($"hi {path}", stringResult);
        }

        [Theory]
        [InlineData("foo1")]
        [InlineData("foo2")]
        [InlineData("bar1")]
        [InlineData("bar2")]
        public async Task CanReachBranchWithMultipleEntryPoints(string path)
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"/{path}");
            var result = await client.SendAsync(request);
            var stringResult = await result.Content.ReadAsStringAsync();

            Assert.Equal($"hi {path.Substring(0, path.Length-1)}", stringResult);
        }
    }
}
