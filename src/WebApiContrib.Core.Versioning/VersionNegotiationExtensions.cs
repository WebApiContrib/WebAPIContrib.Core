using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace WebApiContrib.Core.Versioning
{
    public static class VersionNegotiationExtensions
    {
        /// <summary>
        /// Adds version negotiation.
        /// </summary>
        public static IMvcBuilder AddVersionNegotiation(this IMvcBuilder builder)
        {
            return builder.AddVersionNegotiation(options => { });
        }

        /// <summary>
        /// Adds version negotiation.
        /// </summary>
        public static IMvcBuilder AddVersionNegotiation(this IMvcBuilder builder, Action<VersionNegotiationOptions> configure)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.AddVersionNegotiation(configure);
            return builder;
        }

        /// <summary>
        /// Adds version negotiation.
        /// </summary>
        public static IMvcCoreBuilder AddVersionNegotiation(this IMvcCoreBuilder builder)
        {
            return builder.AddVersionNegotiation(options => { });
        }

        /// <summary>
        /// Adds version negotiation.
        /// </summary>
        public static IMvcCoreBuilder AddVersionNegotiation(this IMvcCoreBuilder builder, Action<VersionNegotiationOptions> configure)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.AddVersionNegotiation(configure);
            return builder;
        }

        /// <summary>
        /// Adds version negotiation.
        /// </summary>
        public static IServiceCollection AddVersionNegotiation(this IServiceCollection services)
        {
            return services.AddVersionNegotiation(options => { });
        }

        /// <summary>
        /// Adds version negotiation.
        /// </summary>
        public static IServiceCollection AddVersionNegotiation(this IServiceCollection services, Action<VersionNegotiationOptions> configure)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            return services
                .Configure(configure)
                .AddTransient<IConfigureOptions<MvcOptions>, ConfigureMvcOptions>()
                .AddScoped(GetConfiguredVersionStrategy);
        }

        private static IVersionStrategy GetConfiguredVersionStrategy(IServiceProvider provider)
        {
            var options = provider.GetRequiredService<IOptions<VersionNegotiationOptions>>();

            var strategies = GetVersionStrategies(provider, options);

            return new CompositeVersionStrategy(strategies);
        }

        private static IEnumerable<IVersionStrategy> GetVersionStrategies(IServiceProvider provider, IOptions<VersionNegotiationOptions> options)
        {
            foreach (var strategyType in options.Value.StrategyTypes)
            {
                // We use ActivatorUtilities.CreateInstance to create the type,
                // but get its constructor arguments from the provider. This allows
                // you to inject whatever services you need in the versioning strategy.
                var strategy = (IVersionStrategy) ActivatorUtilities.CreateInstance(provider, strategyType);

                Action<object> configure;
                if (options.Value.ConfigureStrategy.TryGetValue(strategyType, out configure))
                {
                    configure(strategy);
                }

                yield return strategy;
            }
        }

        /// <summary>
        /// We use this class in order to get an instance of <see cref="IServiceProvider"/>
        /// that we can pass to the <see cref="VersioningResultFilter"/>.
        /// </summary>
        private class ConfigureMvcOptions : IConfigureOptions<MvcOptions>
        {
            public ConfigureMvcOptions(IServiceProvider serviceProvider, IOptions<VersionNegotiationOptions> options)
            {
                ServiceProvider = serviceProvider;
                Options = options;
            }

            private IServiceProvider ServiceProvider { get; }

            private IOptions<VersionNegotiationOptions> Options { get; }

            public void Configure(MvcOptions options)
            {
                if (Options.Value.StrategyTypes.Count == 0)
                {
                    // If a strategy type hasn't been set, use DefaultVersionStrategy.
                    Options.Value.UseStrategy<DefaultVersionStrategy>();
                }

                options.Filters.Add(new VersioningResultFilter(ServiceProvider, Options));
            }
        }
    }
}