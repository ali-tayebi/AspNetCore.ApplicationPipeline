using Microsoft.AspNetCore.Builder;
using Pipelines.Abstractions;

namespace ApplicationPipeline.Abstractions
{
    public interface IApplicationPipeline : IPipeline<IApplicationPipelineStep>, IExecute<IApplicationBuilder>
    {
    }
}
