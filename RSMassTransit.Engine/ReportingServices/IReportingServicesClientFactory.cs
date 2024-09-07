// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

using System.Net;
using RSMassTransit.ReportingServices.Execution;

namespace RSMassTransit.ReportingServices;

internal interface IReportingServicesClientFactory
{
    IReportExecutionSoapClient CreateExecutionClient(NetworkCredential? credential = null);
}
