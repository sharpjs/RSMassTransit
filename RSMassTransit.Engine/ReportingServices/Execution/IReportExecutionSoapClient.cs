// Copyright Subatomix Research Inc.
// SPDX-License-Identifier: MIT

namespace RSMassTransit.ReportingServices.Execution;

internal interface IReportExecutionSoapClient
    : ReportExecutionServiceSoap
    , IDisposable
{ }
