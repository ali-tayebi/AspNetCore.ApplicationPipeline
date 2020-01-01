using Microsoft.AspNetCore.Builder;
using ApplicationPipeline;

namespace ApplicationPipeline.WebApi.Sample
{
    public class SecurityStep : ApplicationPipelineStepBase<SecurityStep>
    {
        public override IApplicationBuilder Invoke(IApplicationBuilder app) =>
            app
                .UseAuthentication()
                .UseCors()
                .UseHsts();
    }
}
