using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApiContrib.Core.Filters;

namespace WebApiContrib.Core
{
    public static class ServiceCollectionExtensions
    {
        public static void EnableFilterOverriding(this IServiceCollection services)
        {
            services.AddSingleton<IFilterProvider, OverridableFilterProvider>();
        }

        public static TConfig ConfigurePOCO<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
        {
            var config = new TConfig();
            configuration.Bind(config);
            services.AddSingleton(config);
            return config;
        }
    }
}