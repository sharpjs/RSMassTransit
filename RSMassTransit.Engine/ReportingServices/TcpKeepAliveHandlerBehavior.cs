// Copyright Subatomix Research Inc.
// SPDX-License-Identifier: MIT

using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using System.Reflection;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace RSMassTransit.ReportingServices;

using B = BindingFlags;
using L = SocketOptionLevel;
using N = SocketOptionName;

internal sealed class TcpKeepAliveHandlerBehavior : IEndpointBehavior
{
    // Used this code as an example endpoint behavior:
    // https://github.com/dotnet/wcf/blob/v8.1.2-rtm/src/System.Private.ServiceModel/tests/Scenarios/Binding/Http/HttpBindingTestHelpers.cs

    // Accesses a private member of HttpClientHandler:
    // https://github.com/dotnet/runtime/blob/v8.0.21/src/libraries/System.Net.Http/src/System/Net/Http/HttpClientHandler.cs#L56

    public static TcpKeepAliveHandlerBehavior Instance { get; } = new();

    private static readonly Func<HttpClientHandler, SocketsHttpHandler>
        InnerHandlerAccessor = CreateInnerHandlerAccessor();

    [ExcludeFromCodeCoverage]
    private static Func<HttpClientHandler, SocketsHttpHandler> CreateInnerHandlerAccessor()
    {
        var accessor = typeof(HttpClientHandler)
            .GetProperty("Handler", B.NonPublic | B.Instance)?
            .GetMethod?
            .CreateDelegate<Func<HttpClientHandler, SocketsHttpHandler>>();

        if (accessor is null)
            throw new MethodAccessException("Failed to create HttpClientHandler.Handler accessor.");

        return accessor;
    }

    public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection parameters)
    {
        parameters.Add(new Func<HttpClientHandler, HttpMessageHandler>(DecorateHandler));
    }

    private HttpMessageHandler DecorateHandler(HttpClientHandler handler)
    {
        InnerHandlerAccessor(handler).ConnectCallback = OnConnectAsync;
        return handler;
    }

    private async ValueTask<Stream> OnConnectAsync(
        SocketsHttpConnectionContext context, CancellationToken cancellation)
    {
        var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

        try
        {
            socket.NoDelay = true;

            socket.SetSocketOption(L.Socket, N.KeepAlive,            true);
            socket.SetSocketOption(L.Tcp,    N.TcpKeepAliveTime,      120);
            socket.SetSocketOption(L.Tcp,    N.TcpKeepAliveInterval,   10);
            socket.SetSocketOption(L.Tcp,    N.TcpKeepAliveRetryCount, 12);

            await socket
                .ConnectAsync(context.DnsEndPoint, cancellation)
                .ConfigureAwait(false);

            return new NetworkStream(socket, ownsSocket: true);
        }
        catch
        {
            socket.Dispose();
            throw;
        }
    }

    public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime runtime)
    {
        // NOP
    }

    public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher dispatcher)
    {
        // NOP
    }

    public void Validate(ServiceEndpoint endpoint)
    {
        // NOP
    }
}
