using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApiContrib.Core.Formatter.Csv;
using WebApiContrib.Core.Formatter.Jsonp;
using WebApiContrib.Core.Formatter.PlainText;
using WebApiContrib.Core.Samples.Controllers;
using WebApiContrib.Core.Razor;
using WebApiContrib.Core.Samples.Model;
using WebApiContrib.Core.Versioning;

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
            services.AddSingleton<IModelMapper<PersonModel>, PersonModelMapper>();

            services.AddMvc(o =>
            {
                o.AddJsonpOutputFormatter();
                o.UseFromBodyBinding(controllerPredicate: c => c.ControllerType.AsType() == typeof (BindingController));
            }).AddCsvSerializerFormatters()
                .AddPlainTextFormatters()
                .AddVersionNegotiation(x =>
                    x.UseStrategy<DefaultVersioningStrategy>(y =>
                        y.ParameterName = "apiVersion"));

            services.EnableAddTagHelperAssemblyGlobbing();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
