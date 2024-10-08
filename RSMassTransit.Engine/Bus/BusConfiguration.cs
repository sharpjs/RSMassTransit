// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

using Microsoft.Extensions.Configuration;

namespace RSMassTransit.Bus;

/// <summary>
///   Configuration for bus access.
/// </summary>
internal class BusConfiguration : IBusConfiguration
{
    private const string
        DefaultQueueName  = "reports",
        DefaultSecretName = "guest",
        DefaultSecret     = "guest";

    private static Uri
        DefaultHostUri => new("rabbitmq://localhost", UriKind.Absolute);

    /// <summary>
    ///   Gets or sets the URI of the bus host. The default value is
    ///   <c>rabbitmq://localhost</c>.
    /// </summary>
    public Uri HostUri { get; set; } = DefaultHostUri;

    /// <summary>
    ///   Gets or sets the name of the queue from which to consume
    ///   requests. The default value is <c>reports</c>.
    /// </summary>
    public string QueueName { get; set; } = DefaultQueueName;

    /// <summary>
    ///   Gets or sets the name of the shared secret used for
    ///   authentication.  The default value is <c>guest</c>.
    /// </summary>
    public string SecretName { get; set; } = DefaultSecretName;

    /// <summary>
    ///   Gets or sets the content of the shared secret used for
    ///   authentication.  The default value is <c>guest</c>.
    /// </summary>
    public string Secret { get; set; } = DefaultSecret;

    /// <summary>
    ///   Initializes a new <see cref="BusConfiguration"/> instance with
    ///   values from the specified configuration repository.
    /// </summary>
    /// <param name="configuration">
    ///   The configuration repository from which to load values.
    /// </param>
    public BusConfiguration(IConfiguration configuration)
    {
        Load(configuration);
    }

    /// <summary>
    ///   Loads values from the specified configuration repository.
    /// </summary>
    /// <param name="configuration">
    ///   The configuration repository from which to load values.
    /// </param>
    public void Load(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        HostUri    = configuration.GetUri    (nameof(HostUri))    ?? DefaultHostUri;
        QueueName  = configuration.GetString (nameof(QueueName))  ?? DefaultQueueName;
        SecretName = configuration.GetString (nameof(SecretName)) ?? DefaultSecretName;
        Secret     = configuration.GetString (nameof(Secret))     ?? DefaultSecret;
    }
}
