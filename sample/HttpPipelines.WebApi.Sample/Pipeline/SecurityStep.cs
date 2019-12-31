using Microsoft.AspNetCore.Builder;
using Pipelines.Http;

namespace HttpPipelines.WebApi.Sample
{
    public class SecurityStep : HttpPipelineStepBase<SecurityStep>
    {
        public override IApplicationBuilder Invoke(IApplicationBuilder app) =>
            app
                .UseAuthentication()
                .UseCors()
                .UseHsts();
    }
}
