using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace ApplicationPipeline.Abstractions
{
    public interface IApplicationPipelineConfigurar
    {
        IApplicationPipelineConfigurar Add(IApplicationPipelineStep step);
        IApplicationPipelineConfigurar Add<TStep>() where TStep : ApplicationPipelineStepBase<TStep>;
        IApplicationPipelineConfigurar Add(string key, Action<IApplicationBuilder> app);
        IApplicationPipelineConfigurar Add(string key, Action<IWebHostEnvironment, IApplicationBuilder> app);
        IApplicationPipelineConfigurar Add(Expression<Action<IApplicationBuilder>> app);
        IApplicationPipelineConfigurar Add(Expression<Action<IWebHostEnvironment, IApplicationBuilder>> app);

        IApplicationPipelineConfigurar AddAfter(string key, IApplicationPipelineStep step);
        IApplicationPipelineConfigurar AddAfter<TStep>(string key) where TStep : ApplicationPipelineStepBase<TStep>;

        IApplicationPipelineConfigurar AddAfter<TExisting, TStep>()
            where TStep : ApplicationPipelineStepBase<TStep>
            where TExisting : ApplicationPipelineStepBase<TExisting>;

        IApplicationPipelineConfigurar AddAfter(string key, Expression<Action<IApplicationBuilder>> app);
        IApplicationPipelineConfigurar AddAfter(string key, Expression<Action<IWebHostEnvironment, IApplicationBuilder>> app);
        IApplicationPipelineConfigurar AddBefore(string key, IApplicationPipelineStep step);

        IApplicationPipelineConfigurar AddBefore<TExisting, TStep>()
            where TStep : ApplicationPipelineStepBase<TStep>
            where TExisting : ApplicationPipelineStepBase<TExisting>;

        IApplicationPipelineConfigurar AddBefore<TStep>(string key) where TStep : ApplicationPipelineStepBase<TStep>;
        IApplicationPipelineConfigurar AddBefore(string key, Expression<Action<IApplicationBuilder>> app);
        IApplicationPipelineConfigurar AddBefore(string key, Expression<Action<IWebHostEnvironment, IApplicationBuilder>> app);

        IApplicationPipelineConfigurar Remove(string key);
        IApplicationPipelineConfigurar Remove(IApplicationPipelineStep step);
        IApplicationPipelineConfigurar Remove<TStep>()  where TStep : ApplicationPipelineStepBase<TStep>;
    }
}
