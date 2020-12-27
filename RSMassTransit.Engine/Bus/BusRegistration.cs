/*
    Copyright 2020 Jeffrey Sharp

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
using System.Diagnostics.CodeAnalysis;
using GreenPipes;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using MassTransit.RabbitMqTransport;
using Microsoft.Azure.ServiceBus.Primitives;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RSMassTransit.Consumers;

namespace RSMassTransit.Bus
{
    [ExcludeFromCodeCoverage]
    internal static class BusRegistration
    {
        internal const string
            RabbitMqScheme        = "rabbitmq",
            AzureServiceBusScheme = "sb";

        private const StringComparison
            TypeComparison = StringComparison.OrdinalIgnoreCase;

        internal static void AddBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(LoadConfiguration(configuration));
            services.AddMassTransit(ConfigureServices);
            services.AddHostedService<BusService>();
        }

        private static IBusConfiguration LoadConfiguration(IConfiguration configuration)
        {
            return new BusConfiguration(configuration);
        }

        private static void ConfigureServices(IServiceCollectionBusConfigurator services)
        {
            services.AddConsumer<ExecuteReportConsumer>();
            services.AddBus(CreateBus);
        }

        private static IBusControl CreateBus(IBusRegistrationContext context)
        {
            // This is called only once, so a fancier bus type registry is unwarranted.
            var configuration = context.GetRequiredService<IBusConfiguration>();
            var scheme        = configuration.HostUri.Scheme;

            if (RabbitMqScheme.Equals(scheme, TypeComparison))
                return CreateBusUsingRabbitMq(context, configuration);

            if (AzureServiceBusScheme.Equals(scheme, TypeComparison))
                return CreateBusUsingAzureServiceBus(context, configuration);

            throw new FormatException(string.Format(
                "The scheme '{0}' is invalid for the BusUri application setting.  " +
                "Valid schemes are '{1}' and '{2}'.",
                scheme, RabbitMqScheme, AzureServiceBusScheme
            ));
        }

        private static IBusControl CreateBusUsingRabbitMq(
            IBusRegistrationContext context,
            IBusConfiguration       configuration)
        {
            return MassTransit.Bus.Factory.CreateUsingRabbitMq(b =>
            {
                var uri = configuration.HostUri;
                    uri = new UriBuilder(RabbitMqScheme, uri.Host, uri.Port, uri.AbsolutePath).Uri;

                b.Host(uri, h =>
                {
                    h.Username(configuration.SecretName);
                    h.Password(configuration.Secret);
                });

                b.ReceiveEndpoint(configuration.QueueName, r =>
                {
                    TuneForReportExecution(r);
                    r.ConfigureConsumers(context);
                });
            });
        }

        private static IBusControl CreateBusUsingAzureServiceBus(
            IBusRegistrationContext context,
            IBusConfiguration       configuration)
        {
            const string UriDomain = ".servicebus.windows.net";

            return MassTransit.Bus.Factory.CreateUsingAzureServiceBus(b =>
            {
                var uri = configuration.HostUri;
                    uri = new UriBuilder(AzureServiceBusScheme, uri.Host + UriDomain).Uri;

                b.Host(uri, h =>
                {
                    h.SharedAccessSignature(s =>
                    {
                        s.KeyName         = configuration.SecretName;
                        s.SharedAccessKey = configuration.Secret;
                        s.TokenTimeToLive = TimeSpan.FromDays(1);
                        s.TokenScope      = TokenScope.Namespace;
                    });
                });

                b.ReceiveEndpoint(configuration.QueueName, r =>
                {
                    TuneForReportExecution(r);
                    r.ConfigureConsumers(context);
                });
            });
        }

        // For RabbitMQ
        private static void TuneForReportExecution(IRabbitMqReceiveEndpointConfigurator r)
        {
            // Queue should survive RabbitMQ restart
            r.Durable = true;

            // Queue should survive RSMassTransit restart
            r.AutoDelete = false;

            // RSMassTransit expects multiple service instances, competing for
            // infrequent, long-running requests.  Prefetch optimizes for the
            // opposite case and actually *hinders* the spread of infrequent
            // messages across instances.  Therefor, turn prefetch off here.
            r.PrefetchCount = 0;

            // Do transport-independent tuning
            TuneForReportExecution((IReceiveEndpointConfigurator) r);
        }

        // For Azure Service Bus
        private static void TuneForReportExecution(IServiceBusReceiveEndpointConfigurator r)
        {
            // Report execution is single-threaded and typically has both idle
            // periods (waiting on query results) and CPU-bound periods
            // (rendering).  Thus SSRS *should* be able to support more
            // concurrent reports than the number of processors in the system.
            // However, while investigating tuning options and future load
            // compensation mechanisms, this will stay at a safer number.
            //
            // WARNING:
            //   Do not use int.MaxValue here, or the bus will never start!
            //   MassTransit will start this many async 'receive' operations
            //   against the queue.  Choose a number that balances the desire
            //   for max utilization against the danger of creating too many
            //   connections to Azure Service Bus.  Each connection adds delay
            //   to bus start and reduces the number of connections available
            //   to other clients.  The cap is 5000 ASB connections/receives.
            //
            // TODO: Consider physical memory in concurrency calculation.
            // TODO: Make loading factor(s) configurable.
            var concurrency = Math.Min(Environment.ProcessorCount, 32);
            r.MaxConcurrentCalls = concurrency;

            // RSMassTransit expects multiple service instances, competing for
            // infrequent, long-running requests.  Prefetch optimizes for the
            // opposite case and actually *hinders* the spread of infrequent
            // messages across instances.  Therefor, turn prefetch off here.
            //
            // WARNING: This must come *AFTER* MaxConcurrentCalls above.
            // Otherwise, the setting is overwritten.
            r.PrefetchCount = 0;

            // When RSMassTransit tries to pause or stop message consumption,
            // unwanted messages continue to be received, due to limitations in
            // the Azure Service Bus OnMessageAsync API.  The messages accrue
            // unconsumed up to the MaxConcurrentCalls limit.  To ensure those
            // messages quickly become consumable by a competing instance, use
            // a short message lock duration.
            r.LockDuration = TimeSpan.FromMinutes(1);

            // While a message is being consumed, its lock must be refreshed
            // regularly so that it does not expire.
            r.MaxAutoRenewDuration = TimeSpan.FromDays(1);

            // It is reasonable to assume that any client will have given up
            // waiting for their response after a day.
            r.DefaultMessageTimeToLive = TimeSpan.FromDays(1);

            // The short lock period, combined with several paused instances,
            // can cause many failed delivery attempts.  Use a high enough
            // maximum delivery count to avoid these actionable messages
            // getting dead-lettered.
            r.MaxDeliveryCount = concurrency * 4;

            // Do transport-independent tuning
            TuneForReportExecution((IReceiveEndpointConfigurator) r);
        }

        // Transport-independent tuning
        private static void TuneForReportExecution(IReceiveEndpointConfigurator r)
        {
            // No automatic retries.  Clients can implement their own.
            r.UseMessageRetry(x => x.None());

            // RSMassTransit's current users do not monitor an _error queue.
            // Prevent MT from inadvertently filling one to its limit.
            r.DiscardFaultedMessages();

            // RSMassTransit's current users do not monitor a _skipped queue.
            // Prevent MT from inadvertently filling one to its limit.
            r.DiscardSkippedMessages();
        }
    }
}
