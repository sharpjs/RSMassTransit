// Copyright Subatomix Research Inc.
// SPDX-License-Identifier: MIT

namespace RSMassTransit.Client.RabbitMQ;

[TestFixture]
public class RabbitMqReportingServicesTests
{
    [Test]
    public void Create_NullConfiguration()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            new RabbitMqReportingServices(null!);
        });
    }

    [Test]
    public void Create_UnrecognizedUri()
    {
        Assert.Throws<ConfigurationException>(() =>
        {
            new RabbitMqReportingServices(new ReportingServicesConfiguration
            {
                BusUri = new Uri("unrecognized://example.com")
            });
        });
    }
}
