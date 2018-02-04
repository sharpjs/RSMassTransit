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
