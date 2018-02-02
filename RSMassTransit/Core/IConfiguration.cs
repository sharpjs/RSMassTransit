using System;

namespace RSMassTransit
{
    internal interface IConfiguration
        : IServiceConfiguration
        , IMessageBusConfiguration
    { }

    internal interface IServiceConfiguration
    {
        string InstanceId { get; }
    }

    internal interface IMessageBusConfiguration
    {
        Uri    BusUri        { get; }
        string BusQueue      { get; }
        string BusSecret     { get; }
        string BusSecretName { get; }
    }
}
