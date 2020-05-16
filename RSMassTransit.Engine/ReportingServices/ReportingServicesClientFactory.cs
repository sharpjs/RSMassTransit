/*
    Copyright (C) 2020 Jeffrey Sharp

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

using System.Net;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using RSMassTransit.ReportingServices.Execution;

namespace RSMassTransit.ReportingServices
{
    internal class ReportingServicesClientFactory : IReportingServicesClientFactory
    {
        public static ReportingServicesClientFactory
            Instance = new ReportingServicesClientFactory();

        protected ReportingServicesClientFactory() { }

        public IReportExecutionSoapClient CreateExecutionClient(NetworkCredential? credential = null)
            => CreateClient
                <ReportExecutionServiceSoapClient, ReportExecutionServiceSoap>
                (credential);

        private static TClient CreateClient<TClient, TChannel>(NetworkCredential? credential = null)
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

        private static void ProvideCredential(ClientCredentials client, NetworkCredential? credential)
        {
            client.Windows.AllowedImpersonationLevel
                = TokenImpersonationLevel.Impersonation;

            client.Windows.ClientCredential
                = credential ?? CredentialCache.DefaultNetworkCredentials;
        }
    }
}
