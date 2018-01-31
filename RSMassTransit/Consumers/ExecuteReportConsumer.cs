using System;
using System.Net;
using System.Threading.Tasks;
using MassTransit;

using RSMassTransit.Messages;
using RSMassTransit.ReportingServices;
using RSMassTransit.ReportingServices.Execution;

namespace RSMassTransit
{
    internal class ExecuteReportConsumer : IConsumer<IExecuteReportRequest>
    {
        private readonly IReportingServicesClientFactory _services;

        public ExecuteReportConsumer(IReportingServicesClientFactory services)
        {
            _services = services
                ?? throw new ArgumentNullException(nameof(services));
        }

        public async Task Consume(ConsumeContext<IExecuteReportRequest> context)
        {
            var request    = context.Message;
            var response   = new ExecuteReportResponse();
            var credential = request.GetNetworkCredential();

            using (var client = _services.CreateExecutionClient(credential))
            {
                var loadReportResponse = await client.LoadReport2Async(
                    new LoadReport2Request { Report = request.Path }
                );


                await client.SetExecutionParameters2Async(
                    new SetExecutionParameters2Request
                    {
                        
                    }
                );
            }

            await context.RespondAsync<IExecuteReportResponse>(response);
        }
    }
}
