using System;
using System.Linq.Expressions;
using ApplicationPipeline.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationPipeline
{
    public class ApplicationPipelineConfigurar  : IApplicationPipelineConfigurar
    {
        private readonly IServiceCollection _services;
        private Action<IServiceCollection, IApplicationPipeline> _pipelineSetup;

        public ApplicationPipelineConfigurar(IServiceCollection services)
        {
            _services = services;
        }

        #region Add
        public IApplicationPipelineConfigurar Add(IApplicationPipelineStep step)
        {
            _pipelineSetup += (sb, pipeline) => pipeline.Add(step);
            return this;
        }

        public IApplicationPipelineConfigurar Add<TStep>() where TStep : ApplicationPipelineStepBase<TStep>
        {
            _pipelineSetup += (services, pipeline) =>
            {
                services.AddSingleton<TStep>();
                pipeline.Add(services.BuildServiceProvider().GetRequiredService<TStep>());
            };
            return this;
        }

        public IApplicationPipelineConfigurar Add(string key, Action<IApplicationBuilder> app)
        {
            _pipelineSetup += (services, pipeline) => pipeline.Add(new ApplicationPipelineStep(key, app));
            return this;
        }

        public IApplicationPipelineConfigurar Add(string key, Action<IWebHostEnvironment, IApplicationBuilder> app)
        {
            void Action(IApplicationBuilder builder)
            {
                var env = builder.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
                app.Invoke(env, builder);
            }
            return Add(new ApplicationPipelineStep(key, Action));
        }

        public IApplicationPipelineConfigurar Add(Expression<Action<IWebHostEnvironment, IApplicationBuilder>> app)
        {
            void Action(IApplicationBuilder builder)
            {
                var env = builder.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
                app.Compile().Invoke(env, builder);
            }
            return Add(new ApplicationPipelineStep(app.ToString(), Action));
        }

        public IApplicationPipelineConfigurar Add(Expression<Action<IApplicationBuilder>> app)
        {
            return Add(new ApplicationPipelineStep(app.ToString(), app.Compile()));
        }
        #endregion Add

        #region AddAfter
        public IApplicationPipelineConfigurar AddAfter(string key, IApplicationPipelineStep step)
        {
	        _pipelineSetup += (services, pipeline) => pipeline.AddAfter(key, step);
            return this;
        }

        public IApplicationPipelineConfigurar AddAfter<TStep>(string key) where TStep: ApplicationPipelineStepBase<TStep>
        {
            _pipelineSetup += (services, pipeline) =>
            {
                services.AddSingleton<TStep>();
                pipeline.AddAfter(key, services.BuildServiceProvider().GetRequiredService<TStep>());
            };
            return this;
        }

        public IApplicationPipelineConfigurar AddAfter<TExisting, TStep>()
            where TStep: ApplicationPipelineStepBase<TStep>
            where TExisting: ApplicationPipelineStepBase<TExisting>
        {
            _pipelineSetup += (services, pipeline) =>
            {
                services.AddSingleton<TStep>();
                var sp = services.BuildServiceProvider();
                pipeline.AddAfter(sp.GetRequiredService<TExisting>().Key, sp.GetRequiredService<TStep>());
            };
            return this;
        }

        public IApplicationPipelineConfigurar AddAfter(string key, Expression<Action<IApplicationBuilder>> app) =>
            AddAfter(key, new ApplicationPipelineStep(app.ToString(), app.Compile()));

        public IApplicationPipelineConfigurar AddAfter(string key, Expression<Action<IWebHostEnvironment, IApplicationBuilder>> app)
        {
            void Action(IApplicationBuilder builder)
            {
                var env = builder.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
                app.Compile().Invoke(env, builder);
            }
            return AddAfter(key, new ApplicationPipelineStep(app.ToString(), Action));
        }
        #endregion AddAfter

        #region AddBefore
        public IApplicationPipelineConfigurar AddBefore(string key, IApplicationPipelineStep step)
        {
            _pipelineSetup += (sb, pipeline) =>  pipeline.AddBefore(key, step);
            return this;
        }

        public IApplicationPipelineConfigurar AddBefore<TStep>(string key) where TStep: ApplicationPipelineStepBase<TStep>
        {
            _pipelineSetup += (services, pipeline) =>
            {
                services.AddSingleton<TStep>();
                pipeline.AddBefore(key, services.BuildServiceProvider().GetRequiredService<TStep>());
            };
            return this;
        }

        public IApplicationPipelineConfigurar AddBefore<TExisting, TStep>()
            where TStep: ApplicationPipelineStepBase<TStep>
            where TExisting: ApplicationPipelineStepBase<TExisting>
        {
            _pipelineSetup += (services, pipeline) =>
            {
                services.AddSingleton<TStep>();
                var sp = services.BuildServiceProvider();
                pipeline.AddBefore(sp.GetRequiredService<TExisting>().Key, sp.GetRequiredService<TStep>());
            };
            return this;
        }

        public IApplicationPipelineConfigurar AddBefore(string key, Expression<Action<IApplicationBuilder>> app) =>
            AddBefore(key, new ApplicationPipelineStep(app.ToString(), app.Compile()));

        public IApplicationPipelineConfigurar AddBefore(string key, Expression<Action<IWebHostEnvironment, IApplicationBuilder>> app)
        {
            void Action(IApplicationBuilder builder)
            {
                var env = builder.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
                app.Compile().Invoke(env, builder);
            }
            return AddBefore(key, new ApplicationPipelineStep(app.ToString(), Action));
        }
        #endregion AddBefore

        #region Remove

        public IApplicationPipelineConfigurar Remove(string key)
        {
            _pipelineSetup += (services, pipeline) => pipeline.RemoveWithKey(key);
            return this;
        }

        public IApplicationPipelineConfigurar Remove(IApplicationPipelineStep step)
        {
            _pipelineSetup += (services, pipeline) =>  pipeline.Remove(step);;
            return this;
        }

        public IApplicationPipelineConfigurar Remove<TStep>() where TStep : ApplicationPipelineStepBase<TStep>
        {
            _pipelineSetup += (services, pipeline) =>  pipeline.RemoveWithKey(services.BuildServiceProvider().GetService<TStep>()?.Key);
            return this;
        }
        #endregion Remove

        public void Configure(IApplicationPipeline pipeline)
        {
            _pipelineSetup?.Invoke(_services, pipeline);
        }
    }
}
