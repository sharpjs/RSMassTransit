/*
    Copyright (C) 2018 Jeffrey Sharp

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

using Autofac;
using MassTransit;
using RSMassTransit.ReportingServices;
using RSMassTransit.Storage;
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
            InitializeObjects();
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
            builder.RegisterModule<BusModule>();
            builder.RegisterModule<ReportingServicesModule>();
            builder.RegisterModule<StorageModule>();
            _container = builder.Build();
        }

        private void InitializeObjects()
        {
            Log.Verbose("Initializing reporting services.");
            _container.Resolve<IReportingServicesClientFactory>();

            Log.Verbose("Initializing storage.");
            _container.Resolve<IBlobRepository>();
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
            _container.Resolve<IBusControl>().Start();
        }

        private void StopBus()
        {
            Log.Verbose("Stopping message bus, waiting on consumers to finish.");
            _container.Resolve<IBusControl>().Stop();
        }
    }
}
