using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using WebApiContrib.Core.Results;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace WebApiContrib.Core.Tests
{
    public class Item
    {
        public string Name { get; set; }
    }

    public class ActionResultsStartup
    {
        public IConfigurationRoot Configuration { get; }

        public ActionResultsStartup(IHostingEnvironment env)
        {
            env.WebRootFileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().AddJsonFormatters();
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

            app.Map("/nocontent", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.NoContent();
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

            app.Map("/conflict", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.Conflict();
                });
            });

            app.Map("/conflict-with-object", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.Conflict(new Item { Name = "test" });
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

            app.Map("/teapot", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.StatusCode(StatusCodes.Status418ImATeapot);
                });
            });

            app.Map("/unprocessable", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.UnprocessableEntity(new Item { Name = "test" });
                });
            });

            app.Map("/content", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.Content(JsonConvert.SerializeObject(new Item { Name = "test" }), "application/json");
                });
            });

            app.Map("/redirect", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.Redirect("https://foo.bar");
                });
            });

            app.Map("/redirect-permanent", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.RedirectPermanent("https://foo.bar");
                });
            });

            app.Map("/redirect-preserve-temporary", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.RedirectPreserveMethod("https://foo.bar");
                });
            });

            app.Map("/redirect-preserve-permanent", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.RedirectPermanentPreserveMethod("https://foo.bar");
                });
            });

            app.Map("/local-redirect", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.LocalRedirect("/foo");
                });
            });

            app.Map("/local-redirect-permanent", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.LocalRedirectPermanent("/foo");
                });
            });

            app.Map("/local-redirect-preserve-temporary", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.LocalRedirectPreserveMethod("/foo");
                });
            });

            app.Map("/local-redirect-preserve-permanent", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.LocalRedirectPermanentPreserveMethod("/foo");
                });
            });

            app.Map("/problemdetails", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    await ctx.ValidationProblem(new ValidationProblemDetails
                    {
                        Title = "problem details",
                        Instance = "error",
                        Detail = "stack trace"
                    });
                });
            });

            app.Map("/file-byte", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    var file = Encoding.UTF8.GetBytes("file");
                    await ctx.File(file, "text/plain", "file.txt");
                });
            });

            app.Map("/file-stream", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    var file = new MemoryStream(Encoding.UTF8.GetBytes("file"));
                    await ctx.File(file, "text/plain", "file.txt");
                });
            });

            app.Map("/file-physical", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    var path = Path.GetFullPath(Path.Combine("test-files", "file.txt"));
                    await ctx.PhysicalFile(path, "text/plain", "file.txt");
                });
            });

            app.Map("/file-virtual", b =>
            {
                b.Use(async (ctx, next) =>
                {
                    var path = Path.Combine("test-files", "file.txt");
                    await ctx.File(path, "text/plain", "file.txt");
                });
            });
        }
    }
}
