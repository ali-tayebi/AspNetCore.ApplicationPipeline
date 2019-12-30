using Microsoft.AspNetCore.Builder;
using Pipelines.Http;

namespace HttpPipelines.WebApi.Sample
{
    public class EndpointsStep : HttpPipelineStepBase<EndpointsStep>
    {
        public override IApplicationBuilder Invoke(IApplicationBuilder app) =>
            app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}