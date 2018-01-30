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
        string MessageBusHost       { get; }
        string MessageBusQueue      { get; }
        string MessageBusSecret     { get; }
        string MessageBusSecretName { get; }
        string MessageBusType       { get; }
    }
}
