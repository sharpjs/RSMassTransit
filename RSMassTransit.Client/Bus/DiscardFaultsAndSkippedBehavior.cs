// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

using MassTransit.EndpointConfigurators;

namespace RSMassTransit.Bus;

internal class DiscardFaultAndSkippedMessagesBehavior : IEndpointConfigurationObserver
{
    public void EndpointConfigured<T>(T configurator)
        where T : IReceiveEndpointConfigurator
    {
        if (configurator is IReceivePipelineConfigurator c)
            Configure(c);
    }

    private void Configure(IReceivePipelineConfigurator c)
    {
        // RSMassTransit's current users do not monitor an _error queue.
        // Prevent MT from inadvertently filling one to its limit.
        c.DiscardFaultedMessages();

        // RSMassTransit's current users do not monitor a _skipped queue.
        // Prevent MT from inadvertently filling one to its limit.
        c.DiscardSkippedMessages();
    }
}
