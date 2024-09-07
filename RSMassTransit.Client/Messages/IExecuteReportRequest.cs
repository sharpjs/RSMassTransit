// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

namespace RSMassTransit.Messages;

/// <summary>
///   A request to execute a report.
/// </summary>
public interface IExecuteReportRequest : ICredential, IMessage
{
    /// <summary>
    ///   Virtual path of the report on the report server.
    ///   Example: <c>"/My Reports/Contoso/BalanceSheet"</c>
    /// </summary>
    string? Path { get; set; }

    /// <summary>
    ///   Values for the report's parameters.
    /// </summary>
    /// <remarks>
    ///   To provide multiple values for one parameter, populate this
    ///   collection with multiple key-value pairs for that parameter name.
    /// </remarks>
    IList<KeyValuePair<string, string>> ParameterValues { get; set; }

    /// <summary>
    ///   Language and locale used to interpret parameter values such as
    ///   dates and numbers.  Example: <c>"en-US"</c>
    /// </summary>
    string? ParameterLanguage { get; set; }

    /// <summary>
    ///   Format in which to render the report.
    /// </summary>
    ReportFormat Format { get; set; }
}
