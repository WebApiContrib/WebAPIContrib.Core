using Microsoft.Extensions.DependencyInjection;
using System;
using WebApiContrib.Core.Concurrency;

namespace WebApiContrib.Core.Concurrency.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConcurrency(
            this IServiceCollection serviceCollection,
            Action<ConcurrencyOptionsBuilder> callback)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            var builder = new ConcurrencyOptionsBuilder();
            callback(builder);
            serviceCollection.AddSingleton(builder.ConcurrencyOptions);
            serviceCollection.AddTransient<IConcurrencyManager, ConcurrencyManager>();
            serviceCollection.AddTransient<IRepresentationManager, RepresentationManager>();
            return serviceCollection;
        }
    }
}
