using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using HttpMethod = System.Net.Http.HttpMethod;

namespace ApplicationPipeline.WebApi.Sample.Tests
{
    public class TransactionIdTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpTransactionRecorder _recorder;

        public TransactionIdTests(WebApplicationFactory<Startup> factory)
        {
            _recorder = new HttpTransactionRecorder();

            _factory = factory.WithWebHostBuilder(host =>
                host.ConfigureTestServices(services =>
                {
                    services
                        .AddSingleton(_recorder)
                        .AddApplicationPipeline(pipeline =>
                        {
                            pipeline.AddBefore<EndpointsStep, HttpTransactionRecorderStep>();
                        });
                }));
        }

        [Fact]
        public async Task EnsureTransactionIdIsGenerated()
        {
            var client = _factory.CreateClient();
            const string URL = "/WeatherForecast";
            const string EgressTrxId = "xyz";

            var request = new HttpRequestMessage(HttpMethod.Get, URL);
            request.Headers.Add("Trx-Id", EgressTrxId);

            var response = await client.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            _recorder.Transactions.Count.Should().Be(1);

            var trx = _recorder.Transactions.First();
            trx.Path.Should().Be(URL);
            trx.TraceId.Should().StartWith($"|{EgressTrxId}|");
            trx.TraceId.Replace($"|{EgressTrxId}|", string.Empty).Cast<Guid>().Should().NotBeNull();
        }
    }
}
