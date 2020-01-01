using Microsoft.AspNetCore.Builder;

namespace ApplicationPipeline.Abstractions
{
    public interface IApplicationPipelineStep : IPipelineStep<IApplicationBuilder>
    {
    }
}
