using System;

namespace RSMassTransit
{
    internal interface IConfiguration
        : IServiceConfiguration
        , IBusConfiguration
    { }

    internal interface IServiceConfiguration
    {
        string InstanceId { get; }
    }

    internal interface IBusConfiguration
    {
        Uri    BusUri        { get; }
        string BusQueue      { get; }
        string BusSecret     { get; }
        string BusSecretName { get; }
    }
}
