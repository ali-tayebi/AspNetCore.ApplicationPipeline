using ApplicationPipeline.Abstractions;
using Microsoft.AspNetCore.Builder;
using Pipelines;

namespace ApplicationPipeline
{
    public abstract class ApplicationPipelineStepBase : PipelineStepBase<IApplicationBuilder>, IApplicationPipelineStep
    {
        protected ApplicationPipelineStepBase(string key) : base(key)
        {
        }
    }

    public abstract class ApplicationPipelineStepBase<T> : ApplicationPipelineStepBase
    {
        public static string StaticKey => typeof(T).FullName;

        protected ApplicationPipelineStepBase() : base(StaticKey)
        {
        }
    }
}
