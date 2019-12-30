using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Pipelines.Http
{
    public interface IHttpPipelineConfigurar
    {
        IHttpPipelineConfigurar Add(IHttpPipelineStep step);
        IHttpPipelineConfigurar Add<TStep>() where TStep : HttpPipelineStepBase<TStep>;
        IHttpPipelineConfigurar Add(string key, Action<IApplicationBuilder> app);
        IHttpPipelineConfigurar Add(string key, Action<IWebHostEnvironment, IApplicationBuilder> app);
        IHttpPipelineConfigurar Add(Expression<Action<IApplicationBuilder>> app);
        IHttpPipelineConfigurar Add(Expression<Action<IWebHostEnvironment, IApplicationBuilder>> app);

        IHttpPipelineConfigurar AddAfter(string key, IHttpPipelineStep step);
        IHttpPipelineConfigurar AddAfter<TStep>(string key) where TStep : HttpPipelineStepBase<TStep>;

        IHttpPipelineConfigurar AddAfter<TExisting, TStep>()
            where TStep : HttpPipelineStepBase<TStep>
            where TExisting : HttpPipelineStepBase<TExisting>;

        IHttpPipelineConfigurar AddAfter(string key, Expression<Action<IApplicationBuilder>> app);
        IHttpPipelineConfigurar AddAfter(string key, Expression<Action<IWebHostEnvironment, IApplicationBuilder>> app);
        IHttpPipelineConfigurar AddBefore(string key, IHttpPipelineStep step);

        IHttpPipelineConfigurar AddBefore<TExisting, TStep>()
            where TStep : HttpPipelineStepBase<TStep>
            where TExisting : HttpPipelineStepBase<TExisting>;

        IHttpPipelineConfigurar AddBefore<TStep>(string key) where TStep : HttpPipelineStepBase<TStep>;
        IHttpPipelineConfigurar AddBefore(string key, Expression<Action<IApplicationBuilder>> app);
        IHttpPipelineConfigurar AddBefore(string key, Expression<Action<IWebHostEnvironment, IApplicationBuilder>> app);

        IHttpPipelineConfigurar Remove(string key);
        IHttpPipelineConfigurar Remove(IHttpPipelineStep step);
        IHttpPipelineConfigurar Remove<TStep>()  where TStep : HttpPipelineStepBase<TStep>;
    }
}
