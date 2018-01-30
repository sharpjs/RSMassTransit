using System.Configuration;

namespace RSMassTransit
{
    internal class Configuration
    {
        private static Configuration _current;

        public static Configuration Current
            => _current ?? (_current = new Configuration());

        public Configuration()
        {
            var settings = ConfigurationManager.AppSettings;

            InstanceId           = settings[nameof(InstanceId)]             ?? "Default";

            MessageBusType       = settings[nameof(MessageBusType)]         ?? "RabbitMQ";
            MessageBusHost       = settings[nameof(MessageBusHost)]         ?? "localhost";
            MessageBusQueue      = settings[nameof(MessageBusQueue)]        ?? "reports";
            MessageBusSecretName = settings[nameof(MessageBusSecretName)]   .NullIfEmpty();
            MessageBusSecret     = settings[nameof(MessageBusSecret)]       .NullIfEmpty();
        }

        public string InstanceId             { get; private set; }

        public string MessageBusType         { get; private set; }
        public string MessageBusHost         { get; private set; }
        public string MessageBusQueue        { get; private set; }
        public string MessageBusSecretName   { get; private set; }
        public string MessageBusSecret       { get; private set; }
    }
}
