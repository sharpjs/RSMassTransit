// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

namespace RSMassTransit;

internal static class StringExtensions
{
    public static string? NullIfEmpty(this string? s)
        => s == "" ? null : s;
}
