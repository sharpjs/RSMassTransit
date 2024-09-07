// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

using System.Net;

namespace RSMassTransit.Client;

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

    public Dictionary<Type, Mock> RequestClients { get; } = new();

    protected override IBusControl CreateBus(out Uri queueUri)
    {
        Bus.Setup(b => b.StartAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Mock.Of<BusHandle>());

        queueUri = new Uri(Uri, ReportingServicesConfiguration.DefaultBusQueue);
        return Bus.Object;
    }

    protected override IRequestClient<TRequest>
        CreateRequestClient<TRequest>(TimeSpan? timeout = null)
        where TRequest  : class
    {
        return GetRequestClient<TRequest>().Object;
    }

    private Mock<IRequestClient<TRequest>> GetRequestClient<TRequest>()
        where TRequest  : class
    {
        var key = typeof(TRequest);

        if (!RequestClients.TryGetValue(key, out Mock? mock))
            mock = RequestClients[key] = new Mock<IRequestClient<TRequest>>(MockBehavior.Strict);

        return (Mock<IRequestClient<TRequest>>) mock;
    }

    public void SetupRequest<TRequest, TResponse>(TRequest request, TResponse response)
        where TRequest  : class
        where TResponse : class
    {
        var responseWrapper = new Mock<Response<TResponse>>(MockBehavior.Strict);

        responseWrapper.Setup(r => r.Message).Returns(response);

        GetRequestClient<TRequest>()
            .Setup(c => c.GetResponse<TResponse>(request, It.IsAny<CancellationToken>(), It.IsAny<RequestTimeout>()))
            .ReturnsAsync(responseWrapper.Object)
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
