// Copyright Subatomix Research Inc.
// SPDX-License-Identifier: MIT

namespace RSMassTransit.ReportingServices;

/// <summary>
///   Configuration for SSRS web service clients.
/// </summary>
public interface IReportingServicesClientConfiguration
{
    /// <summary>
    ///   Gets the SSRS report execution service endpoint URI.
    /// </summary>
    Uri ExecutionUri { get; }

    /// <summary>
    ///   Gets the maximum size of response to support from an SSRS web service.
    /// </summary>
    long MaxResponseSize { get; }

    /// <summary>
    ///   Gets the maximum duration to wait for a response from an SSRS web
    ///   service.
    /// </summary>
    TimeSpan Timeout { get; }
}
