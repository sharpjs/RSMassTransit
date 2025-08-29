// Copyright Subatomix Research Inc.
// SPDX-License-Identifier: MIT

namespace RSMassTransit.Messages;

/// <summary>
///   The response to a request to execute a report.
/// </summary>
public interface IExecuteReportResponse : IMessage
{
    /// <summary>
    ///   URI of the rendered report.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     The following URI types are valid:
    ///   </para>
    ///   <list type="bullet">
    ///     <item><c>file:</c></item>
    ///     <item><c>https:</c></item>
    ///   </list>
    /// </remarks>
    Uri? Uri { get; set; }

    /// <summary>
    ///   Content type (MIME type) of the rendered report.
    /// </summary>
    string? ContentType { get; set; }

    /// <summary>
    ///   File name extension of the rendered report.
    /// </summary>
    string? FileNameExtension { get; set; }

    /// <summary>
    ///   Length of the rendered report, in bytes.
    /// </summary>
    long Length { get; set; }

    /// <summary>
    ///   Diagnostic messages produced during report execution.
    /// </summary>
    IList<string> Messages { get; set; }
}
