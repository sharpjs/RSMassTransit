// Copyright (C) 2018 (to be determined)

using RSMassTransit.Storage;

namespace RSMassTransit
{
    internal interface IConfiguration
        : IBusConfiguration
        , IStorageConfiguration
    {
        string InstanceId { get; }
    }
}
