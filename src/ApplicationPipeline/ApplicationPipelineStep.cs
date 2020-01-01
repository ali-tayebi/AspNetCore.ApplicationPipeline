using System;
using Microsoft.AspNetCore.Builder;

namespace ApplicationPipeline
{
    public class ApplicationPipelineStep : ApplicationPipelineStepBase
    {
        private readonly Action<IApplicationBuilder> _appBuilder;

        public ApplicationPipelineStep(string key, Action<IApplicationBuilder> appBuilder) : base(key)
        {
            _appBuilder = appBuilder ?? throw new ArgumentNullException(nameof(appBuilder));
        }

        public override IApplicationBuilder Invoke(IApplicationBuilder app)
        {
            _appBuilder?.Invoke(app);
            return app;
        }
    }
}