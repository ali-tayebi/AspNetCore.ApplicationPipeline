using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Pipelines.Http;

namespace HttpPipelines
{
    public static class HttpPipelinesApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHttpPipeline(this IApplicationBuilder app)
        {
            var pipeline = app.ApplicationServices.GetService<IHttpPipeline>();
            pipeline.Execute(app);
            return app;
        }
    }
}