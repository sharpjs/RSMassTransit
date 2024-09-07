// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

using System.Net;
using System.Security.Principal;
using RSMassTransit.ReportingServices.Execution;

namespace RSMassTransit.ReportingServices;

[TestFixture]
public class ReportingServicesClientFactoryTests
{
    private static ReportingServicesClientFactory
        Factory => ReportingServicesClientFactory.Instance;

    private static NetworkCredential
        DefaultCredential => CredentialCache.DefaultNetworkCredentials;

    private static readonly NetworkCredential
        ExplicitCredential = new("un", "pw");

    [Test]
    public void CreateExecutionClient_DefaultCredential()
    {
        var client = Factory.CreateExecutionClient();

        AssertClient<ReportExecutionServiceSoapClient>(client, DefaultCredential);
    }

    [Test]
    public void CreateExecutionClient_ExplicitCredential()
    {
        var client = Factory.CreateExecutionClient(ExplicitCredential);

        AssertClient<ReportExecutionServiceSoapClient>(client, ExplicitCredential);
    }

    private static void AssertClient<T>(
        IReportExecutionSoapClient client,
        NetworkCredential          credential)
    {
        var windowsCredential = client
            .Should().BeAssignableTo<ReportExecutionServiceSoapClient>()
            .Which.ClientCredentials.Windows;

        windowsCredential.AllowedImpersonationLevel
            .Should().Be(TokenImpersonationLevel.Impersonation);

        windowsCredential.ClientCredential
            .Should().BeSameAs(credential);
    }
}
