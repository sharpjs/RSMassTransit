using System;
using RSMassTransit.Storage;

namespace RSMassTransit
{
    internal interface IConfiguration
        : IServiceConfiguration
        , IBusConfiguration
        , IStorageConfiguration
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
