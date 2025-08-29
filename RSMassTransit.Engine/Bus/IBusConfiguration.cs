// Copyright Subatomix Research Inc.
// SPDX-License-Identifier: MIT

namespace RSMassTransit.Bus;

/// <summary>
///   Configuration for bus access.
/// </summary>
internal interface IBusConfiguration
{
    /// <summary>
    ///   Gets the URI of the bus host.
    /// </summary>
    Uri HostUri { get; }

    /// <summary>
    ///   Gets the name of the queue from which to consume requests.
    /// </summary>
    string QueueName { get; }

    /// <summary>
    ///   Gets the name of the shared secret used for authentication.
    /// </summary>
    string Secret { get; }

    /// <summary>
    ///   Gets the content of the shared secret used for authentication.
    /// </summary>
    string SecretName { get; }
}
