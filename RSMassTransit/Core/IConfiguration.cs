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
        string BusHost       { get; }
        string BusQueue      { get; }
        string BusSecret     { get; }
        string BusSecretName { get; }
        string BusType       { get; }
    }
}
