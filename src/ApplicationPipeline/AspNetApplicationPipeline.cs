using ApplicationPipeline.Abstractions;
using Microsoft.AspNetCore.Builder;

namespace ApplicationPipeline
{
    public class AspNetApplicationPipeline : Pipeline<IApplicationPipelineStep, IApplicationBuilder>, IApplicationPipeline
    {
    }
}
