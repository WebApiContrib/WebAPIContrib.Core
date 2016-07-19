using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace WebApiContrib.Core.Concurrency.Integration.Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {            
			var configuration = new ConfigurationBuilder()
                .AddCommandLine(args)
                .AddEnvironmentVariables(prefix: "ASPNETCORE_")
                .Build();
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseConfiguration(configuration)
                .UseStartup<Startup>()
                .Build();
            host.Run();
        }
    }
}
