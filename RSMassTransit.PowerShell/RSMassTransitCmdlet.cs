using System;
using System.Management.Automation;
using System.Net;
using MassTransit;
using MassTransit.AzureServiceBusTransport;
using Microsoft.ServiceBus;

namespace RSMassTransit.PowerShell
{
    public abstract class RSMassTransitCmdlet : PSCmdlet, IDisposable
    {
        public const BusType
            DefaultBusType = BusType.RabbitMQ;

        public const string
            DefaultBusHost  = "localhost",
            DefaultBusQueue = "reports";

        public const int
            DefaultTimeoutSeconds = 10;

        [Parameter]
        public BusType BusType { get; set; } = DefaultBusType;

        [Parameter]
        [ValidateNotNullOrEmpty]
        public string BusHost { get; set; } = DefaultBusHost;

        [Parameter]
        [ValidateNotNullOrEmpty]
        public string BusQueue { get; set; } = DefaultBusQueue;

        [Parameter]
        [Credential]
        public PSCredential BusCredential { get; set; } = PSCredential.Empty;

        [Parameter]
        [ValidateRange(0, int.MaxValue)]
        public int TimeoutSeconds { get; set; } = DefaultTimeoutSeconds;

        protected IBusControl Bus { get; private set; }

        private bool _isDisposed;

        ~RSMassTransitCmdlet()
        {
            Dispose(managed: false);
        }

        public void Dispose()
        {
            Dispose(managed: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool managed)
        {
            if (_isDisposed)
                return;

            if (managed)
                DisposeBus();

            _isDisposed = true;
        }

        protected void CreateBus()
        {
            switch (BusType)
            {
                case BusType.RabbitMQ:        CreateBusUsingRabbitMq();        break;
                case BusType.AzureServiceBus: CreateBusUsingAzureServiceBus(); break;
                default:
                    throw new ValidationMetadataException(string.Format(
                        "BusType '{0}' is invalid.  See cmdlet documentation for valid values.",
                        BusType
                    ));
            }
        }

        private void CreateBusUsingRabbitMq()
        {
            Bus = MassTransit.Bus.Factory.CreateUsingRabbitMq(b =>
            {
                var hostUri    = new UriBuilder("rabbitmq", BusHost).Uri;
                var credential = BusCredential.GetNetworkCredential();

                b.Host(hostUri, h =>
                {
                    h.Username(credential.UserName);
                    h.Password(credential.Password);
                });
            });
        }

        private void CreateBusUsingAzureServiceBus()
        {
            Bus = MassTransit.Bus.Factory.CreateUsingAzureServiceBus(b =>
            {
                var hostUri    = ServiceBusEnvironment.CreateServiceUri("sb", BusHost, "");
                var credential = BusCredential.GetNetworkCredential();

                b.Host(hostUri, h =>
                {
                    h.SharedAccessSignature(s =>
                    {
                        s.KeyName         = credential.UserName;
                        s.SharedAccessKey = credential.Password;
                        s.TokenTimeToLive = TimeSpan.FromDays(1);
                        s.TokenScope      = TokenScope.Namespace;
                    });
                });
            });
        }

        protected void CreateRequestClient<TRequest, TResponse>
            (out IRequestClient<TRequest, TResponse> client)
            where TRequest  : class
            where TResponse : class
        {
            client = Bus.CreateRequestClient<TRequest, TResponse>(
                new Uri(""),
                TimeSpan.FromSeconds(TimeoutSeconds)
            );
        }

        protected void DisposeBus()
        {
            Bus?.Stop();
            Bus = null;
        }
    }
}
