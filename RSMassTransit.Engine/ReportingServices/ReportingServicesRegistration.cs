// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

using Microsoft.Extensions.DependencyInjection;

namespace RSMassTransit.ReportingServices;

internal static class ReportingServicesRegistration
{
    internal static void AddReportingServices(this IServiceCollection services)
    {
        if (services is null)
            throw new ArgumentNullException(nameof(services));

        services.AddSingleton(
            (IReportingServicesClientFactory)
            ReportingServicesClientFactory.Instance
        );
    }
}
