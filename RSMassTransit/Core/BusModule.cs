/*
    Copyright (C) 2018 (to be determined)

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
using System.Configuration;
using System.Threading.Tasks;
using Autofac;
using GreenPipes;
using MassTransit;
using MassTransit.AzureServiceBusTransport;
using Microsoft.ServiceBus;
using RSMassTransit.Consumers;

namespace RSMassTransit.Core
{
    internal class BusModule : Module
    {
        public const string
            RabbitMqScheme        = "rabbitmq",
            AzureServiceBusScheme = "sb";

        private const StringComparison
            TypeComparison = StringComparison.OrdinalIgnoreCase;

        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterConsumers(typeof(ExecuteReportConsumer).Assembly)
                .Except<ErrorConsumer>();

            builder
                .Register(CreateBus)
                .SingleInstance()
                .As<IBusControl>()
                .As<IBus>();
                // Expose other interfaces as needed
        }

        private IBusControl CreateBus(IComponentContext context)
        {
            // This is called only once, so a fancier bus type registry is unwarranted.
            var configuration = context.Resolve<IBusConfiguration>();
            var scheme        = configuration.BusUri.Scheme;

            if (RabbitMqScheme.Equals(scheme, TypeComparison))
                return CreateBusUsingRabbitMq(context, configuration);

            if (AzureServiceBusScheme.Equals(scheme, TypeComparison))
                return CreateBusUsingAzureServiceBus(context, configuration);

            throw new ConfigurationErrorsException(string.Format(
                "The scheme '{0}' is invalid for the BusUri application setting.  " +
                "Valid schemes are '{1}' and '{2}'.",
                scheme, RabbitMqScheme, AzureServiceBusScheme
            ));
        }

        private IBusControl CreateBusUsingRabbitMq(
            IComponentContext context,
            IBusConfiguration configuration)
        {
            return Bus.Factory.CreateUsingRabbitMq(b =>
            {
                var uri = configuration.BusUri;
                    uri = new UriBuilder(RabbitMqScheme, uri.Host, uri.Port, uri.AbsolutePath).Uri;

                var host = b.Host(uri, h =>
                {
                    h.Username(configuration.BusSecretName);
                    h.Password(configuration.BusSecret);
                });

                b.ReceiveEndpoint(host, configuration.BusQueue, r =>
                {
                    r.Durable    = true;    // Queue should survive broker restart
                    r.AutoDelete = false;   // Queue should survive service restart
                    r.LoadFrom(context);    // All registered consumers
                });

                b.ReceiveEndpoint(host, configuration.BusQueue + "_error", r =>
                {
                    r.BindMessageExchanges = false; // binding not required to get messages into queue
                    r.PurgeOnStartup       = true;  // we discard them anyway
                    r.Consumer<ErrorConsumer>();
                });

                b.UseRetry(r => r.None());
            });
        }

        private IBusControl CreateBusUsingAzureServiceBus(
            IComponentContext context,
            IBusConfiguration configuration)
        {
            return Bus.Factory.CreateUsingAzureServiceBus(b =>
            {
                var uri = configuration.BusUri;
                    uri = ServiceBusEnvironment.CreateServiceUri("sb", uri.Host, "");

                var host = b.Host(uri, h =>
                {
                    h.SharedAccessSignature(s =>
                    {
                        s.KeyName         = configuration.BusSecretName;
                        s.SharedAccessKey = configuration.BusSecret;
                        s.TokenTimeToLive = TimeSpan.FromDays(1);
                        s.TokenScope      = TokenScope.Namespace;
                    });
                });

                b.ReceiveEndpoint(host, configuration.BusQueue, r =>
                {
                    r.LoadFrom(context); // All registered consumers
                });

                b.ReceiveEndpoint(host, configuration.BusQueue + "_error", r =>
                {
                    r.Consumer<ErrorConsumer>();
                });

                b.UseRetry(r => r.None());
            });
        }
    }
}
