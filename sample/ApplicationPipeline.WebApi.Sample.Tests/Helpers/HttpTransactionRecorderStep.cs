using ApplicationPipeline;
using Microsoft.AspNetCore.Builder;

namespace ApplicationPipeline.WebApi.Sample.Tests
{
    public class HttpTransactionRecorderStep : ApplicationPipelineStepBase<HttpTransactionRecorderStep>
    {
        private readonly HttpTransactionRecorder _recorder;

        public HttpTransactionRecorderStep(HttpTransactionRecorder recorder) =>
            _recorder = recorder;

        public override IApplicationBuilder Invoke(IApplicationBuilder app) =>
            app.Use(async (context, next) =>
            {
                _recorder.Record(context);

                await next.Invoke();
            });
    }
}
