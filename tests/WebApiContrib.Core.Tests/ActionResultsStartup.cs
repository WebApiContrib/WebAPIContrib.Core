using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using WebApiContrib.Core.Results;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;

namespace WebApiContrib.Core.Tests
{
    public class TestController : ControllerBase
    {
        [HttpGet("test-controller", Name = "TestController")]
        public IActionResult Get()
        {
            return Ok("test controller");
        }
    } 

    public class Item
    {
        public string Name { get; set; }
    }

    public class ActionResultsStartup
    {
        public IConfigurationRoot Configuration { get; }

        public ActionResultsStartup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().AddJsonFormatters();

            var provider = services.BuildServiceProvider(validateScopes: true);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.Map("/ok", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.Ok();
                });
            });

            app.Map("/ok-with-object", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.Ok(new Item { Name = "test" });
                });
            });

            app.Map("/accepted", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.Accepted();
                });
            });

            app.Map("/accepted-with-object", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.Accepted(new Item { Name = "test" });
                });
            });

            app.Map("/notfound", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.NotFound();
                });
            });

            app.Map("/notfound-with-object", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.NotFound(new Item { Name = "test" });
                });
            });

            app.Map("/created", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.Created("https://foo.bar", new Item { Name = "test" });
                });
            });

            app.Map("/badrequest", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.BadRequest();
                });
            });

            app.Map("/badrequest-with-object", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.BadRequest(new Item { Name = "test" });
                });
            });

            app.Map("/unauthorized", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.Unauthorized();
                });
            });

            app.Map("/unauthorized-with-object", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.Unauthorized(new Item { Name = "test" });
                });
            });

            app.Map("/forbidden", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.Forbid();
                });
            });

            app.Map("/unprocessable", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.UnprocessableEntity(new Item { Name = "test" });
                });
            });

            app.Map("/redirect-to-route", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.RedirectToRoute("TestController");
                });
            });

            app.Map("/redirect-to-route-permanent", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.RedirectToRoutePermanent("TestController");
                });
            });

            app.UseMvc();
        }
    }
}
