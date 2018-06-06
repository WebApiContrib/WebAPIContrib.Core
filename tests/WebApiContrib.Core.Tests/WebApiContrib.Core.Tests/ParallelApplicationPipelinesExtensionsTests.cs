using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
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
            private readonly IServiceProvider _serviceProvider;

            public Startup(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            public void ConfigureServices()
            {}

            public void Configure(IApplicationBuilder app)
            {
                app.UseBranchWithServices("/path",
                    services =>
                    {
                        services.AddSingleton<Service>();
                        // Uncommenting the line below makes the test pass. However are there
                        // side effects? What else needs to be passed through?
                        // services.AddSingleton(_ =>_serviceProvider.GetService<IHttpContextAccessor>());
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
