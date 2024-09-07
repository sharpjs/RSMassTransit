// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

using RSMassTransit.Bus;

namespace RSMassTransit.Client.AzureServiceBus;

/// <summary>
///   Client that invokes actions on a RSMassTransit instance via messages
///   in an Azure Service Bus namespace.
/// </summary>
public class AzureServiceBusReportingServices : ReportingServices
{
    /// <summary>
    ///   The scheme component required in message bus URIs.
    /// </summary>
    public const string
        UriScheme = "sb";

    private const string
        HostSuffix = ".servicebus.windows.net";

    /// <summary>
    ///   Creates a new <see cref="AzureServiceBusReportingServices"/>
    ///   instance with the specified configuration.
    /// </summary>
    /// <param name="configuration">
    ///   The configuration for the client, specifying how to communicate
    ///   with RSMassTransit.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="configuration"/> is <see langword="null"/>.
    /// </exception>
    public AzureServiceBusReportingServices(ReportingServicesConfiguration configuration)
        : base(configuration) { }

    /// <inheritdoc/>
    protected override IBusControl CreateBus(out Uri queueUri)
    {
        var uri        = NormalizeBusUri(UriScheme, "Azure Service Bus namespace");
        var queue      = NormalizeBusQueue();
        var credential = NormalizeBusCredential();

        var bus = MassTransit.Bus.Factory.CreateUsingAzureServiceBus(b =>
        {
            b.Host(uri, h =>
            {
                h.NamedKeyCredential = new(
                    credential.UserName,
                    credential.Password
                );
            });

            b.DiscardFaultAndSkippedMessages();
        });

        queueUri = new Uri(uri, queue);
        return bus;
    }

    /// <inheritdoc/>
    protected override Uri NormalizeBusUri(string scheme, string kind)
    {
        var uri = base.NormalizeBusUri(scheme, kind);

        var host = uri.Host;
        if (!host.EndsWith(HostSuffix, StringComparison.OrdinalIgnoreCase))
            host += HostSuffix;

        return new UriBuilder(UriScheme, host).Uri;
    }
}
