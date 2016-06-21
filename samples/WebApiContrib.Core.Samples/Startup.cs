using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApiContrib.Core.Formatter.Csv;
using WebApiContrib.Core.Formatter.PlainText;

namespace WebApiContrib.Core.Samples
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //var csvOptions = new CsvFormatterOptions
            //{
            //    UseSingleLineHeaderInCsv = true,
            //    CsvDelimiter = ","
            //};

            //services.AddMvc()
            //    .AddCsvSerializerFormatters(csvOptions);

            services.AddMvc()
                .AddCsvSerializerFormatters()
                .AddPlainTextFormatters();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
