// Copyright Subatomix Research Inc.
// SPDX-License-Identifier: MIT

using System.Net;
using System.Security.Principal;
using RSMassTransit.ReportingServices.Execution;

namespace RSMassTransit.ReportingServices;

[TestFixture]
public class ReportingServicesClientFactoryTests
{
    private static NetworkCredential
        DefaultCredential => CredentialCache.DefaultNetworkCredentials;

    private static readonly NetworkCredential
        ExplicitCredential = new("un", "pw");

    private ReportingServicesClientFactory
        Factory { get; }

    public ReportingServicesClientFactoryTests()
    {
        Factory = new(Mock.Of<IReportingServicesClientConfiguration>(c
            => c.ExecutionUri    == new Uri("http://localhost/ReportServer/ReportExecution2005.asmx")
            && c.MaxResponseSize == 2L * 1024 * 1024 * 1024
            && c.Timeout         == TimeSpan.FromHours(1)
        ));
    }

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
