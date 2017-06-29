using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApiContrib.Core.Formatter.Protobuf;

namespace WebApiContrib.Core.Protobuf.Tests
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.
                AddMvcCore().
                AddJsonFormatters().AddProtobufFormatters();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}
