using System;
using ApplicationPipeline;
using Microsoft.AspNetCore.Builder;

namespace ApplicationPipeline.Tests
{
    public class SampleApplicationPipelineStep : ApplicationPipelineStepBase<SampleApplicationPipelineStep>
    {
        public override IApplicationBuilder Invoke(IApplicationBuilder input)
        {
            throw new NotImplementedException();
        }
    }
}
