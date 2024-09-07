// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

namespace RSMassTransit.Messages;

/// <summary>
///   Default implementation of <see cref="IExecuteReportResponse"/>.
/// </summary>
public class ExecuteReportResponse : IExecuteReportResponse
{
    /// <inheritdoc/>
    public Uri? Uri { get; set; }

    /// <inheritdoc/>
    public string? ContentType { get; set; }

    /// <inheritdoc/>
    public string? FileNameExtension { get; set; }

    /// <inheritdoc/>
    public long Length { get; set; }

    /// <inheritdoc/>
    public IList<string> Messages
    {
        get => _messages ??= [];
        set => _messages   = value;
    }
    private IList<string>? _messages;
}
