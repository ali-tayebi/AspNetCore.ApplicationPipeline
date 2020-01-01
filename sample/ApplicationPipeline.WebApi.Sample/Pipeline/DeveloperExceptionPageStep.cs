using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ApplicationPipeline;

namespace ApplicationPipeline.WebApi.Sample
{
    public class DeveloperExceptionPageStep : ApplicationPipelineStepBase<DeveloperExceptionPageStep>
    {
        public override IApplicationBuilder Invoke(IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetService<IHostEnvironment>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            return app;
        }
    }
}
