using System;
using ApplicationPipeline;
using ApplicationPipeline.Abstractions;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationPipelineServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationPipeline(
            this IServiceCollection services)
            => services.AddApplicationPipeline(null);

        public static IServiceCollection AddApplicationPipeline(
            this IServiceCollection services,
            Action<IApplicationPipelineConfigurar> pipelineConfig)
        {
            services.TryAddSingleton<AspNetApplicationPipeline>();
            services.TryAddSingleton<IApplicationPipeline>(c => c.GetService<AspNetApplicationPipeline>());

            services.Replace(ServiceDescriptor.Singleton(new ApplicationPipelineConfigurar(services)));
            services.Replace(ServiceDescriptor.Singleton<IApplicationPipelineConfigurar>(c => c.GetService<ApplicationPipelineConfigurar>()));

            var serviceProvider = services.BuildServiceProvider();

            var pipelineConfigurar = serviceProvider.GetService<ApplicationPipelineConfigurar>();
            pipelineConfig?.Invoke(pipelineConfigurar);

            var pipeline = serviceProvider.GetService<IApplicationPipeline>();
            pipelineConfigurar.Configure(pipeline);

            services.Replace(ServiceDescriptor.Singleton(pipeline));

            return services;
        }
    }
}
