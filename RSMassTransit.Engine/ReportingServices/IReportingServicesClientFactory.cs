// Copyright Subatomix Research Inc.
// SPDX-License-Identifier: MIT

using System.Net;
using RSMassTransit.ReportingServices.Execution;

namespace RSMassTransit.ReportingServices;

internal interface IReportingServicesClientFactory
{
    IReportExecutionSoapClient CreateExecutionClient(NetworkCredential? credential = null);
}
