using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationPipeline.WebApi.Sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddApplicationPipeline(pipeline =>
            {
                pipeline
                    .Add<DeveloperExceptionPageStep>()
                    .Add<TransactionIdStep>()
                    .Add(app => app.UseHttpsRedirection())
                    .Add("routing-step", app => app.UseRouting())
                    .Add<SecurityStep>()
                    .Add<EndpointsStep>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseApplicationPipeline();
        }
    }
}
