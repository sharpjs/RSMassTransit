namespace RSMassTransit
{
    internal class Configuration
    {
        private static Configuration _current;

        public static Configuration Current
            => _current ?? (_current = new Configuration());

        public string InstanceId { get; } = "Default";
    }
}
