using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApiContrib.Core.Concurrency.Extensions;
using WebApiContrib.Core.Concurrency.SqlServer;
using WebApiContrib.Core.Concurrency.Redis;

namespace WebApiContrib.Core.Concurrency.Integration.Tests
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // InMemory Storage
            // services.AddConcurrency(opt => opt.UseInMemoryStorage());
            // SqlServer storage
            /*
            services.AddConcurrency(opt => opt.UseSqlServer(o =>
            {
                o.ConnectionString = "Data Source=.;Initial Catalog=Caching;User Id=user;Password=Password19890;";
                o.SchemaName = "dbo";
                o.TableName = "MyAppCache";
            }));
            */
            // Redis storage
            // Install redis on your machine + run the command redis-server + run command redis-cli
            services.AddConcurrency(opt => opt.UseRedis(o =>
            {   
                o.Configuration = "localhost";
                o.InstanceName = "SampleInstance";
            }));
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            app.UseMvc();
        }
    }
}
