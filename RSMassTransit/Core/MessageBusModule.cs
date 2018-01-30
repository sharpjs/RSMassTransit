using System;
using System.Configuration;
using Autofac;
using MassTransit;
using MassTransit.AzureServiceBusTransport;

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
                return CreateBusUsingRabbitMq(configuration);

            if (AzureServiceBusType.Equals(messageBusType, TypeComparison))
                return CreateBusUsingAzureServiceBus(configuration);

            throw new ConfigurationErrorsException(string.Format(
                "MessageBusType value '{0}' is not recognized.  " +
                "Valid MessageBusType values are: '{1}', '{2}'.",
                messageBusType,
                RabbitMqType,
                AzureServiceBusType
            ));
        }

        private IBusControl CreateBusUsingRabbitMq(IMessageBusConfiguration configuration)
        {
            return null;
        }

        private IBusControl CreateBusUsingAzureServiceBus(IMessageBusConfiguration configuration)
        {
            return Bus.Factory.CreateUsingAzureServiceBus(bus =>
            {
                var hostUri = new Uri("");
                //var serviceUri = ServiceBusEnvironment.CreateServiceUri(
                //    config.MessageBusScheme,
                //    config.MessageBusHostUri,
                //    config.MessageBusQueueName
                //);

                bus.Host(hostUri, host =>
                {
                    //host.TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(
                    //    config.MessageBusSecretName,
                    //    config.MessageBusSecret,
                    //    TimeSpan.FromDays(1),
                    //    TokenScope.Namespace
                    //);
                });

                bus.ReceiveEndpoint("rs", ep =>
                {
                });
            });
        }
    }
}
