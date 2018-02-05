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
using System.Management.Automation;
using System.Net;
using MassTransit;
using MassTransit.AzureServiceBusTransport;
using Microsoft.ServiceBus;
using RSMassTransit.Messages;

namespace RSMassTransit.PowerShell
{
    public abstract class RSMassTransitCmdlet : PSCmdlet, IDisposable
    {
        public const string
            DefaultBusQueue       = "reports",
            RabbitMqScheme        = "rabbitmq",
            AzureServiceBusScheme = "sb";

        public const int
            DefaultTimeoutSeconds = 10;

        public static readonly Uri
            DefaultBusUri = new Uri("rabbitmq://localhost");

        public static readonly PSCredential
            DefaultBusCredential  = CreateGuestCredential();

        [Parameter]
        [ValidateNotNull]
        public Uri BusUri { get; set; } = DefaultBusUri;

        [Parameter]
        [ValidateNotNullOrEmpty]
        public string BusQueue { get; set; } = DefaultBusQueue;

        [Parameter]
        [Credential]
        public PSCredential BusCredential { get; set; } = DefaultBusCredential;

        [Parameter]
        [Credential]
        public PSCredential RsCredential { get; set; } = PSCredential.Empty;

        [Parameter]
        [ValidateRange(0, int.MaxValue)]
        public int TimeoutSeconds { get; set; } = DefaultTimeoutSeconds;

        protected IBusControl Bus { get; private set; }

        private bool _isDisposed;

        protected override void BeginProcessing()
        {
            CreateBus();
        }

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

        private void CreateBus()
        {
            switch (BusUri.Scheme.ToLower()) // in current culture
            {
                case RabbitMqScheme:
                    CreateBusUsingRabbitMq();
                    break;
                case AzureServiceBusScheme:
                    CreateBusUsingAzureServiceBus();
                    break;
                default:
                    throw new ValidationMetadataException(string.Format(
                        "The scheme '{0}' is invalid for the BusUri parameter.  " +
                        "Valid schemes are '{1}' and '{2}'.",
                        BusUri.Scheme, RabbitMqScheme, AzureServiceBusScheme
                    ));
            }

            WriteVerbose($"Starting message bus.");
            Bus.Start();
        }

        private void CreateBusUsingRabbitMq()
        {
            BusUri = new UriBuilder(
                RabbitMqScheme, BusUri.Host, BusUri.Port, BusUri.AbsolutePath
            ).Uri;

            WriteVerbose($"Using RabbitMQ: {BusUri}");

            var credential = BusCredential.GetNetworkCredential();

            Bus = MassTransit.Bus.Factory.CreateUsingRabbitMq(b =>
            {
                b.Host(BusUri, h =>
                {
                    h.Username(credential.UserName);
                    h.Password(credential.Password);
                });
            });
        }

        private void CreateBusUsingAzureServiceBus()
        {
            BusUri = ServiceBusEnvironment.CreateServiceUri(
                AzureServiceBusScheme, BusUri.Host, servicePath: ""
            );

            WriteVerbose($"Using Azure Service Bus: {BusUri}");

            var credential = BusCredential.GetNetworkCredential();

            Bus = MassTransit.Bus.Factory.CreateUsingAzureServiceBus(b =>
            {
                b.Host(BusUri, h =>
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

        protected void CreateBusClient<TRequest, TResponse>
            (out IRequestClient<TRequest, TResponse> client)
            where TRequest  : class
            where TResponse : class
        {
            client = Bus.CreateRequestClient<TRequest, TResponse>(
                new Uri(BusUri, BusQueue),
                TimeSpan.FromSeconds(TimeoutSeconds)
            );
        }

        private void DisposeBus()
        {
            Bus?.Stop();
            Bus = null;
        }

        private static PSCredential CreateGuestCredential()
        {
            return new PSCredential
            (
                userName: "guest",
                password: new NetworkCredential("", "guest").SecurePassword
            );
        }

        protected void ProvideRsCredential(ICredential message)
        {
            var credential = (RsCredential == PSCredential.Empty)
                ? null // use RSMassTransit service's windows credentials
                : RsCredential.GetNetworkCredential();

            message.UserName = credential?.UserName;
            message.Password = credential?.Password;
        }
    }
}
