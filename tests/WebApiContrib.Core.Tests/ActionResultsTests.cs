using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
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

        [Theory]
        [InlineData("/ok", 200)]
        [InlineData("/accepted", 202)]
        [InlineData("/nocontent", 204)]
        [InlineData("/badrequest", 400)]
        [InlineData("/unauthorized", 401)]
        [InlineData("/forbidden", 403)]
        [InlineData("/notfound", 404)]
        [InlineData("/conflict", 409)]
        [InlineData("/teapot", 418)]
        public async Task StatusCode(string route, int httpStatusCode)
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, route);
            var result = await client.SendAsync(request);

            Assert.Equal(httpStatusCode, (int)result.StatusCode);
        }

        [Theory]
        [InlineData("/ok-with-object", 200)]
        [InlineData("/accepted-with-object", 202)]
        [InlineData("/badrequest-with-object", 400)]
        [InlineData("/notfound-with-object", 404)]
        [InlineData("/conflict-with-object", 409)]
        [InlineData("/unprocessable", 422)]
        public async Task StatusCodeWithObject(string route, int httpStatusCode)
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, route);
            var result = await client.SendAsync(request);
            var objectResult = JsonConvert.DeserializeObject<Item>(await result.Content.ReadAsStringAsync());

            Assert.Equal(httpStatusCode, (int)result.StatusCode);
            Assert.Equal("test", objectResult.Name);
        }

        [Fact]
        public async Task Created()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"/created");
            var result = await client.SendAsync(request);
            var objectResult = JsonConvert.DeserializeObject<Item>(await result.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            Assert.Equal("https://foo.bar/", result.Headers.Location.ToString());
            Assert.Equal("test", objectResult.Name);
        }

        [Fact]
        public async Task Content()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"/content");
            var result = await client.SendAsync(request);
            var objectResult = JsonConvert.DeserializeObject<Item>(await result.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("application/json", result.Content.Headers.ContentType.MediaType.ToString());
            Assert.Equal("test", objectResult.Name);
        }

        [Theory]
        [InlineData("/local-redirect-permanent", 301, "/foo")]
        [InlineData("/redirect-permanent", 301, "https://foo.bar/")]
        [InlineData("/local-redirect", 302, "/foo")]
        [InlineData("/redirect", 302, "https://foo.bar/")]
        [InlineData("/local-redirect-preserve-temporary", 307, "/foo")]
        [InlineData("/redirect-preserve-temporary", 307, "https://foo.bar/")]
        [InlineData("/local-redirect-preserve-permanent", 308, "/foo")]
        [InlineData("/redirect-preserve-permanent", 308, "https://foo.bar/")]
        public async Task Redirect(string route, int httpStatusCode, string redirect)
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, route);
            var result = await client.SendAsync(request);

            Assert.Equal(httpStatusCode, (int)result.StatusCode);
            Assert.Equal(redirect, result.Headers.Location.ToString());
        }

        [Fact]
        public async Task ProblemDetails()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"/problemdetails");
            var result = await client.SendAsync(request);
            var problemDetails = JsonConvert.DeserializeObject<ValidationProblemDetails>(await result.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("problem details", problemDetails.Title);
            Assert.Equal("stack trace", problemDetails.Detail);
            Assert.Equal("error", problemDetails.Instance);
        }

        [Theory]
        [InlineData("/file-byte")]
        [InlineData("/file-stream")]
        [InlineData("/file-physical")]
        [InlineData("/file-virtual")]
        public async Task File(string route)
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, route);
            var result = await client.SendAsync(request);
            var body = Encoding.UTF8.GetString(await result.Content.ReadAsByteArrayAsync());

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("text/plain", result.Content.Headers.ContentType.MediaType.ToString());
            Assert.Equal("file.txt", result.Content.Headers.ContentDisposition.FileName);
            Assert.Equal("file", body, StringComparer.InvariantCulture);
        }
    }
}
