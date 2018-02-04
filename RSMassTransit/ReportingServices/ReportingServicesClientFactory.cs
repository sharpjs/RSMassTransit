// Copyright (C) 2018 (to be determined)

using System.Net;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using RSMassTransit.ReportingServices.Execution;

namespace RSMassTransit.ReportingServices
{
    internal class ReportingServicesClientFactory : IReportingServicesClientFactory
    {
        public IReportExecutionSoapClient CreateExecutionClient(NetworkCredential credential = null)
            => CreateClient
                <ReportExecutionServiceSoapClient, ReportExecutionServiceSoap>
                (credential);

        private static TClient CreateClient<TClient, TChannel>(NetworkCredential credential = null)
            where TClient  : ClientBase<TChannel>, new()
            where TChannel : class
        {
            var client = null as TClient;
            try
            {
                client = new TClient();
                ProvideCredential(client.ClientCredentials, credential);
                return client;
            }
            catch
            {
                client?.Close();
                throw;
            }
        }

        private static void ProvideCredential(ClientCredentials client, NetworkCredential credential)
        {
            client.Windows.AllowedImpersonationLevel
                = TokenImpersonationLevel.Impersonation;

            client.Windows.ClientCredential
                = credential ?? CredentialCache.DefaultNetworkCredentials;
        }
    }
}
