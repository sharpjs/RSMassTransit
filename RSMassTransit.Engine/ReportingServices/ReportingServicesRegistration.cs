// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

using Microsoft.Extensions.DependencyInjection;

namespace RSMassTransit.ReportingServices;

internal static class ReportingServicesRegistration
{
    internal static void AddReportingServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton(
            (IReportingServicesClientFactory)
            ReportingServicesClientFactory.Instance
        );
    }
}
