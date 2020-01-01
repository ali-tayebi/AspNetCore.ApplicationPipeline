using Microsoft.AspNetCore.Builder;
using Pipelines.Abstractions;

namespace ApplicationPipeline.Abstractions
{
    public interface IApplicationPipelineStep : IPipelineStep<IApplicationBuilder>
    {
    }
}
