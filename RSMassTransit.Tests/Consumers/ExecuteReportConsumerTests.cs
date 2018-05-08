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
using System.Linq;
using System.Linq.Expressions;
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
    [SetCulture(TestCultureName)]
    public class ExecuteReportConsumerTests
    {
        // Object under test
        private ExecuteReportConsumer Consumer;

        // Mocks
        private MockRepository                             Mocks;
        private Mock<IReportingServicesClientFactory>      ClientFactory;
        private Mock<IReportExecutionSoapClient>           ExecutionClient;
        private Mock<IBlobStorage>                         Storage;
        private Mock<ConsumeContext<ExecuteReportRequest>> Context;

        // Data
        private ExecutionHeader ExecutionHeader;

        [SetUp]
        public void SetUp()
        {
            Mocks           = new MockRepository(MockBehavior.Strict);
            ClientFactory   = Mocks.Create<IReportingServicesClientFactory>();
            ExecutionClient = Mocks.Create<IReportExecutionSoapClient>();
            Storage         = Mocks.Create<IBlobStorage>();
            Context         = Mocks.Create<ConsumeContext<ExecuteReportRequest>>();

            Consumer        = new ExecuteReportConsumer(ClientFactory.Object, Storage.Object);

            ExecutionHeader = new ExecutionHeader();
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
        public async Task Consume_NoParameters()
        {
            SetUpRequest();
            SetUpCreateExecutionClient();
            SetUpLoadReport();
            SetUpSetParameters();
            SetUpRender();
            SetUpStore();
            ExpectResponse();

            await Consumer.Consume(Context.Object);
        }

        private void SetUpRequest(Action<ExecuteReportRequest> setup = null)
        {
            var request = new ExecuteReportRequest
            {
                Path     = TestReportPath,
                UserName = TestUserName,
                Password = TestPassword,
                Format   = TestFormat
            };

            setup?.Invoke(request);

            Context
                .SetupGet(c => c.Message)
                .Returns(request)
                .Verifiable();
        }

        private void SetUpCreateExecutionClient(
            Expression<Func<NetworkCredential, bool>> predicate = null)
        {
            if (predicate == null)
                predicate = c
                    => c.UserName == TestUserName
                    && c.Password == TestPassword;

            ClientFactory
                .Setup(f => f.CreateExecutionClient(It.Is(predicate)))
                .Returns(ExecutionClient.Object)
                .Verifiable();

            ExecutionClient
                .Setup(c => c.Dispose())
                .Verifiable();
        }

        private void SetUpLoadReport(
            Expression<Func<LoadReport2Request, bool>> predicate = null)
        {
            if (predicate == null)
                predicate = r => r.Report == TestReportPath;

            var response = new LoadReport2Response { ExecutionHeader = ExecutionHeader };

            ExecutionClient
                .Setup(c => c.LoadReport2Async(It.Is(predicate)))
                .ReturnsAsync(response)
                .Verifiable();
        }

        private void SetUpSetParameters(
            Expression<Func<SetExecutionParameters2Request, bool>> predicate = null)
        {
            if (predicate == null)
                predicate = r
                    => r.ExecutionHeader   == ExecutionHeader
                    && r.Parameters.Length == 0
                    && r.ParameterLanguage == TestCultureName;

            var response = new SetExecutionParameters2Response();

            ExecutionClient
                .Setup(c => c.SetExecutionParameters2Async(It.Is(predicate)))
                .ReturnsAsync(response)
                .Verifiable();
        }

        private void SetUpRender(
            Action<Render2Response> setup = null)
        {
            var response = new Render2Response
            {
                MimeType  = TestContentType,
                Extension = TestExtension,
                Result    = TestContent,
                Warnings  = new Warning[0]
            };

            setup?.Invoke(response);

            ExecutionClient
                .Setup(c => c.Render2Async(
                    It.Is<Render2Request>(r
                        => r.ExecutionHeader == ExecutionHeader
                        && r.Format          == TestFormatName
                        && r.PaginationMode  == PageCountMode.Actual
                    )
                ))
                .ReturnsAsync(response)
                .Verifiable();
        }

        private void SetUpStore(
            byte[] bytes     = null,
            string extension = TestExtension)
        {
            if (bytes == null)
                bytes = TestContent;

            Storage
                .Setup(s => s.PutAsync(
                    It.Is<Stream>(b => StreamYields(b, bytes)),
                    TestExtension
                ))
                .ReturnsAsync(TestUri)
                .Verifiable();
        }

        private void ExpectResponse(
            Expression<Func<IExecuteReportResponse, bool>> predicate = null)
        {
            if (predicate == null)
                predicate = r
                    => r.Uri               == TestUri
                    && r.ContentType       == TestContentType
                    && r.FileNameExtension == TestExtension
                    && r.Length            == TestContent.Length
                    && r.Messages.Count    == 0;
            
            Context
                .Setup(c => c.RespondAsync(It.Is(predicate)))
                .Returns(Task.CompletedTask)
                .Verifiable();
        }

        private static bool StreamYields(Stream s, byte[] bytes)
        {
            byte[] actual;

            using (var memory = new MemoryStream())
            {
                s.CopyTo(memory);
                actual = memory.ToArray();
            }

            return actual?.SequenceEqual(bytes) ?? false;
        }

        private const string
            TestCultureName = "de-CH",
            TestReportPath  = "/My Reports/Some Report",
            TestUserName    = "Tester",
            TestPassword    = "Testing123",
            TestFormatName  = "PDF",
            TestContentType = "application/pdf",
            TestExtension   = ".pdf";

        private const ReportFormat
            TestFormat      = ReportFormat.Pdf;

        private static readonly byte[]
            TestContent     = { 0xAA, 0x55, 0xA5, 0x5A };

        private static readonly Uri
            TestUri         = new Uri("blobs://test/some-report.pdf");
    }
}
