// Copyright Subatomix Research Inc.
// SPDX-License-Identifier: MIT

using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RSMassTransit.Bus;
using RSMassTransit.ReportingServices;
using RSMassTransit.Storage;

namespace RSMassTransit;

/// <summary>
///   Extension methods for <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///   Registers a complete set of RSMassTransit services with the
    ///   specified configuration.
    /// </summary>
    /// <param name="services">
    ///   The services collection in which to register services.
    /// </param>
    /// <param name="configuration">
    ///   The configuration to use.
    /// </param>
    /// <returns>
    ///   The services collection <paramref name="services"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="services"/> and/or
    ///   <paramref name="configuration"/> are <c>null</c>.
    /// </exception>
    /// <exception cref="ConfigurationException">
    ///   One or more values in <paramref name="configuration"/> is
    ///   invalid.
    /// </exception>
    public static IServiceCollection AddRSMassTransit(
        this IServiceCollection services,
        IConfiguration          configuration)
    {
        ServicePointManager.SetTcpKeepAlive(
            enabled: true,
            /* send keep-alive packet after */ 15_000 /* ms of inactivity  */,
            /* send another KA packet after */  2_000 /* ms if no response */
        );

        services.AddBus               (configuration.GetSection("Bus"      ));
        services.AddBlobStorage       (configuration.GetSection("Storage"  ));
        services.AddReportingServices (configuration.GetSection("Reporting"));
        return services;
    }
}
