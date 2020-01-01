using System;
using Microsoft.AspNetCore.Builder;
using ApplicationPipeline;

namespace ApplicationPipeline.WebApi.Sample
{
    public class TransactionIdStep : ApplicationPipelineStepBase<TransactionIdStep>
    {
        public override IApplicationBuilder Invoke(IApplicationBuilder app) =>
            app.Use(async (context, next) =>
            {
                var transactionId = Guid.NewGuid().ToString();

                if (context.Request.Headers.TryGetValue("Trx-Id", out var trxId) && !string.IsNullOrWhiteSpace(trxId))
                {
                    transactionId = "|" + trxId + "|" + transactionId;
                }

                context.TraceIdentifier = transactionId;

                await next.Invoke();
            });
    }
}
