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

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using GreenPipes;
using MassTransit;
using MassTransit.Context;
using Moq;
using NUnit.Framework;
using RSMassTransit.Messages;
using Fake = RSMassTransit.Client.FakeReportingServices;

namespace RSMassTransit.Client
{
    [TestFixture]
    public class ReportingServicesTests
    {
        [Test]
        public void Create_NullConfiguration()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                ReportingServices.Create(null);
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
                .Should().Be(Fake.Uri);
        }

        [Test]
        public void NormalizeBusUri_Null()
        {
            WithInstance(c => c.BusUri = null)
                .Invoking(s => s.NormalizeBusUri(Fake.UriScheme, "fake bus"))
                .Should().Throw<ConfigurationException>();
        }

        [Test]
        public void NormalizeBusUri_Relative()
        {
            WithInstance(c => c.BusUri = new Uri("relative", UriKind.Relative))
                .Invoking(s => s.NormalizeBusUri(Fake.UriScheme, "fake bus"))
                .Should().Throw<ConfigurationException>();
        }

        [Test]
        public void NormalizeBusUri_Hostless()
        {
            WithInstance(c => c.BusUri = new Uri("file:///somewhere"))
                .Invoking(s => s.NormalizeBusUri(Fake.UriScheme, "fake bus"))
                .Should().Throw<ConfigurationException>();
        }

        [Test]
        public void NormalizeBusQueue_Ok()
        {
            WithInstance(c => c.BusQueue = "stuff")
                .NormalizeBusQueue()
                .Should().Be("stuff");
        }

        [Test]
        public void NormalizeBusQueue_Null()
        {
            WithInstance(c => c.BusQueue = null)
                .NormalizeBusQueue()
                .Should().Be(ReportingServicesConfiguration.DefaultBusQueue);
        }

        [Test]
        public void NormalizeBusQueue_Empty()
        {
            WithInstance(c => c.BusQueue = "")
                .NormalizeBusQueue()
                .Should().Be(ReportingServicesConfiguration.DefaultBusQueue);
        }

        [Test]
        public void NormalizeBusCredential_Ok()
        {
            var credential = new NetworkCredential("username", "password");

            WithInstance(c => c.BusCredential = credential)
                .NormalizeBusCredential()
                .Should().BeSameAs(credential);
        }

        [Test]
        public void NormalizeBusCredential_Null()
        {
            WithInstance(c => c.BusCredential = null)
                .Invoking(s => s.NormalizeBusCredential())
                .Should().Throw<ConfigurationException>();
        }

        [Test]
        public void ExecuteReport()
        {
            IExecuteReportRequest  request  = new ExecuteReportRequest();
            IExecuteReportResponse response = new ExecuteReportResponse();

            var instance = WithInstance();
            instance.SetupRequest(request, response);

            var result = instance.ExecuteReport(request);

            result.Should().BeSameAs(response);
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

            result.Should().BeSameAs(response);
            instance.Verify();
        }

        private static Fake WithInstance
            (Action<ReportingServicesConfiguration> setup = null)
        {
            var configuration = new ReportingServicesConfiguration
            {
                BusUri = Fake.Uri
            };

            setup?.Invoke(configuration);

            return new Fake(configuration);
        }
    }
}
