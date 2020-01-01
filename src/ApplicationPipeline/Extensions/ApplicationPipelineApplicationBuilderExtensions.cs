using Microsoft.Extensions.DependencyInjection;
using ApplicationPipeline;
using ApplicationPipeline.Abstractions;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationPipelineApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseApplicationPipeline(this IApplicationBuilder app)
        {
            var pipeline = app.ApplicationServices.GetService<IApplicationPipeline>();
            pipeline.Execute(app);
            return app;
        }
    }
}
