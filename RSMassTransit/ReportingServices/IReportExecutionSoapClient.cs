using RSMassTransit.ReportingServices.Execution;

namespace RSMassTransit.ReportingServices
{
    internal interface IReportExecutionSoapClient : ReportExecutionServiceSoap { }

    namespace Execution
    {
        partial class ReportExecutionServiceSoapClient : IReportExecutionSoapClient { }
    }
}
