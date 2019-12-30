using Microsoft.AspNetCore.Builder;
using Pipelines.Abstractions;

namespace Pipelines.Http
{
    public interface IHttpPipelineStep : IPipelineStep<IApplicationBuilder>
    {
    }
}