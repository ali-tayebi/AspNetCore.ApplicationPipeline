using Microsoft.AspNetCore.Builder;

namespace ApplicationPipeline.Abstractions
{
    public interface IApplicationPipeline : IPipeline<IApplicationPipelineStep>, IExecute<IApplicationBuilder>
    {
    }
}
