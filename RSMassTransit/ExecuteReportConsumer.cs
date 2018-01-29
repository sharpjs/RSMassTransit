using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MassTransit;

using RSMassTransit.Messages;

namespace RSMassTransit
{
    internal class ExecuteReportConsumer : IConsumer<IExecuteReportRequest>
    {
        public Task Consume(ConsumeContext<IExecuteReportRequest> context)
        {
            //await context.RespondAsync<IExecuteReportResponse>(...);
            return Task.CompletedTask;
        }
    }
}
