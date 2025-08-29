// Copyright Subatomix Research Inc.
// SPDX-License-Identifier: MIT

namespace RSMassTransit.Client.AzureServiceBus;

[TestFixture]
public class AzureServiceBusReportingServicesTests
{
    [Test]
    public void Create_NullConfiguration()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            new AzureServiceBusReportingServices(null!);
        });
    }

    [Test]
    public void Create_UnrecognizedUri()
    {
        Assert.Throws<ConfigurationException>(() =>
        {
            new AzureServiceBusReportingServices(new ReportingServicesConfiguration
            {
                BusUri = new Uri("unrecognized://example.com")
            });
        });
    }
}
