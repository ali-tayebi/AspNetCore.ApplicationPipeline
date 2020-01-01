using Microsoft.AspNetCore.Builder;
using ApplicationPipeline;

namespace ApplicationPipeline.WebApi.Sample
{
    public class EndpointsStep : ApplicationPipelineStepBase<EndpointsStep>
    {
        public override IApplicationBuilder Invoke(IApplicationBuilder app) =>
            app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
