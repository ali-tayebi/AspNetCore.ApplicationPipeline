using System;

namespace ApplicationPipeline.Tests
{
    public class SamplePipelineStep : PipelineStepBase<int>
    {
        public SamplePipelineStep(string name) : base(name)
        {
        }

        public override int Invoke(int input)
        {
            throw new NotImplementedException();
        }
    }
}
