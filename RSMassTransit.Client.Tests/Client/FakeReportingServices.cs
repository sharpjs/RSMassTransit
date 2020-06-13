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
using System.Collections.Generic;
using System.Net;
using System.Threading;
using MassTransit;
using Moq;

#pragma warning disable CS0618 // Type or member is obsolete

namespace RSMassTransit.Client
{
    public class FakeReportingServices : ReportingServices
    {
        public const string
            UriScheme = "fake";

        public static readonly Uri
            Uri = new Uri("fake://example.com");

        public FakeReportingServices(ReportingServicesConfiguration configuration)
            : base(configuration) { }

        public Mock<IBusControl> Bus { get; }
            = new Mock<IBusControl>(MockBehavior.Strict);

        public Dictionary<(Type, Type), Mock> RequestClients { get; }
            = new Dictionary<(Type, Type), Mock>();

        protected override IBusControl CreateBus(out Uri queueUri)
        {
            Bus.Setup(b => b.StartAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<BusHandle>());

            queueUri = new Uri(Uri, ReportingServicesConfiguration.DefaultBusQueue);
            return Bus.Object;
        }

        protected override IRequestClient<TRequest, TResponse>
            CreateRequestClient<TRequest, TResponse>(TimeSpan? timeout = null)
        {
            return GetRequestClient<TRequest, TResponse>().Object;
        }

        private Mock<IRequestClient<TRequest, TResponse>> GetRequestClient<TRequest, TResponse>()
            where TRequest  : class
            where TResponse : class
        {
            var key = (typeof(TRequest), typeof(TResponse));

            if (!RequestClients.TryGetValue(key, out Mock mock))
                mock = RequestClients[key] = new Mock<IRequestClient<TRequest, TResponse>>(MockBehavior.Strict);

            return (Mock<IRequestClient<TRequest, TResponse>>) mock;
        }

        public void SetupRequest<TRequest, TResponse>(TRequest request, TResponse response)
            where TRequest  : class
            where TResponse : class
        {
            GetRequestClient<TRequest, TResponse>()
                .Setup(c => c.Request(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(response)
                .Verifiable();
        }

        public void Verify()
        {
            Bus.Verify();

            foreach (var mock in RequestClients.Values)
                mock.Verify();
        }

        public new Uri NormalizeBusUri(string scheme, string kind)
            => base.NormalizeBusUri(scheme, kind);

        public new string NormalizeBusQueue()
            => base.NormalizeBusQueue();

        public new NetworkCredential NormalizeBusCredential()
            => base.NormalizeBusCredential();
    }
}
