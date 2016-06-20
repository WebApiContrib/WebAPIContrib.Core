using Microsoft.AspNetCore.Mvc.Filters;
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
    }
}