// Copyright (C) 2018 (to be determined)

using System.Net;

namespace RSMassTransit.ReportingServices
{
    internal interface IReportingServicesClientFactory
    {
        IReportExecutionSoapClient CreateExecutionClient(NetworkCredential credential = null);
    }
}
