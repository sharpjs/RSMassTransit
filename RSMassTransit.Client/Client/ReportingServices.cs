// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

using System.Net;
using System.Reflection;
using System.Text;
using RSMassTransit.Client.Internal;
using RSMassTransit.Messages;
using Sharp.Async;

namespace RSMassTransit.Client;

using static BindingFlags;
using static StringComparison;

/// <summary>
///   Base class for RSMassTransit clients.
/// </summary>
public abstract class ReportingServices : IReportingServices
{
    private const string
        ClientAssemblyPrefix  = "RSMassTransit.Client.",
        ClientAssemblyPattern = "RSMassTransit.Client.*.dll";

    private static bool          _clientAssembliesLoaded;

    private readonly IBusControl _bus;
    private readonly Uri         _queueUri;
    private int                  _isDisposed;

    /// <summary>
    ///   Creates a new <see cref="ReportingServices"/> instance with the
    ///   specified configuration.
    /// </summary>
    /// <param name="configuration">
    ///   The configuration for the client, specifying how to communicate
    ///   with RSMassTransit.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="configuration"/> is <see langword="null"/>.
    /// </exception>
    protected ReportingServices(ReportingServicesConfiguration configuration)
    {
        Configuration = configuration
            ?? throw new ArgumentNullException(nameof(configuration));

        var bus = CreateBus(out _queueUri);
        bus.Start();
        _bus = bus;
    }

    /// <summary>
    ///   The configuration of the client, specifying how to communicate
    ///   with RSMassTransit.
    /// </summary>
    public ReportingServicesConfiguration Configuration { get; }

    /// <summary>
    ///   When implemented in a derived class, creates the message bus
    ///   instance used to communicate with RSMassTransit.
    /// </summary>
    /// <param name="queueUri">
    ///   When this method returns, contains the normalized URI of the bus
    ///   queue used to send and receive messages.
    /// </param>
    /// <returns>
    ///   The message bus instance on which to send and receive messages.
    /// </returns>
    protected abstract IBusControl CreateBus(out Uri queueUri);

    /// <inheritdoc/>
    public virtual IExecuteReportResponse ExecuteReport(
        IExecuteReportRequest request,
        TimeSpan?             timeout = default)
    {
        return Send<IExecuteReportRequest, IExecuteReportResponse>(
            request, timeout
        );
    }

    /// <inheritdoc/>
    public virtual Task<IExecuteReportResponse> ExecuteReportAsync(
        IExecuteReportRequest request,
        TimeSpan?             timeout           = default,
        CancellationToken     cancellationToken = default)
    {
        return SendAsync<IExecuteReportRequest, IExecuteReportResponse>(
            request, timeout, cancellationToken
        );
    }

    private TResponse Send<TRequest, TResponse>(
        TRequest          request,
        TimeSpan?         timeout           = default,
        CancellationToken cancellationToken = default)
        where TRequest  : class
        where TResponse : class
    {
        using var _ = new AsyncScope();

        return SendAsync<TRequest, TResponse>(request, timeout, cancellationToken)
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();
    }

    private async Task<TResponse> SendAsync<TRequest, TResponse>(
        TRequest          request,
        TimeSpan?         timeout           = default,
        CancellationToken cancellationToken = default)
        where TRequest  : class
        where TResponse : class
    {
        var response = await CreateRequestClient<TRequest>(timeout)
            .GetResponse<TResponse>(request, cancellationToken)
            .ConfigureAwait(false);

        return response.Message;
    }

    /// <summary>
    ///   Creates a MassTransit request client.
    /// </summary>
    /// <typeparam name="TRequest">
    ///   Type of the request.
    /// </typeparam>
    /// <param name="timeout">
    ///   If specified, overrides the configured timeout.
    /// </param>
    /// <returns>
    ///   A MassTransit request client.
    /// </returns>
    protected virtual IRequestClient<TRequest>
        CreateRequestClient<TRequest>(TimeSpan? timeout = null)
        where TRequest : class
    {
        return _bus.CreateRequestClient<TRequest>(
            _queueUri,
            timeout ?? Configuration.RequestTimeout
        );
    }

    /// <summary>
    ///   Creates a new <see cref="ReportingServices"/> instance with
    ///   the specified configuration.
    /// </summary>
    /// <param name="configuration">
    ///   The configuration for the client, specifying how to communicate
    ///   with RSMassTransit.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="configuration"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///   The URI scheme specified by <paramref name="configuration"/> in
    ///   the <see cref="ReportingServicesConfiguration.BusUri"/> property
    ///   is not supported by any loaded RSMassTransit client type.
    /// </exception>
    /// <exception cref="FileLoadException">
    ///   Could not load a client assembly (<c>RSMassTransit.Client.*.dll</c>).
    /// </exception>
    public static ReportingServices Create(ReportingServicesConfiguration configuration)
    {
        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration));

        LoadAssemblies();
        var supportedSchemes = DiscoverSupportedSchemes();
        var requestedScheme  = configuration.BusUri?.Scheme ?? "(not specified)";

        if (!supportedSchemes.TryGetValue(requestedScheme, out Type type))
            throw OnUnsupportedScheme(requestedScheme, supportedSchemes.Keys);

        return (ReportingServices) Activator.CreateInstance(type, configuration);
    }

    private static void LoadAssemblies()
    {
        if (_clientAssembliesLoaded)
            return;

        if (GetAssemblyDirectory() is string directory)
            foreach (var path in Directory.GetFiles(directory, ClientAssemblyPattern))
                Assembly.LoadFrom(path);

        _clientAssembliesLoaded = true;
    }

    private static string? GetAssemblyDirectory()
    {
        // This is not a dynamic assembly, so Assembly.Location will not
        // throw.  This assembly might have been loaded via Load(byte[]),
        // in which case its location is "".
        var path = typeof(ReportingServices).Assembly.Location;

        // GetDirectoryName(null or "") behavior differs between target
        // frameworks, so avoid that case.
        return path is null or ""
            ? null
            : Path.GetDirectoryName(path);
    }

    private static SortedDictionary<string, Type> DiscoverSupportedSchemes()
    {
        var schemes = new SortedDictionary<string, Type>(StringComparer.Ordinal);

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            // Assembly must be from RSMassTransit.Client family
            if (!assembly.GetName().Name.StartsWith(ClientAssemblyPrefix, Ordinal))
                continue;

            foreach (var type in assembly.GetExportedTypes())
            {
                // Type must be a concrete client class
                if (!typeof(ReportingServices).IsAssignableFrom(type) || type.IsAbstract)
                    continue;

                // Type must declare a URI scheme
                var schemeField = type.GetField("UriScheme", Public | Static);
                if (schemeField.FieldType != typeof(string))
                    continue;

                // URI scheme must be non-null
                var scheme = schemeField.GetValue(obj: null) as string;
                if (scheme != null)
                    schemes.Add(scheme, type);
            }
        }

        return schemes;
    }

    private static Exception OnUnsupportedScheme(
        string              requestedScheme,
        IEnumerable<string> supportedSchemes)
    {
        var message = new StringBuilder()
            .Append("The URI scheme '")
            .Append(requestedScheme)
            .Append("' is not supported by any loaded RSMassTransit client type.  ");

        if (supportedSchemes.Any())
            message
                .Append("Supported schemes are: ")
                .AppendDelimitedList(supportedSchemes);
        else
            message
                .Append(
                    "No RSMassTransit client types are loaded.  " +
                    "Did you forget to install a client package?"
                );

        return new ArgumentException(message.ToString(), "configuration");
    }

    /// <summary>
    ///   Validates the configured bus URI and converts it to a normalized
    ///   form.
    /// </summary>
    /// <param name="scheme">
    ///   The required URI scheme.
    /// </param>
    /// <param name="kind">
    ///   A short human-readable name for the kind of bus URI.
    /// </param>
    /// <returns>
    ///   The normalized bus URI.
    /// </returns>
    /// <exception cref="ConfigurationException">
    ///   The <see cref="ReportingServicesConfiguration.BusUri"/> value is
    ///   not a valid URI for the <paramref name="kind"/> of bus.
    /// </exception>
    protected virtual Uri NormalizeBusUri(string scheme, string kind)
    {
        var uri = Configuration.BusUri;

        if (uri is not null
            && uri.IsAbsoluteUri
            && uri.Scheme.Equals(scheme, OrdinalIgnoreCase)
            && uri.Host is not (null or ""))
        {
            return uri;
        }

        throw new ConfigurationException(string.Format(
            "Invalid RSMassTransit client configuration.  " +
            "The BusUri value '{0}' is not a valid {2} bus URI.  " +
            "The value must be an absolute URI with scheme '{1}' and must contain a hostname.",
            uri, scheme, kind
        ));
    }

    /// <summary>
    ///   Validates the configured queue name and converts it to a
    ///   normalized form.
    /// </summary>
    /// <returns>
    ///   The normalized queue name.
    /// </returns>
    protected virtual string NormalizeBusQueue()
    {
        var queue = Configuration.BusQueue;

        return queue is null or ""
            ? ReportingServicesConfiguration.DefaultBusQueue
            : queue;
    }

    /// <summary>
    ///   Validates the configured bus credential and converts to a
    ///   normalized form.
    /// </summary>
    /// <returns>
    ///   The normalized bus credential.
    /// </returns>
    /// <exception cref="ConfigurationException">
    ///   The <see cref="ReportingServicesConfiguration.BusCredential"/>
    ///   property is <see langword="null"/>.
    /// </exception>
    protected virtual NetworkCredential NormalizeBusCredential()
    {
        var credential = Configuration.BusCredential;

        if (credential is not null)
            return credential;

        throw new ConfigurationException(
            "Invalid RSMassTransit client configuration.  " +
            "The BusCredential property is null."
        );
    }

    /// <summary>
    ///   Stops the bus instance used by the client and releases any
    ///   managed or unmanaged resources owned by the client.
    /// </summary>
    public void Dispose()
    {
        Dispose(managed: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///   Invoked by either <see cref="Dispose()"/> or the finalizer thread.
    ///   Stops the bus instance used by the client and releases any
    ///   resources of the specified kind owned by the client.
    /// </summary>
    /// <param name="managed">
    ///   <c>true</c> to dispose managed an unmanaged resources;
    ///   <c>false</c> to dispose only unamanged resources.
    /// </param>
    /// <remarks>
    ///   The current <see cref="ReportingServices"/> implementation does
    ///   not expect to own unmanaged resources and thus does not provide a
    ///   finalizer.  Thus the <paramref name="managed"/> parameter always
    ///   will be <c>true</c>.
    /// </remarks>
    protected virtual void Dispose(bool managed)
    {
        if (Interlocked.Exchange(ref _isDisposed, 1) == 1)
            return; // already disposed

        if (managed)
            _bus.Stop();
    }
}
