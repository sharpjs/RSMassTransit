using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MassTransit;

using RSMassTransit.Messages;

namespace RSMassTransit
{
    internal class ExecuteReportConsumer : IConsumer<IExecuteReportRequest>
    {
        public async Task Consume(ConsumeContext<IExecuteReportRequest> context)
        {
            var request  = context.Message;
            var response = new ExecuteReportResponse();

            await context.RespondAsync<IExecuteReportResponse>(response);
        }
    }
}
