using System.Configuration;

namespace RSMassTransit
{
    internal class Configuration : IConfiguration
    {
        private static Configuration _current;

        public static Configuration Current
            => _current ?? (_current = new Configuration());

        public Configuration()
        {
            var settings  = ConfigurationManager.AppSettings;

            InstanceId    = settings[nameof(InstanceId)]    ?? "Default";

            BusType       = settings[nameof(BusType)]       ?? "RabbitMQ";
            BusHost       = settings[nameof(BusHost)]       ?? "localhost";
            BusQueue      = settings[nameof(BusQueue)]      ?? "reports";
            BusSecretName = settings[nameof(BusSecretName)] .NullIfEmpty();
            BusSecret     = settings[nameof(BusSecret)]     .NullIfEmpty();
        }

        public string InstanceId    { get; private set; }

        public string BusType       { get; private set; }
        public string BusHost       { get; private set; }
        public string BusQueue      { get; private set; }
        public string BusSecretName { get; private set; }
        public string BusSecret     { get; private set; }
    }
}
