/*
    Copyright (C) 2020 Jeffrey Sharp

    Permission to use, copy, modify, and distribute this software for any
    purpose with or without fee is hereby granted, provided that the above
    copyright notice and this permission notice appear in all copies.

    THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
    WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
    MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
    ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
    WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
    ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
    OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
*/

using System;
using System.Net;
using MassTransit;

namespace RSMassTransit.Client.RabbitMQ
{
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
        public RabbitMqReportingServices(ReportingServicesConfiguration configuration)
            : base(configuration) { }

        /// <inheritdoc/>
        protected override IBusControl CreateBus(out Uri queueUri)
        {
            var uri        = NormalizeBusUri(UriScheme, "RabbitMQ host");
            var queue      = NormalizeBusQueue();
            var credential = NormalizeBusCredential();

            var bus = Bus.Factory.CreateUsingRabbitMq(b =>
            {
                b.Host(uri, h =>
                {
                    h.Username(credential.UserName);
                    h.Password(credential.Password);
                });
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
}
