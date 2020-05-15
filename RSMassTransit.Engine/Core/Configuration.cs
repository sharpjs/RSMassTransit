/*
    Copyright (C) 2020 Jeffrey Sharp

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
using RSMassTransit.Storage;
using Sharp.BlobStorage.Azure;
using Sharp.BlobStorage.File;

namespace RSMassTransit
{
    public class Configuration : IBusConfiguration, IStorageConfiguration
    {
        public Uri         BusUri        { get; set; } = new Uri("rabbitmq://localhost", UriKind.Absolute);
        public string      BusQueue      { get; set; } = "reports";
        public string      BusSecretName { get; set; } = "guest";
        public string      BusSecret     { get; set; } = "guest";
        public StorageType StorageType   { get; set; } = StorageType.File;

        public FileBlobStorageConfiguration File { get; } = new FileBlobStorageConfiguration
        {
            Path = @"C:\Reports"
        };

        public AzureBlobStorageConfiguration Azure { get; } = new AzureBlobStorageConfiguration
        {
            ConnectionString = "UseDevelopmentStorage=true",
            ContainerName    = "reports"
        };

#if OLD
        private static string GetString(NameValueCollection settings, string name, string defaultValue)
            => settings[name].NullIfEmpty() ?? defaultValue;

        private static Uri GetUri(NameValueCollection settings, string name, string defaultValue)
        {
            var text = GetString(settings, name, defaultValue);

            if (Uri.TryCreate(text, UriKind.Absolute, out Uri uri))
                return uri;

            throw new FormatException(string.Format(
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

            throw new FormatException(string.Format(
                "The value '{1}' is invalid for application setting '{0}'.  " +
                "The value must be one of: {2}",
                name, text, validValues
            ));
        }

        private int? GetNullableInt32(NameValueCollection settings, string name, int? defaultValue)
        {
            var text = settings[name].NullIfEmpty();
            if (text == null)
                return defaultValue;

            if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
                return value;

            throw new FormatException(string.Format(
                "The value '{1}' is invalid for application setting '{0}'.  " +
                "The value must be an integer, digits only, with optional leading sign, " +
                "between -2147483648 and +2147483647.",
                name, text
            ));
        }
#endif
    }
}
