using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebApiContrib.Core.Tests
{
    public interface IService
    {
        string Hi();
    }

    public class FooService : IService
    {
        public string Hi() => "hi foo";
    }

    public class BarService : IService
    {
        public string Hi() => "hi bar";
    }

    public class ParallelPipelinesStartup
    {
        public IConfigurationRoot Configuration { get; }

        public ParallelPipelinesStartup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseBranchWithServices("/foo", s =>
            {
                s.AddTransient<IService, FooService>();
            }, a =>
            {
                a.Use(async (c, next) =>
                {
                    var service = c.RequestServices.GetRequiredService<IService>();
                    await c.Response.WriteAsync(service.Hi());
                });
            });

            app.UseBranchWithServices("/bar", s =>
            {
                s.AddTransient<IService, BarService>();
            }, a =>
            {
                a.Use(async (c, next) =>
                {
                    var service = c.RequestServices.GetRequiredService<IService>();
                    await c.Response.WriteAsync(service.Hi());
                });
            });

            app.UseBranchWithServices(new PathString[] { "/foo1", "/foo2" }, s =>
            {
                s.AddTransient<IService, FooService>();
            }, a =>
            {
                a.Use(async (c, next) =>
                {
                    var service = c.RequestServices.GetRequiredService<IService>();
                    await c.Response.WriteAsync(service.Hi());
                });
            });

            app.UseBranchWithServices(new PathString[] { "/bar1", "/bar2" }, s =>
            {
                s.AddTransient<IService, BarService>();
            }, a =>
            {
                a.Use(async (c, next) =>
                {
                    var service = c.RequestServices.GetRequiredService<IService>();
                    await c.Response.WriteAsync(service.Hi());
                });
            });

            // Add middleware before the next branch to verify state of the context coming out of the branch
            app.Use(async (c, next) =>
            {
                // Call into the branch
                await next();

                // Verify the service provider is working
                var service = c.RequestServices.GetService<ILogger>();
            });

            app.UseBranchWithServices("/baz", s =>
            {
            }, a =>
            {
            });
        }
    }
}
