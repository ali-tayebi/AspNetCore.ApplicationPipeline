using Microsoft.AspNetCore.Http;

namespace HttpPipelines.WebApi.Sample.Tests
{
    public class HttpTransaction
    {
        public string TraceId { get; }
        public string Path { get; }
        public IHeaderDictionary Headers { get; }

        public HttpTransaction(string traceId, string path, IHeaderDictionary headers)
        {
            TraceId = traceId;
            Path = path;
            Headers = headers;
        }
    }
}