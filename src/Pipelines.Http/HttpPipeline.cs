using Microsoft.AspNetCore.Builder;

namespace Pipelines.Http
{
    public class HttpPipeline : Pipeline<IHttpPipelineStep, IApplicationBuilder>, IHttpPipeline
    {
    }
}