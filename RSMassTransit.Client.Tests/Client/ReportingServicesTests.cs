// Copyright Subatomix Research Inc.
// SPDX-License-Identifier: MIT

using System.Net;
using RSMassTransit.Messages;
using Fake = RSMassTransit.Client.FakeReportingServices;

namespace RSMassTransit.Client;

[TestFixture]
public class ReportingServicesTests
{
    [Test]
    public void Create_NullConfiguration()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            ReportingServices.Create(null!);
        });
    }

    [Test]
    public void Create_RecognizedUri()
    {
        ReportingServices.Create(new ReportingServicesConfiguration
        {
            BusUri = Fake.Uri
        });
    }

    [Test]
    public void Create_UnrecognizedUri()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            ReportingServices.Create(new ReportingServicesConfiguration
            {
                BusUri = new Uri("unrecognized://example.com")
            });
        });
    }

    [Test]
    public void NormalizeBusUri_Ok()
    {
        WithInstance()
            .NormalizeBusUri(Fake.UriScheme, "fake bus")
            .ShouldBe(Fake.Uri);
    }

    [Test]
    public void NormalizeBusUri_Null()
    {
        Should.Throw<ConfigurationException>(() =>
        {
            WithInstance(c => c.BusUri = null)
                .NormalizeBusUri(Fake.UriScheme, "fake bus");
        });
    }

    [Test]
    public void NormalizeBusUri_Relative()
    {
        Should.Throw<ConfigurationException>(() =>
        {
            WithInstance(c => c.BusUri = new Uri("relative", UriKind.Relative))
                .NormalizeBusUri(Fake.UriScheme, "fake bus");
        });
    }

    [Test]
    public void NormalizeBusUri_Hostless()
    {
        Should.Throw<ConfigurationException>(() =>
        {
            WithInstance(c => c.BusUri = new Uri("file:///somewhere"))
                .NormalizeBusUri(Fake.UriScheme, "fake bus");
        });
    }

    [Test]
    public void NormalizeBusQueue_Ok()
    {
        WithInstance(c => c.BusQueue = "stuff")
            .NormalizeBusQueue()
            .ShouldBe("stuff");
    }

    [Test]
    public void NormalizeBusQueue_Null()
    {
        WithInstance(c => c.BusQueue = null)
            .NormalizeBusQueue()
            .ShouldBe(ReportingServicesConfiguration.DefaultBusQueue);
    }

    [Test]
    public void NormalizeBusQueue_Empty()
    {
        WithInstance(c => c.BusQueue = "")
            .NormalizeBusQueue()
            .ShouldBe(ReportingServicesConfiguration.DefaultBusQueue);
    }

    [Test]
    public void NormalizeBusCredential_Ok()
    {
        var credential = new NetworkCredential("username", "password");

        WithInstance(c => c.BusCredential = credential)
            .NormalizeBusCredential()
            .ShouldBeSameAs(credential);
    }

    [Test]
    public void NormalizeBusCredential_Null()
    {
        Should.Throw<ConfigurationException>(() =>
        {
            WithInstance(c => c.BusCredential = null)
                .NormalizeBusCredential();
        });
    }

    [Test]
    public void ExecuteReport()
    {
        IExecuteReportRequest  request  = new ExecuteReportRequest();
        IExecuteReportResponse response = new ExecuteReportResponse();

        var instance = WithInstance();
        instance.SetupRequest(request, response);

        var result = instance.ExecuteReport(request);

        result.ShouldBeSameAs(response);
        instance.Verify();
    }

    [Test]
    public async Task ExecuteReportAsync()
    {
        IExecuteReportRequest  request  = new ExecuteReportRequest();
        IExecuteReportResponse response = new ExecuteReportResponse();

        var instance = WithInstance();
        instance.SetupRequest(request, response);

        var result = await instance.ExecuteReportAsync(request);

        result.ShouldBeSameAs(response);
        instance.Verify();
    }

    private static Fake WithInstance(Action<ReportingServicesConfiguration>? setup = null)
    {
        var configuration = new ReportingServicesConfiguration
        {
            BusUri = Fake.Uri
        };

        setup?.Invoke(configuration);

        return new Fake(configuration);
    }
}
