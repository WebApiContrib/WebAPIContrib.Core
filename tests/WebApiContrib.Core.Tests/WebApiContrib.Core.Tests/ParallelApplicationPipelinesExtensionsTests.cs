using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using Xunit;

namespace WebApiContrib.Core.Tests
{
    public class ParallelApplicationPipelinesExtensionsTests
    {
        [Fact]
        public async Task HttpContextAccessor_Not_Null()
        {
            var webHostBuilder = new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>();

            var testServer = new TestServer(webHostBuilder);

            var client = testServer.CreateClient();

            var response = await client.GetAsync("/path");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        public class Startup
        {
            public void ConfigureServices(IServiceCollection services)
            {}

            public void Configure(IApplicationBuilder app)
            {
                app.UseBranchWithServices("/path",
                    services =>
                    {
                        services.AddSingleton<Service>();
                    },
                    pathApp =>
                    {
                        pathApp.UseMiddleware<Middleware>();
                    });
            }
        }

        public class Service
        {
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Service(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
            }
        }

        public class Middleware
        {
            private readonly RequestDelegate _next;
            private readonly Service _service;

            public Middleware(RequestDelegate next, Service service)
            {
                _next = next;
                _service = service;
            }

            public async Task Invoke(HttpContext httpContext)
            {
                await _next(httpContext);
            }
        }
    }
}
