using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Pipelines.Http
{
    public class HttpPipelineConfigurar  : IHttpPipelineConfigurar
    {
        private readonly IServiceCollection _services;
        private Action<IServiceCollection, IHttpPipeline> _pipelineSetup;
        
        public HttpPipelineConfigurar(IServiceCollection services)
        {
            _services = services;
        }
        
        #region Add
        public IHttpPipelineConfigurar Add(IHttpPipelineStep step)
        {
            _pipelineSetup += (sb, pipeline) => pipeline.Add(step);
            return this;
        }
        
        public IHttpPipelineConfigurar Add<TStep>() where TStep : HttpPipelineStepBase<TStep>
        {
            _pipelineSetup += (services, pipeline) =>
            {
                services.AddSingleton<TStep>();
                pipeline.Add(services.BuildServiceProvider().GetRequiredService<TStep>());
            };
            return this;
        }
        
        public IHttpPipelineConfigurar Add(string key, Action<IApplicationBuilder> app)
        {
            _pipelineSetup += (services, pipeline) => pipeline.Add(new HttpPipelineStep(key, app));
            return this;
        }
        
        public IHttpPipelineConfigurar Add(string key, Action<IWebHostEnvironment, IApplicationBuilder> app)
        {
            void Action(IApplicationBuilder builder)
            {
                var env = builder.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
                app.Invoke(env, builder);
            }
            return Add(new HttpPipelineStep(key, Action));
        }
        
        public IHttpPipelineConfigurar Add(Expression<Action<IWebHostEnvironment, IApplicationBuilder>> app)
        {
            void Action(IApplicationBuilder builder)
            {
                var env = builder.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
                app.Compile().Invoke(env, builder);
            }
            return Add(new HttpPipelineStep(app.ToString(), Action));
        }
        
        public IHttpPipelineConfigurar Add(Expression<Action<IApplicationBuilder>> app)
        {
            return Add(new HttpPipelineStep(app.ToString(), app.Compile()));
        }
        #endregion Add

        
        #region AddAfter
        public IHttpPipelineConfigurar AddAfter(string key, IHttpPipelineStep step)
        {
	        _pipelineSetup += (services, pipeline) => pipeline.AddAfter(key, step);
            return this;
        }

        public IHttpPipelineConfigurar AddAfter<TStep>(string key) where TStep: HttpPipelineStepBase<TStep>
        {
            _pipelineSetup += (services, pipeline) =>
            {
                services.AddSingleton<TStep>();
                pipeline.AddAfter(key, services.BuildServiceProvider().GetRequiredService<TStep>());
            };
            return this;
        }
        
        public IHttpPipelineConfigurar AddAfter<TExisting, TStep>() 
            where TStep: HttpPipelineStepBase<TStep>
            where TExisting: HttpPipelineStepBase<TExisting>
        {
            _pipelineSetup += (services, pipeline) =>
            {
                services.AddSingleton<TStep>();
                var sp = services.BuildServiceProvider();
                pipeline.AddAfter(sp.GetRequiredService<TExisting>().Key, sp.GetRequiredService<TStep>());
            };
            return this;
        }

        public IHttpPipelineConfigurar AddAfter(string key, Expression<Action<IApplicationBuilder>> app) => 
            AddAfter(key, new HttpPipelineStep(app.ToString(), app.Compile()));

        public IHttpPipelineConfigurar AddAfter(string key, Expression<Action<IWebHostEnvironment, IApplicationBuilder>> app)
        {
            void Action(IApplicationBuilder builder)
            {
                var env = builder.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
                app.Compile().Invoke(env, builder);
            }
            return AddAfter(key, new HttpPipelineStep(app.ToString(), Action));
        }
        #endregion AddAfter
        
        #region AddBefore
        public IHttpPipelineConfigurar AddBefore(string key, IHttpPipelineStep step)
        {
            _pipelineSetup += (sb, pipeline) =>  pipeline.AddBefore(key, step);
            return this;
        }

        public IHttpPipelineConfigurar AddBefore<TStep>(string key) where TStep: HttpPipelineStepBase<TStep>
        {
            _pipelineSetup += (services, pipeline) =>
            {
                services.AddSingleton<TStep>();
                pipeline.AddBefore(key, services.BuildServiceProvider().GetRequiredService<TStep>());
            };
            return this;
        }
        
        public IHttpPipelineConfigurar AddBefore<TExisting, TStep>() 
            where TStep: HttpPipelineStepBase<TStep>
            where TExisting: HttpPipelineStepBase<TExisting>
        {
            _pipelineSetup += (services, pipeline) =>
            {
                services.AddSingleton<TStep>();
                var sp = services.BuildServiceProvider();
                pipeline.AddBefore(sp.GetRequiredService<TExisting>().Key, sp.GetRequiredService<TStep>());
            };
            return this;
        }
        
        public IHttpPipelineConfigurar AddBefore(string key, Expression<Action<IApplicationBuilder>> app) =>
            AddBefore(key, new HttpPipelineStep(app.ToString(), app.Compile()));
        
        public IHttpPipelineConfigurar AddBefore(string key, Expression<Action<IWebHostEnvironment, IApplicationBuilder>> app)
        {
            void Action(IApplicationBuilder builder)
            {
                var env = builder.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
                app.Compile().Invoke(env, builder);
            }
            return AddBefore(key, new HttpPipelineStep(app.ToString(), Action));
        }
        #endregion AddBefore
        
        #region Remove

        public IHttpPipelineConfigurar Remove(string key)
        {
            _pipelineSetup += (services, pipeline) => pipeline.RemoveWithKey(key);
            return this;
        }
        
        public IHttpPipelineConfigurar Remove(IHttpPipelineStep step)
        {
            _pipelineSetup += (services, pipeline) =>  pipeline.Remove(step);;
            return this;
        }
        
        public IHttpPipelineConfigurar Remove<TStep>() where TStep : HttpPipelineStepBase<TStep>
        {
            _pipelineSetup += (services, pipeline) =>  pipeline.RemoveWithKey(services.BuildServiceProvider().GetService<TStep>()?.Key);
            return this;
        }
        #endregion Remove
        
        public void Configure(IHttpPipeline pipeline)
        {
            _pipelineSetup?.Invoke(_services, pipeline);
        }
    }
}