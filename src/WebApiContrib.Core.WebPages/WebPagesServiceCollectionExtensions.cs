using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace WebApiContrib.Core.WebPages
{
    public static class WebPagesServiceCollectionExtensions
    {
        public static void AddWebPages(this IServiceCollection services)
        {
            AddWebPages(services, new WebPagesOptions());
        }

        public static void AddWebPages(this IServiceCollection services, WebPagesOptions webPagesOptions)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddMvcCore().AddRazorViewEngine(o =>
            {
                o.ViewLocationFormats.Clear();
                o.ViewLocationFormats.Add("/" + webPagesOptions.ViewsFolderName + "/{0}" + RazorViewEngine.ViewExtension);
            });

            services.AddSingleton<WebPagesOptions>(webPagesOptions);
            services.AddSingleton<RazorViewToStringRenderer>();
            services.AddSingleton<WebPagesRouter>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
