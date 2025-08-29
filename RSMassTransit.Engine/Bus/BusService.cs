// Copyright Subatomix Research Inc.
// SPDX-License-Identifier: MIT

using Microsoft.Extensions.Hosting;

namespace RSMassTransit.Bus;

internal class BusService : IHostedService
{
    public IBusControl Bus { get; }

    public BusService(IBusControl bus)
        => Bus = bus ?? throw new ArgumentNullException(nameof(bus));

    public Task StartAsync(CancellationToken cancellationToken)
        => Bus.StartAsync(cancellationToken);

    public Task StopAsync(CancellationToken cancellationToken)
        => Bus.StopAsync(cancellationToken);
}
