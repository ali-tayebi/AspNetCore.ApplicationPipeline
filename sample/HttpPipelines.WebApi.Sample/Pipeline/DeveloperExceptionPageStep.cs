using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pipelines.Http;

namespace HttpPipelines.WebApi.Sample
{
    public class DeveloperExceptionPageStep : HttpPipelineStepBase<DeveloperExceptionPageStep>
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