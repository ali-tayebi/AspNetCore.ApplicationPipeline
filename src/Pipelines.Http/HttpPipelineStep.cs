using System;
using Microsoft.AspNetCore.Builder;

namespace Pipelines.Http
{
    public class HttpPipelineStep : HttpPipelineStepBase
    {
        private readonly Action<IApplicationBuilder> _appBuilder;

        public HttpPipelineStep(string key, Action<IApplicationBuilder> appBuilder) : base(key)
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