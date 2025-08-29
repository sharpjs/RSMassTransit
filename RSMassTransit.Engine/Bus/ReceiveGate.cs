// Copyright Subatomix Research Inc.
// SPDX-License-Identifier: MIT

using Sharp.Async;
using Sharp.Disposable;

namespace RSMassTransit.Bus;

internal class ReceiveGate : Disposable, IReceiveObserver
{
    private readonly AsyncGate _gate;

    public ReceiveGate()
    {
        _gate = new AsyncGate(true);
    }

    public bool IsOpen
    {
        get => _gate.IsOpen;
        set => _gate.IsOpen = value;
    }

    public Task PreReceive(ReceiveContext context)
    {
        if (!IsOpen)
            throw new GateClosedException();

        return Task.CompletedTask;
    }

    public Task PostConsume<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType)
        where T : class
    {
        return Task.CompletedTask;
    }

    public Task ConsumeFault<T>(ConsumeContext<T> context, TimeSpan duration, string consumerType, Exception exception)
        where T : class
    {
        return Task.CompletedTask;
    }

    public Task PostReceive(ReceiveContext context)
    {
        return Task.CompletedTask;
    }

    public Task ReceiveFault(ReceiveContext context, Exception exception)
    {
        return exception is GateClosedException
            ? _gate.WaitAsync()
            : Task.CompletedTask;
    }

#if !NET8_0_OR_GREATER
    [Serializable]
#endif
    private class GateClosedException : OperationCanceledException
    {
        private const string
            DefaultMessage = "Cannot consume the received message, because the receive gate is closed.";

        public GateClosedException()
            : base(DefaultMessage) { }

#if !NET8_0_OR_GREATER
        protected GateClosedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
#endif
    }
}
