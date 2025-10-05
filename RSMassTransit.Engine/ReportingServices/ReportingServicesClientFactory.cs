// Copyright Subatomix Research Inc.
// SPDX-License-Identifier: MIT

using System.Net;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using RSMassTransit.ReportingServices.Execution;

namespace RSMassTransit.ReportingServices;

internal class ReportingServicesClientFactory : IReportingServicesClientFactory
{
    private readonly Binding         _binding;
    private readonly EndpointAddress _executionAddress;

    public ReportingServicesClientFactory(IReportingServicesClientConfiguration configuration)
    {
        _binding          = CreateBinding(configuration);
        _executionAddress = new(configuration.ExecutionUri);
    }

    public IReportExecutionSoapClient CreateExecutionClient(NetworkCredential? credential = null)
    {
        return CreateClient<
            ReportExecutionServiceSoapClient,
            ReportExecutionServiceSoap
        >(
            () => new ReportExecutionServiceSoapClient(_binding, _executionAddress),
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

    private static Binding CreateBinding(IReportingServicesClientConfiguration configuration)
    {
        var securityMode = configuration.ExecutionUri.Scheme == Uri.UriSchemeHttp
            ? BasicHttpSecurityMode.TransportCredentialOnly
            : BasicHttpSecurityMode.Transport;

        var binding = new BasicHttpBinding(securityMode)
        {
            Name                   = "ReportingServicesHttp",
            AllowCookies           = true,
            MaxReceivedMessageSize = configuration.MaxResponseSize,
            SendTimeout            = configuration.Timeout,
        };

        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;

        return binding;
    }

    private static void ProvideCredential(ClientCredentials client, NetworkCredential? credential)
    {
        client.Windows.AllowedImpersonationLevel
            = TokenImpersonationLevel.Delegation;

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
