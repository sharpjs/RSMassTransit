// Copyright (C) 2018 (to be determined)

using System.Threading.Tasks;
using MassTransit;
using RSMassTransit.Messages;

namespace RSMassTransit.Consumers
{
    internal class ErrorConsumer : IConsumer<IMessage>
    {
        public Task Consume(ConsumeContext<IMessage> context)
        {
            Log.Verbose("Consuming message from error queue.");
            return Task.CompletedTask;
        }
    }
}
