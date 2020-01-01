using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace ApplicationPipeline.WebApi.Sample.Tests
{
    public class HttpTransactionRecorder
    {
        public IList<HttpTransaction> Transactions { get; }

        public HttpTransactionRecorder() => 
            Transactions = new List<HttpTransaction>();

        public void Record(HttpContext httpContext) =>
            Transactions.Add(new HttpTransaction(
                httpContext.TraceIdentifier, 
                httpContext.Request.Path, 
                httpContext.Request.Headers));
    }
}