using System;
using System.Net;
using System.Security.Principal;

namespace RSMassTransit
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var client = new ReportingServices.Execution.ReportExecutionServiceSoapClient())
            {
                var credential                       = client.ClientCredentials.Windows;
                credential.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
                credential.ClientCredential          = CredentialCache.DefaultNetworkCredentials;

                client.ListSecureMethods(
                    null,
                    out string[] methods
                );
            }
        }
    }
}
