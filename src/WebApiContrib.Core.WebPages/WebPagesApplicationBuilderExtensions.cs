using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiContrib.Core.WebPages
{
    public static class WebPagesApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseWebPages(this IApplicationBuilder app)
        {
            var router = app.ApplicationServices.GetRequiredService<WebPagesRouter>();
            return app.UseRouter(router);
        }
    }
}