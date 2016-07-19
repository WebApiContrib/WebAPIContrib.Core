using Microsoft.AspNetCore.Builder;

namespace WebApiContrib.Core.Concurrency.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseConcurrency(this IApplicationBuilder applicationBuilder)
        {
            // return applicationBuilder.UseMiddleware<ConcurrencyMiddleware>();
            return applicationBuilder;
        }
    }
}
