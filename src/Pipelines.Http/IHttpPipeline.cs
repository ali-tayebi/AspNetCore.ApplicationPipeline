using Microsoft.AspNetCore.Builder;
using Pipelines.Abstractions;

namespace Pipelines.Http
{
    public interface IHttpPipeline : IPipeline<IHttpPipelineStep>, IExecute<IApplicationBuilder>
    {
    }
}