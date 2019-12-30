using Microsoft.AspNetCore.Builder;

namespace Pipelines.Http
{
    public abstract class HttpPipelineStepBase : PipelineStepBase<IApplicationBuilder>, IHttpPipelineStep
    {
        protected HttpPipelineStepBase(string key) : base(key)
        {
        }
    }
    
    public abstract class HttpPipelineStepBase<T> : HttpPipelineStepBase
    {
        public static string StaticKey => typeof(T).FullName;
        
        protected HttpPipelineStepBase() : base(StaticKey)
        {
        }
    }
}