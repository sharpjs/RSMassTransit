/*
    Copyright 2020 Jeffrey Sharp

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
using System.Net;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using RSMassTransit.ReportingServices.Execution;

namespace RSMassTransit.ReportingServices
{
    internal class ReportingServicesClientFactory : IReportingServicesClientFactory
    {
        public static ReportingServicesClientFactory
            Instance = new ReportingServicesClientFactory();

        private static readonly Binding
            Binding = CreateBinding();

        private static readonly EndpointAddress
            ExecutionAddress = new EndpointAddress("http://localhost:80/ReportServer/ReportExecution2005.asmx");

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
}
