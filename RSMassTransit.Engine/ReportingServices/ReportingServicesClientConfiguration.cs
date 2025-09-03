// Copyright Subatomix Research Inc.
// SPDX-License-Identifier: MIT

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;

namespace RSMassTransit.ReportingServices;

using static TimeSpan;

/// <inheritdoc cref="IReportingServicesClientConfiguration"/>
public class ReportingServicesClientConfiguration : IReportingServicesClientConfiguration
{
    public const string
        DefaultExecutionUri = "http://localhost:80/ReportServer/ReportExecution2005.asmx";

    public const long
        DefaultMaxReceivedMessageSize = 1024 * 1024 * 1024, // 1 GiB
        DefaultTimeoutTicks           = 4 * TicksPerHour + 30 * TicksPerSecond;
    // Just a bit longer than a 4-hour SSRS report timeout ^^ 

    /// <summary>
    ///   Initializes a new <see cref="ReportingServicesClientConfiguration"/>
    ///   instance with values from the specified configuration source.
    /// </summary>
    /// <param name="configuration">
    ///   The configuration source from which to load values.
    /// </param>
    public ReportingServicesClientConfiguration(IConfiguration configuration)
    {
        Load(configuration);
    }

    /// <inheritdoc/>
    public Uri ExecutionUri { get; set; }

    /// <inheritdoc/>
    public long MaxResponseSize { get; set; }

    /// <inheritdoc/>
    public TimeSpan Timeout { get; set; }

    [MemberNotNull(nameof(ExecutionUri))]
    private void Load(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        ExecutionUri
            =  configuration.GetUri(nameof(ExecutionUri))
            ?? new(DefaultExecutionUri);

        MaxResponseSize
            =  configuration.GetInt64(nameof(MaxResponseSize), min: 1)
            ?? DefaultMaxReceivedMessageSize;

        Timeout
            =  configuration.GetTimeSpan(nameof(Timeout), minTicks: 1)
            ?? FromTicks(DefaultTimeoutTicks);
    }
}
