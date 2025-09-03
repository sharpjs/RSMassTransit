// Copyright Subatomix Research Inc.
// SPDX-License-Identifier: MIT

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RSMassTransit.ReportingServices;

internal static class ReportingServicesRegistration
{
    internal static void AddReportingServices(
        this IServiceCollection services,
        IConfiguration          configuration)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton(LoadConfiguration(configuration));
        services.AddSingleton<IReportingServicesClientFactory, ReportingServicesClientFactory>();
    }

    private static IReportingServicesClientConfiguration
        LoadConfiguration(IConfiguration configuration)
    {
        return new ReportingServicesClientConfiguration(configuration);
    }
}
