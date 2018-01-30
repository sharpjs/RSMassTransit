using Sharp.ServiceHost;

namespace RSMassTransit.Core
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ServiceHost<RSMassTransitService>.Run(args);
            /*
            using (var client = new ReportExecutionServiceSoapClient())
            {
                var credential                       = client.ClientCredentials.Windows;
                credential.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
                credential.ClientCredential          = CredentialCache.DefaultNetworkCredentials;

                var loadResponse = client.LoadReport2(new LoadReport2Request
                {
                    Report = "/My Reports/AP Reports/350-APReport",
                });

                var execution = loadResponse.ExecutionHeader;

                client.SetExecutionParameters2(new SetExecutionParameters2Request
                {
                    ExecutionHeader = execution,
                    Parameters = new[]
                    {
                        new ParameterValue { Name = "IsVendor",         Value = "True" },
                        new ParameterValue { Name = "CompanySelection", Value = "1"    },
                    },
                    ParameterLanguage = "en-US"
                });

                var renderResponse = client.Render2(new Render2Request
                {
                    ExecutionHeader = execution,
                    Format          = "ExcelOpenXML",
                    PaginationMode  = PageCountMode.Estimate
                });

                var bytes = renderResponse.Result;
            }
            */
        }
    }
}
