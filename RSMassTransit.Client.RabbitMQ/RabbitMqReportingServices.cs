// Copyright Subatomix Research Inc.
// SPDX-License-Identifier: MIT

using System.Net;
using RSMassTransit.Bus;

namespace RSMassTransit.Client.RabbitMQ;

/// <summary>
///   Client that invokes actions on a RSMassTransit instance via a
///   RabbitMQ message bus.
/// </summary>
public class RabbitMqReportingServices : ReportingServices
{
    /// <summary>
    ///   The scheme component required in message bus URIs.
    /// </summary>
    public const string
        UriScheme = "rabbitmq";

    /// <summary>
    ///   Creates a new <see cref="RabbitMqReportingServices"/>
    ///   instance with the specified configuration.
    /// </summary>
    /// <param name="configuration">
    ///   The configuration for the client, specifying how to communicate
    ///   with RSMassTransit.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="configuration"/> is <see langword="null"/>.
    /// </exception>
    public RabbitMqReportingServices(ReportingServicesConfiguration configuration)
        : base(configuration) { }

    /// <inheritdoc/>
    protected override IBusControl CreateBus(out Uri queueUri)
    {
        var uri        = NormalizeBusUri(UriScheme, "RabbitMQ host");
        var queue      = NormalizeBusQueue();
        var credential = NormalizeBusCredential();

        var bus = MassTransit.Bus.Factory.CreateUsingRabbitMq(b =>
        {
            b.Host(uri, h =>
            {
                h.Username(credential.UserName);
                h.Password(credential.Password);
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

        return new UriBuilder(
            UriScheme, uri.Host, uri.Port, uri.AbsolutePath
        ).Uri;
    }

    /// <inheritdoc/>
    protected override NetworkCredential NormalizeBusCredential()
    {
        return Configuration.BusCredential
            ?? new NetworkCredential("guest", "guest");
    }
}
