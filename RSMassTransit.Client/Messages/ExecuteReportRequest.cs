// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

namespace RSMassTransit.Messages;

/// <summary>
///   Default implementation of <see cref="IExecuteReportRequest"/>.
/// </summary>
public class ExecuteReportRequest : IExecuteReportRequest
{
    /// <inheritdoc/>
    public string? Path { get; set; }

    /// <inheritdoc/>
    public IList<KeyValuePair<string, string>> ParameterValues
    {
        get => _parameterValues ??= [];
        set => _parameterValues   = value;
    }
    private IList<KeyValuePair<string, string>>? _parameterValues;

    /// <inheritdoc/>
    public string? ParameterLanguage { get; set; }

    /// <inheritdoc/>
    public ReportFormat Format { get; set; }

    /// <inheritdoc/>
    public string? UserName { get; set; }

    /// <inheritdoc/>
    public string? Password { get; set; }
}
