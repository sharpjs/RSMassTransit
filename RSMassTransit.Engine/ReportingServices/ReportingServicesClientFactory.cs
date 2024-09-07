// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

using System.Net;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using RSMassTransit.ReportingServices.Execution;

namespace RSMassTransit.ReportingServices;

internal class ReportingServicesClientFactory : IReportingServicesClientFactory
{
    public static ReportingServicesClientFactory
        Instance = new();

    private static readonly Binding
        Binding = CreateBinding();

    private static readonly EndpointAddress
        ExecutionAddress = new("http://localhost:80/ReportServer/ReportExecution2005.asmx");

    protected ReportingServicesClientFactory() { }

    public IReportExecutionSoapClient CreateExecutionClient(NetworkCredential? credential = null)
    {
        return CreateClient<
            ReportExecutionServiceSoapClient,
            ReportExecutionServiceSoap
        >(
            () => new ReportExecutionServiceSoapClient(Binding, ExecutionAddress),
            credential
        );
    }

    private static TClient CreateClient<TClient, TChannel>(
        Func<TClient>      constructor,
        NetworkCredential? credential = null)
        where TClient  : ClientBase<TChannel>
        where TChannel : class
    {
        var client = null as TClient;
        try
        {
            client = constructor();
            ProvideCredential(client.ClientCredentials, credential);
            return client;
        }
        catch
        {
            client?.Close();
            throw;
        }
    }

    private static Binding CreateBinding()
    {
        var binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly)
        {
            // TODO: Make most of these configurable.
            Name                   = "ReportingServicesHttp",
            MaxReceivedMessageSize = 1024 * 1024 * 1024, //   1 GiB
            AllowCookies           = true,
            SendTimeout            = new TimeSpan(hours: 4, minutes:  0, seconds: 30), // just a bit longer than the report timeout
            ReceiveTimeout         = new TimeSpan(hours: 0, minutes: 30, seconds:  0),
        };

        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;

        return binding;
    }

    private static void ProvideCredential(ClientCredentials client, NetworkCredential? credential)
    {
        client.Windows.AllowedImpersonationLevel
            = TokenImpersonationLevel.Impersonation;

        client.Windows.ClientCredential
            = credential ?? CredentialCache.DefaultNetworkCredentials;
    }

    /*
        <!-- Previous XML configuration -->
        <system.serviceModel>
          <bindings>
            <basicHttpBinding>
              <binding name="ReportingServicesHttp" maxReceivedMessageSize="268435456" sendTimeout="04:00:30">
                <!-- 268435456 = 256 MB maximum output size -->
                <security mode="TransportCredentialOnly">
                  <transport clientCredentialType="Ntlm" />
                  <message algorithmSuite="Default" />
                </security>
              </binding>
            </basicHttpBinding>
          </bindings>
          <client>
            <endpoint
              address="http://localhost:80/ReportServer/ReportExecution2005.asmx"
              binding="basicHttpBinding" bindingConfiguration="ReportingServicesHttp"
              contract="RSMassTransit.ReportingServices.Execution.ReportExecutionServiceSoap"
              name="ReportExecutionServiceSoap" />
          </client>
        </system.serviceModel>
    */
}
