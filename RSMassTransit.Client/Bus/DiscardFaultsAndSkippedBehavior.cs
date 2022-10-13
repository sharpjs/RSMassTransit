/*
    Copyright 2022 Jeffrey Sharp

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
