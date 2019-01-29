using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiContrib.Core
{
    public static class ParallelApplicationPipelinesExtensions
    {
        /// <summary>
        /// Sets up an application branch with an isolated DI container
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <param name="path">Relative path for the application branch</param>
        /// <param name="servicesConfiguration">DI container configuration</param>
        /// <param name="appBuilderConfiguration">Application pipeline configuration for the created branch</param>
        public static IApplicationBuilder UseBranchWithServices(this IApplicationBuilder app, PathString path, 
            Action<IServiceCollection> servicesConfiguration, Action<IApplicationBuilder> appBuilderConfiguration)
        {
            return app.UseBranchWithServices(new[] { path }, servicesConfiguration, appBuilderConfiguration);
        }

        /// <summary>
        /// Sets up an application branch with an isolated DI container with several routes (entry points)
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <param name="path">Relative paths for the application branch</param>
        /// <param name="servicesConfiguration">DI container configuration</param>
        /// <param name="appBuilderConfiguration">Application pipeline configuration for the created branch</param>
        public static IApplicationBuilder UseBranchWithServices(this IApplicationBuilder app, IEnumerable<PathString> paths,
            Action<IServiceCollection> servicesConfiguration, Action<IApplicationBuilder> appBuilderConfiguration)
        {
            var webHost = new WebHostBuilder().
                ConfigureServices(s => {
                    s.AddSingleton<IServer, DummyServer>();
                }).
                ConfigureServices(servicesConfiguration).
                UseStartup<EmptyStartup>().
                Build();

            var serviceProvider = webHost.Services;
            var serverFeatures = webHost.ServerFeatures;

            var appBuilderFactory = serviceProvider.GetRequiredService<IApplicationBuilderFactory>();
            var branchBuilder = appBuilderFactory.CreateBuilder(serverFeatures);
            var factory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

            branchBuilder.Use(async (context, next) =>
            {
                using (var scope = factory.CreateScope())
                {
                    context.RequestServices = scope.ServiceProvider;

                    var httpContextAccessor = context.RequestServices
                                                     .GetService<IHttpContextAccessor>();

                    if (httpContextAccessor != null)
                        httpContextAccessor.HttpContext = context;

                    await next();
                }
            });

            appBuilderConfiguration(branchBuilder);

            var branchDelegate = branchBuilder.Build();

            foreach (var path in paths)
            {
                app.Map(path, builder =>
                {
                    builder.Use(async (context, next) =>
                    {
                        await branchDelegate(context);
                    });
                });
            }

            return app;
        }

        private class EmptyStartup
        {
            public void ConfigureServices(IServiceCollection services) {}

            public void Configure(IApplicationBuilder app) {}
        }

        private class DummyServer : IServer
        {
            public IFeatureCollection Features { get; } = new FeatureCollection();

            public void Dispose() {}

            public Task StartAsync<TContext>(IHttpApplication<TContext> application, CancellationToken cancellationToken) => Task.CompletedTask;

            public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        }
    }
}
