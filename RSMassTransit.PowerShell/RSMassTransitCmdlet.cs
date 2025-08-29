// Copyright Subatomix Research Inc.
// SPDX-License-Identifier: MIT

using System.Management.Automation;
using RSMassTransit.Client;
using RSMassTransit.Messages;

namespace RSMassTransit.PowerShell;

public abstract class RSMassTransitCmdlet : PSCmdlet, IDisposable
{
    protected static readonly Uri
        DefaultBusUri = new("rabbitmq://localhost");

    protected const string
        DefaultBusQueue = "reports";

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
    public PSCredential BusCredential { get; set; } = PSCredential.Empty;

    [Parameter]
    [Credential]
    public PSCredential RsCredential { get; set; } = PSCredential.Empty;

    [Parameter]
    [ValidateRange(0, int.MaxValue)]
    public int TimeoutSeconds { get; set; } = DefaultTimeoutSeconds;

    protected IReportingServices? Client { get; private set; }

    protected override void BeginProcessing()
    {
        Client = ReportingServices.Create(new ReportingServicesConfiguration
        {
            BusUri         = BusUri,
            BusQueue       = BusQueue,
            BusCredential  = BusCredential.GetNetworkCredentialSafe(),
            RequestTimeout = TimeSpan.FromSeconds(TimeoutSeconds)
        });
    }

    protected void ProvideRsCredential(ICredential message)
    {
        var credential   = RsCredential.GetNetworkCredentialSafe();
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
