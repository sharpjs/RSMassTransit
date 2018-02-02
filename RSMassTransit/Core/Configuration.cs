using System;
using System.Collections.Specialized;
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

            InstanceId    = GetString (settings, nameof(InstanceId),    "Default");

            BusUri        = GetUri    (settings, nameof(BusQueue),      "rabbitmq://localhost");
            BusQueue      = GetString (settings, nameof(BusQueue),      "reports");
            BusSecretName = GetString (settings, nameof(BusSecretName), "guest");
            BusSecret     = GetString (settings, nameof(BusSecret),     "guest");
        }

        public string InstanceId    { get; private set; }

        public Uri    BusUri        { get; private set; }
        public string BusQueue      { get; private set; }
        public string BusSecretName { get; private set; }
        public string BusSecret     { get; private set; }

        private static Uri GetUri(NameValueCollection settings, string name, string defaultValue)
        {
            var text = GetString(settings, name, defaultValue);
            var ok   = Uri.TryCreate(text, UriKind.Absolute, out Uri uri);
            if (ok) return uri;

            throw new ConfigurationErrorsException(string.Format(
                "The value '{1}' is invalid for application setting '{0}'.  " +
                "The value must be an absolute URI.",
                name, text
            ));
        }

        private static string GetString(NameValueCollection settings, string name, string defaultValue)
            => settings[name].NullIfEmpty() ?? defaultValue;
    }
}
