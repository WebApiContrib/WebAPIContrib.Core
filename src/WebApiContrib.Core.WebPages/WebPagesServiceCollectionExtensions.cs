using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiContrib.Core.WebPages
{
    public static class WebPagesServiceCollectionExtensions
    {
        public static void AddWebPages(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddMvcCore().AddRazorViewEngine(o =>
            {
                o.ViewLocationFormats.Clear();
                o.ViewLocationFormats.Add("/Views/{0}" + RazorViewEngine.ViewExtension);
            });
            services.AddSingleton<RazorViewToStringRenderer>();
        }
    }
}
