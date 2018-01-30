using Autofac;
using MassTransit;
using Sharp.ServiceHost;

namespace RSMassTransit.Core
{
    internal class RSMassTransitService : Service
    {
        public const string
            Name        = "RSMassTransit",
            DisplayName = "SQL Server Reporting Services MassTransit Interface",
            Description = "Executes reports in response to messages received on a MassTransit message bus.";

        private IContainer _container;

        public RSMassTransitService()
            : base(Name) { }

        protected override void StartCore()
        {
            CreateContainer();
            StartBus();
        }

        protected override void StopCore()
        {
            StopBus();
            DisposeContainer();
        }

        protected override void Dispose(bool managed)
        {
            if (managed)
                DisposeContainer();

            base.Dispose(managed);
        }

        private void CreateContainer()
        {
            Log.Verbose("Creating IoC container.");
            var builder = new ContainerBuilder();
            builder.RegisterInstance(Configuration.Current).AsImplementedInterfaces();
            //builder.RegisterModule<MessageBusModule>();
            _container = builder.Build();
        }

        private void DisposeContainer()
        {
            Log.Verbose("Disposing IoC container.");
            _container?.Dispose();
            _container = null;
        }

        private void StartBus()
        {
            Log.Verbose("Starting message bus.");
            //_container.Resolve<IBusControl>().Start();
        }

        private void StopBus()
        {
            Log.Verbose("Stopping message bus.");
            //_container.Resolve<IBusControl>().Stop();
        }
    }
}
