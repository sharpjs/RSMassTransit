using System.Net;

namespace RSMassTransit.ReportingServices
{
    internal interface IReportingServicesClientFactory
    {
        IReportExecutionSoapClient CreateExecutionClient(NetworkCredential credential = null);
    }
}
