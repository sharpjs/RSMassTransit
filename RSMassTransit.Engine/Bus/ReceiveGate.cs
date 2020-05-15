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
using System.Runtime.Serialization;
using System.Threading.Tasks;
using MassTransit;
using Sharp.Disposable;

namespace RSMassTransit.Core
{
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

        [Serializable]
        private class GateClosedException : OperationCanceledException
        {
            private const string
                DefaultMessage = "Cannot consume the received message, because the receive gate is closed.";

            public GateClosedException()
                : base(DefaultMessage) { }

            protected GateClosedException(SerializationInfo info, StreamingContext context)
                : base(info, context) { }
        }
    }
}
