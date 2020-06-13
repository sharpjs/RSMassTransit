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
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using RSMassTransit.Client.Internal;
using RSMassTransit.Messages;
using static System.Reflection.BindingFlags;
using static System.StringComparison;

#pragma warning disable CS0618 // Type or member is obsolete

namespace RSMassTransit.Client
{
    /// <summary>
    ///   Base class for RSMassTransit clients.
    /// </summary>
    public abstract class ReportingServices : IReportingServices
    {
        private readonly IBusControl _bus;
        private readonly Uri         _queueUri;
        private int                  _isDisposed;

        private const string         AssemblyPattern = "RSMassTransit.Client.*.dll";
        private const string         AssemblyPrefix  = "RSMassTransit.Client.";
        private static bool          _assembliesLoaded;

        /// <summary>
        ///   Creates a new <see cref="ReportingServices"/> instance with the
        ///   specified configuration.
        /// </summary>
        /// <param name="configuration">
        ///   The configuration for the client, specifying how to communicate
        ///   with RSMassTransit.
        /// </param>
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
            => Send<IExecuteReportRequest, IExecuteReportResponse>(request, timeout);

        /// <inheritdoc/>
        public virtual Task<IExecuteReportResponse> ExecuteReportAsync(
            IExecuteReportRequest request,
            TimeSpan?             timeout           = default,
            CancellationToken     cancellationToken = default)
            => SendAsync<IExecuteReportRequest, IExecuteReportResponse>(request, timeout, cancellationToken);

        private TResponse Send<TRequest, TResponse>(
            TRequest          request,
            TimeSpan?         timeout           = default,
            CancellationToken cancellationToken = default)
            where TRequest  : class
            where TResponse : class
        {
            using (new AsyncScope())
                return SendAsync<TRequest, TResponse>(request, timeout).GetResultOrThrowUnwrapped();
        }

        private Task<TResponse> SendAsync<TRequest, TResponse>(
            TRequest          request,
            TimeSpan?         timeout           = default,
            CancellationToken cancellationToken = default)
            where TRequest  : class
            where TResponse : class
        {
            return CreateRequestClient<TRequest, TResponse>(timeout)
                .Request(request, cancellationToken);
        }

        /// <summary>
        ///   Creates a MassTransit request client.
        /// </summary>
        /// <typeparam name="TRequest">Type of the request.</typeparam>
        /// <typeparam name="TResponse">Type of the response.</typeparam>
        /// <param name="timeout">If specified, overrides the configured timeout.</param>
        /// <returns>A MassTransit request client.</returns>
        protected virtual IRequestClient<TRequest, TResponse>
            CreateRequestClient<TRequest, TResponse>(TimeSpan? timeout = null)
            where TRequest  : class
            where TResponse : class
        {
            return _bus.CreateRequestClient<TRequest, TResponse>(
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
        public static ReportingServices Create(ReportingServicesConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            LoadAssemblies();
            var supportedSchemes = DiscoverSupportedSchemes();
            var requestedScheme  = configuration.BusUri?.Scheme;

            if (!supportedSchemes.TryGetValue(requestedScheme, out Type type))
                throw OnUnsupportedScheme(requestedScheme, supportedSchemes.Keys);

            return (ReportingServices) Activator.CreateInstance(type, configuration);
        }

        private static void LoadAssemblies()
        {
            if (_assembliesLoaded)
                return;

            string directory = GetAssemblyDirectorySafe();

            if (!string.IsNullOrEmpty(directory))
                foreach (var path in Directory.GetFiles(directory, AssemblyPattern))
                    Assembly.LoadFrom(path);

            _assembliesLoaded = true;
        }

        private static string GetAssemblyDirectorySafe()
        {
            string path;

            try
            {
                path = typeof(ReportingServices).Assembly.Location;
            }
            catch (NotSupportedException)
            {
                return null;
            }

            return string.IsNullOrEmpty(path)
                ? null
                : Path.GetDirectoryName(path);
        }

        private static SortedDictionary<string, Type> DiscoverSupportedSchemes()
        {
            var schemes = new SortedDictionary<string, Type>(StringComparer.Ordinal);

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                // Assembly must be from RSMassTransit.Client family
                if (!assembly.GetName().Name.StartsWith(AssemblyPrefix, Ordinal))
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
            var message = new StringBuilder();

            message.AppendFormat(
                "The URI scheme '{0}' is not supported by any loaded RSMassTransit client type.  ",
                requestedScheme
            );

            if (supportedSchemes.Any())
                message.Append("Supported schemes are: ")
                    .AppendDelimitedList(supportedSchemes);
            else
                message.Append(
                    "No RSMassTransit client types are loaded.  " +
                    "Did you forget to install a client package?"
                );

            return new ArgumentException(message.ToString(), "configuration");
        }

        /// <summary>
        ///   Validates the configured bus URI and converts it to a normalized
        ///   form.
        /// </summary>
        /// <param name="scheme">The required URI scheme.</param>
        /// <param name="kind">A short human-readable name for the kind of bus URI.</param>
        /// <returns>The normalized bus URI.</returns>
        protected virtual Uri NormalizeBusUri(string scheme, string kind)
        {
            var uri = Configuration.BusUri;

            var valid
                =  uri != null
                && uri.IsAbsoluteUri
                && uri.Scheme.Equals(scheme, OrdinalIgnoreCase)
                && !string.IsNullOrEmpty(uri.Host);

            if (!valid)
                throw new ConfigurationException(string.Format(
                    "Invalid RSMassTransit client configuration.  " +
                    "The BusUri value '{0}' is not a valid {2} bus URI.  " +
                    "The value must be an absolute URI with scheme '{1}' and must contain a hostname.",
                    uri, scheme, kind
                ));

            return uri;
        }

        /// <summary>
        ///   Validates the configured queue name and converts it to a
        ///   normalized form.
        /// </summary>
        /// <returns>The normalized queue name.</returns>
        protected virtual string NormalizeBusQueue()
        {
            var queue = Configuration.BusQueue;

            return string.IsNullOrEmpty(queue)
                ? ReportingServicesConfiguration.DefaultBusQueue
                : queue;
        }

        /// <summary>
        ///   Validates the configured bus credential and converts to a
        ///   normalized form.
        /// </summary>
        /// <returns>The normalized bus credential.</returns>
        protected virtual NetworkCredential NormalizeBusCredential()
        {
            var credential = Configuration.BusCredential;

            if (credential == null)
                throw new ConfigurationException(
                    "Invalid RSMassTransit client configuration.  " +
                    "The BusCredential property is null."
                );

            return credential;
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
                _bus?.Stop();
        }
    }
}
