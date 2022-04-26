/*
    Copyright 2021 Jeffrey Sharp

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
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace RSMassTransit
{
    internal static class ConfigurationExtensions
    {
        public static string? GetString(this IConfiguration configuration, string key)
        {
            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

            return configuration[key].NullIfEmpty();
        }

        public static Uri? GetUri(this IConfiguration configuration, string key)
        {
            var text = configuration.GetString(key);
            if (text is null)
                return null;

            if (Uri.TryCreate(text, UriKind.Absolute, out var uri))
                return uri;

            throw new ConfigurationException(string.Format(
                "The value '{1}' is invalid for application setting '{0}'.  " +
                "The value must be an absolute URI.",
                configuration.GetKeyPath(key), text
            ));
        }

        public static T? GetEnum<T>(this IConfiguration configuration, string key)
            where T : struct, Enum
        {
            var text = configuration.GetString(key);
            if (text == null)
                return null;

            if (Enum.TryParse(text, out T value))
                return value;

            var validValues = string.Join(", ", typeof(T).GetEnumNames());

            throw new ConfigurationException(string.Format(
                "The value '{1}' is invalid for application setting '{0}'.  " +
                "The value must be one of: {2}",
                configuration.GetKeyPath(key), text, validValues
            ));
        }

        public static int? GetInt32(
            this IConfiguration configuration,
            string              key,
            int                 min = int.MinValue,
            int                 max = int.MaxValue)
        {
            var text = configuration.GetString(key);
            if (text == null)
                return null;

            if (int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value)
                    && value >= min && value <= max)
                return value;

            throw new ConfigurationException(string.Format(
                "The value '{1}' is invalid for application setting '{0}'.  " +
                "The value must be an integer, digits only, with optional leading sign, " +
                "between {2} and {3}.",
                configuration.GetKeyPath(key), text, min, max
            ));
        }

        public static object GetKeyPath(this IConfiguration configuration, string key)
        {
            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

            return configuration is IConfigurationSection section
                ? string.Concat(section.Path, ":", key)
                : key;
        }
    }
}
