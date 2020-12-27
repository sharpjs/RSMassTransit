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
using MassTransit;
using NUnit.Framework;

namespace RSMassTransit.Client.RabbitMQ
{
    [TestFixture]
    public class RabbitMqReportingServicesTests
    {
        [Test]
        public void Create_NullConfiguration()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new RabbitMqReportingServices(null);
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
}