using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace WebApiContrib.Core.Versioning
{
    public static class VersionNegotiationExtensions
    {
        /// <summary>
        /// Adds version negotiation using the <see cref="DefaultVersioningStrategy"/>.
        /// </summary>
        public static IMvcBuilder AddVersionNegotiation(this IMvcBuilder builder)
        {
            return builder.AddVersionNegotiation(options => { });
        }

        /// <summary>
        /// Adds version negotiation using the <see cref="DefaultVersioningStrategy"/>.
        /// </summary>
        public static IMvcBuilder AddVersionNegotiation(this IMvcBuilder builder,
            Action<VersionNegotiationOptions> configure)
        {
            return builder.AddVersionNegotiation<DefaultVersioningStrategy>(configure);
        }

        /// <summary>
        /// Adds version negotiation using the <typeparamref cref="TStrategy"/>.
        /// </summary>
        public static IMvcBuilder AddVersionNegotiation<TStrategy>(this IMvcBuilder builder)
            where TStrategy : class, IVersioningStrategy
        {
            return builder.AddVersionNegotiation<TStrategy>(options => { });
        }

        /// <summary>
        /// Adds version negotiation using the <typeparamref cref="TStrategy"/>.
        /// </summary>
        public static IMvcBuilder AddVersionNegotiation<TStrategy>(this IMvcBuilder builder,
            Action<VersionNegotiationOptions> configure)
            where TStrategy : class, IVersioningStrategy
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.AddVersionNegotiation<TStrategy>(configure);
            return builder;
        }

        /// <summary>
        /// Adds version negotiation using the <see cref="DefaultVersioningStrategy"/>.
        /// </summary>
        public static IMvcCoreBuilder AddVersionNegotiation(this IMvcCoreBuilder builder)
        {
            return builder.AddVersionNegotiation(options => { });
        }

        /// <summary>
        /// Adds version negotiation using the <see cref="DefaultVersioningStrategy"/>.
        /// </summary>
        public static IMvcCoreBuilder AddVersionNegotiation(this IMvcCoreBuilder builder,
            Action<VersionNegotiationOptions> configure)
        {
            return builder.AddVersionNegotiation<DefaultVersioningStrategy>(configure);
        }

        /// <summary>
        /// Adds version negotiation using the <typeparamref cref="TStrategy"/>.
        /// </summary>
        public static IMvcCoreBuilder AddVersionNegotiation<TStrategy>(this IMvcCoreBuilder builder)
            where TStrategy : class, IVersioningStrategy
        {
            return builder.AddVersionNegotiation<TStrategy>(options => { });
        }

        /// <summary>
        /// Adds version negotiation using the <typeparamref cref="TStrategy"/>.
        /// </summary>
        public static IMvcCoreBuilder AddVersionNegotiation<TStrategy>(this IMvcCoreBuilder builder, Action<VersionNegotiationOptions> configure)
            where TStrategy : class, IVersioningStrategy
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.AddVersionNegotiation<TStrategy>(configure);
            return builder;
        }

        /// <summary>
        /// Adds version negotiation using the <see cref="DefaultVersioningStrategy"/>.
        /// </summary>
        public static IServiceCollection AddVersionNegotiation(this IServiceCollection services)
        {
            return services.AddVersionNegotiation(options => { });
        }

        /// <summary>
        /// Adds version negotiation using the <see cref="DefaultVersioningStrategy"/>.
        /// </summary>
        public static IServiceCollection AddVersionNegotiation(this IServiceCollection services, Action<VersionNegotiationOptions> configure)
        {
            return services.AddVersionNegotiation<DefaultVersioningStrategy>(configure);
        }

        /// <summary>
        /// Adds version negotiation using the <typeparamref cref="TStrategy"/>.
        /// </summary>
        public static IServiceCollection AddVersionNegotiation<TStrategy>(this IServiceCollection services)
            where TStrategy : class, IVersioningStrategy
        {
            return services.AddVersionNegotiation<TStrategy>(options => { });
        }

        /// <summary>
        /// Adds version negotiation using the <typeparamref cref="TStrategy"/>.
        /// </summary>
        public static IServiceCollection AddVersionNegotiation<TStrategy>(this IServiceCollection services, Action<VersionNegotiationOptions> configure)
            where TStrategy : class, IVersioningStrategy
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
                .AddSingleton<IVersioningStrategy, TStrategy>()
                .AddTransient<IConfigureOptions<MvcOptions>, ConfigureMvcOptions>();
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
                options.Filters.Add(new VersioningResultFilter(ServiceProvider, Options));
            }
        }
    }
}