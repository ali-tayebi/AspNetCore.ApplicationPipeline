using System;
using Microsoft.AspNetCore.Builder;

namespace Pipelines.Http.Tests
{
    public class SampleHttpPipelineStepWithDI : HttpPipelineStepBase<SampleHttpPipelineStepWithDI>
    {
        private IDummyService _dummyService;

        public SampleHttpPipelineStepWithDI(IDummyService service)
        {
            _dummyService = service;
        }

        public override IApplicationBuilder Invoke(IApplicationBuilder input)
        {
            throw new NotImplementedException();
        }
    }
}