using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Pipelines.Http
{
    public static class HttpPipelinesServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpPipeline(
            this IServiceCollection services)
            => services.AddHttpPipeline(null);
        
        public static IServiceCollection AddHttpPipeline(
            this IServiceCollection services,
            Action<IHttpPipelineConfigurar> pipelineConfig)
        {
            services.TryAddSingleton<HttpPipeline>();
            services.TryAddSingleton<IHttpPipeline>(c => c.GetService<HttpPipeline>());

            services.Replace(ServiceDescriptor.Singleton(new HttpPipelineConfigurar(services)));
            services.Replace(ServiceDescriptor.Singleton<IHttpPipelineConfigurar>(c => c.GetService<HttpPipelineConfigurar>()));

            var serviceProvider = services.BuildServiceProvider();

            var pipelineConfigurar = serviceProvider.GetService<HttpPipelineConfigurar>();
            pipelineConfig?.Invoke(pipelineConfigurar);

            var pipeline = serviceProvider.GetService<IHttpPipeline>();
            pipelineConfigurar.Configure(pipeline);
           
            services.Replace(ServiceDescriptor.Singleton(pipeline));
            
            return services;
        }
    }
}