using System;
using Autofac;
using MassTransit;
using MassTransit.AzureServiceBusTransport;

namespace RSMassTransit.Core
{
    internal class MessageBusModule : Module
    {
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
            return CreateBusUsingAzureServiceBus(context);
        }

        private IBusControl CreateBusUsingAzureServiceBus(IComponentContext context)
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
