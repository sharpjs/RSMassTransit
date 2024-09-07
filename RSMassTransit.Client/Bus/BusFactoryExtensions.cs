// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

namespace RSMassTransit.Bus;

internal static class BusFactoryExtensions
{
    /// <summary>
    ///   Discard messages that fault and unconsumable messages, rather than
    ///   moving them to _error or _skipped queues.
    /// </summary>
    /// <param name="b">
    ///   The bus factory configurator.
    /// </param>
    internal static void DiscardFaultAndSkippedMessages(this IBusFactoryConfigurator b)
    {
        b.ConnectEndpointConfigurationObserver(
            new DiscardFaultAndSkippedMessagesBehavior()
        );
    }
}
