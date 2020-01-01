using System;
using ApplicationPipeline;
using Microsoft.AspNetCore.Builder;

namespace ApplicationPipeline.Tests
{
    public class SampleApplicationPipelineStepWithDI : ApplicationPipelineStepBase<SampleApplicationPipelineStepWithDI>
    {
        private IDummyService _dummyService;

        public SampleApplicationPipelineStepWithDI(IDummyService service)
        {
            _dummyService = service;
        }

        public override IApplicationBuilder Invoke(IApplicationBuilder input)
        {
            throw new NotImplementedException();
        }
    }
}
