/*
    Copyright (C) 2018 Jeffrey Sharp

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
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using MassTransit;
using Moq;
using NUnit.Framework;
using RSMassTransit.Messages;
using RSMassTransit.ReportingServices;
using RSMassTransit.ReportingServices.Execution;
using Sharp.BlobStorage;

namespace RSMassTransit.Consumers
{
    [TestFixture]
    [SetCulture(CultureName)]
    public class ExecuteReportConsumerTests
    {
        private MockRepository                             Mocks;
        private Mock<IReportingServicesClientFactory>      ClientFactory;
        private Mock<IReportExecutionSoapClient>           ExecutionClient;
        private Mock<IBlobStorage>                         Storage;
        private Mock<ConsumeContext<ExecuteReportRequest>> Context;
        private ExecuteReportConsumer                      Consumer;

        [SetUp]
        public void SetUp()
        {
            Mocks           = new MockRepository(MockBehavior.Strict);
            ClientFactory   = Mocks.Create<IReportingServicesClientFactory>();
            ExecutionClient = Mocks.Create<IReportExecutionSoapClient>();
            Storage         = Mocks.Create<IBlobStorage>();
            Context         = Mocks.Create<ConsumeContext<ExecuteReportRequest>>();
            Consumer        = new ExecuteReportConsumer(ClientFactory.Object, Storage.Object);
        }

        [TearDown]
        public void TearDown()
        {
            Mocks.Verify();
        }

        [Test]
        public void Construct_NullServices()
        {
            this.Invoking(_ => new ExecuteReportConsumer(null, Storage.Object))
                .Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Construct_NullStorage()
        {
            this.Invoking(_ => new ExecuteReportConsumer(ClientFactory.Object, null))
                .Should().Throw<ArgumentNullException>();
        }

        [Test]
        public async Task Consume()
        {
            var request = new ExecuteReportRequest
            {
                Path     = ReportPath,
                UserName = UserName,
                Password = Password,
                Format   = ReportFormat.Pdf,
            };

            Context
                .SetupGet(c => c.Message)
                .Returns(request)
                .Verifiable();

            ClientFactory
                .Setup(f => f.CreateExecutionClient(
                    It.Is<NetworkCredential>(c
                        => c.UserName == UserName
                        && c.Password == Password
                    )
                ))
                .Returns(ExecutionClient.Object)
                .Verifiable();

            var executionHeader = new ExecutionHeader();
            var loadResponse    = new LoadReport2Response() { ExecutionHeader = executionHeader };

            ExecutionClient
                .Setup(c => c.LoadReport2Async(
                    It.Is<LoadReport2Request>(r
                        => r.Report == ReportPath
                    )
                ))
                .ReturnsAsync(loadResponse)
                .Verifiable();

            var setParametersResponse = new SetExecutionParameters2Response();

            ExecutionClient
                .Setup(c => c.SetExecutionParameters2Async(
                    It.Is<SetExecutionParameters2Request>(r
                        => r.ExecutionHeader   == executionHeader
                        && r.Parameters.Length == 0
                        && r.ParameterLanguage == CultureName
                    )
                ))
                .ReturnsAsync(setParametersResponse)
                .Verifiable();

            var renderResponse = new Render2Response
            {
                MimeType  = PdfContentType,
                Extension = PdfExtension,
                Result    = TestContent,
                Warnings  = new Warning[0]
            };

            ExecutionClient
                .Setup(c => c.Render2Async(
                    It.Is<Render2Request>(r
                        => r.ExecutionHeader == executionHeader
                        && r.Format          == PdfReportFormat
                        && r.PaginationMode  == PageCountMode.Actual
                    )
                ))
                .ReturnsAsync(renderResponse)
                .Verifiable();

            ExecutionClient
                .Setup(c => c.Dispose())
                .Verifiable();

            Storage
                .Setup(s => s.PutAsync(
                    It.IsAny<Stream>(),
                    PdfExtension
                ))
                .ReturnsAsync(TestUri)
                .Verifiable();

            Context
                .Setup(c => c.RespondAsync(
                    It.Is<IExecuteReportResponse>(r
                        => r.Uri               == TestUri
                        && r.ContentType       == PdfContentType
                        && r.FileNameExtension == PdfExtension
                        && r.Length            == TestContent.Length
                        && r.Messages.Count    == 0
                    )
                ))
                .Returns(Task.CompletedTask)
                .Verifiable();

            await Consumer.Consume(Context.Object);
        }

        private const string
            CultureName     = "de-CH",
            ReportPath      = "/My Reports/Some Report",
            UserName        = "Tester",
            Password        = "Testing123",
            PdfReportFormat = "PDF",
            PdfContentType  = "application/pdf",
            PdfExtension    = ".pdf";

        private static readonly byte[]
            TestContent = { 0xAA, 0x55, 0xA5, 0x5A };

        private static readonly Uri
            TestUri = new Uri("blobs://test/some-report.pdf");
    }
}
