// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

namespace RSMassTransit.ReportingServices.Execution;

internal interface IReportExecutionSoapClient
    : ReportExecutionServiceSoap
    , IDisposable
{ }
