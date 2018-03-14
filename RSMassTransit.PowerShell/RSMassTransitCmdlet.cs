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

using System;
using System.Management.Automation;
using System.Net;
using MassTransit;
using MassTransit.AzureServiceBusTransport;
using Microsoft.ServiceBus;
using RSMassTransit.Client;
using RSMassTransit.Messages;

namespace RSMassTransit.PowerShell
{
    public abstract class RSMassTransitCmdlet : PSCmdlet, IDisposable
    {
        protected static readonly Uri
            DefaultBusUri = new Uri("rabbitmq://localhost");

        protected const string
            DefaultBusQueue = "reports";

        protected static readonly PSCredential
            DefaultBusCredential  = CreateGuestCredential();

        protected const int
            DefaultTimeoutSeconds = 30;

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

        protected IReportingServices Client { get; private set; }

        protected override void BeginProcessing()
        {
            Client = ReportingServices.Create(new ReportingServicesConfiguration
            {
                BusUri         = BusUri,
                BusQueue       = BusQueue,
                BusCredential  = BusCredential.GetNetworkCredential(),
                RequestTimeout = TimeSpan.FromSeconds(TimeoutSeconds)
            });
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

        protected T WithFaultHandling<T>(Func<T> action)
        {
            try
            {
                return action();
            }
            catch (RequestFaultException e)
            {
                foreach (var x in e.Fault.Exceptions)
                    WriteWarning($"{x.ExceptionType}: {x.Message}\r\n{x.StackTrace}");

                throw;
            }
        }

        public void Dispose()
        {
            Dispose(managed: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool managed)
        {
            Client?.Dispose();
            Client = null;
        }
    }
}
