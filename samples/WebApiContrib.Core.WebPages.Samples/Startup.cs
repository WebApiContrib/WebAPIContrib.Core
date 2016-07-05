using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WebApiContrib.Core.WebPages;

namespace WebApiContrib.Core.WebPages.Samples
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebPages();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseWebPages(rootViewName: "RazorSample");
        }
    }
}
