using Microsoft.Extensions.DependencyInjection;
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
            services.AddSingleton<RazorViewToStringRenderer>();
        }
    }
}
