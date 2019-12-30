using Microsoft.AspNetCore.Builder;
using Pipelines.Http;

namespace HttpPipelines.WebApi.Sample.Tests
{
    public class HttpTransactionRecorderStep : HttpPipelineStepBase<HttpTransactionRecorderStep>
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