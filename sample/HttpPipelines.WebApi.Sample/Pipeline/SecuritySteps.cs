using Microsoft.AspNetCore.Builder;
using Pipelines.Http;

namespace HttpPipelines.WebApi.Sample
{
    public class SecuritySteps : HttpPipelineStepBase<SecuritySteps>
    {
        public override IApplicationBuilder Invoke(IApplicationBuilder app) =>
            app
                .UseAuthentication()
                .UseCors()
                .UseHsts();
    }
}