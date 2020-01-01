using ApplicationPipeline.Abstractions;
using Microsoft.AspNetCore.Builder;
using Pipelines;

namespace ApplicationPipeline
{
    public class AspNetApplicationPipeline : Pipeline<IApplicationPipelineStep, IApplicationBuilder>, IApplicationPipeline
    {
    }
}
