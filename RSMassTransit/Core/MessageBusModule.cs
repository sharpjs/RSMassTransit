using System;
using System.Configuration;
using Autofac;
using GreenPipes;
using MassTransit;
using MassTransit.AzureServiceBusTransport;
using Microsoft.ServiceBus;

namespace RSMassTransit.Core
{
    internal class MessageBusModule : Module
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
            var configuration  = context.Resolve<IMessageBusConfiguration>();
            var messageBusType = configuration.MessageBusType;

            if (RabbitMqType.Equals(messageBusType, TypeComparison))
                return CreateBusUsingRabbitMq(context, configuration);

            if (AzureServiceBusType.Equals(messageBusType, TypeComparison))
                return CreateBusUsingAzureServiceBus(context, configuration);

            throw new ConfigurationErrorsException(string.Format(
                "MessageBusType value '{0}' is not recognized.  " +
                "Valid MessageBusType values are: '{1}', '{2}'.",
                messageBusType,
                RabbitMqType,
                AzureServiceBusType
            ));
        }

        private IBusControl CreateBusUsingRabbitMq(
            IComponentContext        context,
            IMessageBusConfiguration configuration)
        {
            return Bus.Factory.CreateUsingRabbitMq(b =>
            {
                var hostUri = new UriBuilder("rabbitmq", configuration.MessageBusHost).Uri;

                var host = b.Host(hostUri, h =>
                {
                    h.Username(configuration.MessageBusSecretName);
                    h.Password(configuration.MessageBusSecret);
                });

                b.ReceiveEndpoint(host, configuration.MessageBusQueue, r =>
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
                    "sb", config.MessageBusHost, ""
                );

                var host = b.Host(uri, h =>
                {
                    h.SharedAccessSignature(s =>
                    {
                        s.KeyName         = config.MessageBusSecretName;
                        s.SharedAccessKey = config.MessageBusSecret;
                        s.TokenTimeToLive = TimeSpan.FromDays(1);
                        s.TokenScope      = TokenScope.Namespace;
                    });
                });

                b.ReceiveEndpoint(host, config.MessageBusQueue, r =>
                {
                    r.LoadFrom(context); // All registered consumers
                });

                b.UseRetry(r => r.None());
            });
        }
    }
}
