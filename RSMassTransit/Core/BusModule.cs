using System;
using System.Configuration;
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
        private const string
            RabbitMqType        = "RabbitMQ",
            AzureServiceBusType = "AzureServiceBus";

        private const StringComparison
            TypeComparison = StringComparison.OrdinalIgnoreCase;

        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterConsumers(typeof(ExecuteReportConsumer).Assembly);

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
            var config  = context.Resolve<IMessageBusConfiguration>();
            var busType = config.BusType;

            if (RabbitMqType.Equals(busType, TypeComparison))
                return CreateBusUsingRabbitMq(context, config);

            if (AzureServiceBusType.Equals(busType, TypeComparison))
                return CreateBusUsingAzureServiceBus(context, config);

            throw new ConfigurationErrorsException(string.Format(
                "MessageBusType value '{0}' is not recognized.  " +
                "Valid MessageBusType values are: '{1}', '{2}'.",
                busType,
                RabbitMqType,
                AzureServiceBusType
            ));
        }

        private IBusControl CreateBusUsingRabbitMq(
            IComponentContext        context,
            IMessageBusConfiguration config)
        {
            return Bus.Factory.CreateUsingRabbitMq(b =>
            {
                var hostUri = new UriBuilder("rabbitmq", config.BusHost).Uri;

                var host = b.Host(hostUri, h =>
                {
                    h.Username(config.BusSecretName);
                    h.Password(config.BusSecret);
                });

                b.ReceiveEndpoint(host, config.BusQueue, r =>
                {
                    r.Durable    = true;    // Queue should survive broker restart
                    r.AutoDelete = false;   // Queue should survive service restart
                    r.LoadFrom(context);    // All registered consumers
                });

                b.UseRetry(r => r.None());
            });
        }

        private IBusControl CreateBusUsingAzureServiceBus(
            IComponentContext        context,
            IMessageBusConfiguration config)
        {
            return Bus.Factory.CreateUsingAzureServiceBus(b =>
            {
                var uri = ServiceBusEnvironment.CreateServiceUri(
                    "sb", config.BusHost, ""
                );

                var host = b.Host(uri, h =>
                {
                    h.SharedAccessSignature(s =>
                    {
                        s.KeyName         = config.BusSecretName;
                        s.SharedAccessKey = config.BusSecret;
                        s.TokenTimeToLive = TimeSpan.FromDays(1);
                        s.TokenScope      = TokenScope.Namespace;
                    });
                });

                b.ReceiveEndpoint(host, config.BusQueue, r =>
                {
                    r.LoadFrom(context); // All registered consumers
                });

                b.UseRetry(r => r.None());
            });
        }
    }
}
