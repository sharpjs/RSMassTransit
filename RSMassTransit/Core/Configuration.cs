/*
    Copyright (C) 2018 Jeffrey Sharp

    Permission to use, copy, modify, and distribute this software for any
    purpose with or without fee is hereby granted, provided that the above
    copyright notice and this permission notice appear in all copies.

    THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
    WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
    MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
    ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
    WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
    ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
    OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
*/

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using RSMassTransit.Storage;

namespace RSMassTransit
{
    internal class Configuration : IConfiguration
    {
        private static Configuration _current;

        public static Configuration Current
            => _current ?? (_current = new Configuration());

        public Configuration()
        {
            var settings   = ConfigurationManager.AppSettings;

            InstanceId     = GetString (settings, nameof(InstanceId),     "Default");

            BusUri         = GetUri    (settings, nameof(BusUri),         "rabbitmq://localhost");
            BusQueue       = GetString (settings, nameof(BusQueue),       "reports");
            BusSecretName  = GetString (settings, nameof(BusSecretName),  "guest");
            BusSecret      = GetString (settings, nameof(BusSecret),      "guest");

            StorageType    = GetEnum   (settings, nameof(StorageType),    StorageType.FileSystem);
            FileSystemPath = GetString (settings, nameof(FileSystemPath), @"C:\Reports");

            AzureStorageConnectionString
                = GetString(settings, nameof(AzureStorageConnectionString), "UseDevelopmentStorage=true");

            AzureStorageContainer
                = GetString(settings, nameof(AzureStorageContainer), "reports");
        }

        public string InstanceId    { get; private set; }

        public Uri    BusUri        { get; private set; }
        public string BusQueue      { get; private set; }
        public string BusSecretName { get; private set; }
        public string BusSecret     { get; private set; }

        public StorageType StorageType                  { get; private set; }
        public string      FileSystemPath               { get; private set; }
        public string      AzureStorageConnectionString { get; private set; }
        public string      AzureStorageContainer        { get; private set; }

        private static string GetString(NameValueCollection settings, string name, string defaultValue)
            => settings[name].NullIfEmpty() ?? defaultValue;

        private static Uri GetUri(NameValueCollection settings, string name, string defaultValue)
        {
            var text = GetString(settings, name, defaultValue);

            if (Uri.TryCreate(text, UriKind.Absolute, out Uri uri))
                return uri;

            throw new ConfigurationErrorsException(string.Format(
                "The value '{1}' is invalid for application setting '{0}'.  " +
                "The value must be an absolute URI.",
                name, text
            ));
        }

        private T GetEnum<T>(NameValueCollection settings, string name, T defaultValue)
            where T : struct // Enum
        {
            var text = settings[name].NullIfEmpty();
            if (text == null)
                return defaultValue;

            if (Enum.TryParse(text, out T value))
                return value;

            var validValues = string.Join(
                ", ",
                Enum.GetNames(typeof(T)).Select(n => $"'{n}'")
            );

            throw new ConfigurationErrorsException(string.Format(
                "The value '{1}' is invalid for application setting '{0}'.  " +
                "The value must be one of: {2}",
                name, text, validValues
            ));
        }
    }
}
