// Copyright Subatomix Research Inc.
// SPDX-License-Identifier: MIT

using RSMassTransit.Messages;

namespace RSMassTransit.Client;

/// <summary>
///   Interface to SQL Server Reporting Services provided by RSMassTransit
///   clients.
/// </summary>
public interface IReportingServices : IDisposable
{
    /// <summary>
    ///   Executes a report.
    /// </summary>
    /// <param name="request">
    ///   The parameters for report execution.
    /// </param>
    /// <param name="timeout">
    ///   The duration after which the client will cease waiting for a
    ///   response and throw an exception.  If this parameter is
    ///   <c>null</c>, this method will use the timeout specified by the
    ///   <see cref="ReportingServicesConfiguration.RequestTimeout"/>
    ///   property.
    /// </param>
    /// <returns>
    ///   The result of report execution.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="request"/> is <see langword="null"/>.
    /// </exception>
    IExecuteReportResponse ExecuteReport(
        IExecuteReportRequest request,
        TimeSpan?             timeout = default);

    /// <summary>
    ///   Executes a report asynchronously.
    /// </summary>
    /// <param name="request">
    ///   The parameters for report execution.
    /// </param>
    /// <param name="timeout">
    ///   The duration after which the client will cease waiting for a
    ///   response and throw an exception.  If this parameter is
    ///   <c>null</c>, this method will use the timeout specified by the
    ///   <see cref="ReportingServicesConfiguration.RequestTimeout"/>
    ///   property.
    /// </param>
    /// <param name="cancellationToken">
    ///   A token that can cancel the operation.
    /// </param>
    /// <returns>
    ///   A task representing the asynchronous operation.
    ///   The task's <c>Result</c> property returns the result of report execution.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="request"/> is <see langword="null"/>.
    /// </exception>
    Task<IExecuteReportResponse> ExecuteReportAsync(
        IExecuteReportRequest request,
        TimeSpan?             timeout           = default,
        CancellationToken     cancellationToken = default);
}
