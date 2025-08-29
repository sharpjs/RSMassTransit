// Copyright Subatomix Research Inc.
// SPDX-License-Identifier: MIT

using System.Text;

namespace RSMassTransit.Client.Internal;

/// <summary>
///   Extensions for <see cref="StringBuilder"/>.
/// </summary>
internal static class StringBuilderExtensions
{
    /// <summary>
    ///   Appends a delimited, separated list to the
    ///   <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="builder">
    ///   The instance to which the list should be appended.
    /// </param>
    /// <param name="items">
    ///   The items from which to form the list.
    /// </param>
    /// <param name="separator">
    ///   The string to separate items in the list.
    /// </param>
    /// <param name="delimiter">
    ///   The string to delimit each item in the list.
    /// </param>
    /// <returns>
    ///   The <paramref name="builder"/> instance.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///   <paramref name="builder"/> is <see langword="null"/>.
    /// </exception>
    public static StringBuilder AppendDelimitedList(
        this StringBuilder   builder,
        IEnumerable<string>? items,
        string?              separator = ", ",
        string?              delimiter = "'")
    {
        if (builder is null)
            throw new ArgumentNullException(nameof(builder));

        if (items is null)
            return builder;

        using var e = items.GetEnumerator();

        if (!e.MoveNext())
            return builder;

        for (;;)
        {
            builder.Append(delimiter);
            builder.Append(e.Current);
            builder.Append(delimiter);

            if (!e.MoveNext())
                return builder;

            builder.Append(separator);
        }
    }
}
