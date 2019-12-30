using System;
using Microsoft.AspNetCore.Builder;

namespace Pipelines.Http.Tests
{
    public class SampleHttpPipelineStep : HttpPipelineStepBase<SampleHttpPipelineStep>
    {
        public override IApplicationBuilder Invoke(IApplicationBuilder input)
        {
            throw new NotImplementedException();
        }
    }
}