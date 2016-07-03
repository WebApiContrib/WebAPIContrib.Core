using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;

namespace WebApiContrib.Core.WebPages
{
    public static class WebPagesApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseWebPages(this IApplicationBuilder app)
        {
            var renderer = app.ApplicationServices.GetRequiredService<RazorViewToStringRenderer>();
            var hostingEnvironment = app.ApplicationServices.GetRequiredService<IHostingEnvironment>();
            return app.UseRouter(new WebPagesRouter(hostingEnvironment, renderer));
        }
    }
}
