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

using System.Net;
using System.Security.Principal;
using FluentAssertions;
using NUnit.Framework;
using RSMassTransit.ReportingServices.Execution;

namespace RSMassTransit.ReportingServices
{
    [TestFixture]
    public class ReportingServicesClientFactoryTests
    {
        private static ReportingServicesClientFactory
            Factory => ReportingServicesClientFactory.Instance;

        private static NetworkCredential
            DefaultCredential => CredentialCache.DefaultNetworkCredentials;

        private static readonly NetworkCredential
            ExplicitCredential = new NetworkCredential("un", "pw");

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

        private void AssertClient<T>(
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
}
