// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace RSMassTransit;

internal static class ConfigurationExtensions
{
    public static string? GetString(this IConfiguration configuration, string key)
    {
        ArgumentNullException.ThrowIfNull(configuration);

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
        ArgumentNullException.ThrowIfNull(configuration);

        return configuration is IConfigurationSection section
            ? string.Concat(section.Path, ":", key)
            : key;
    }
}
