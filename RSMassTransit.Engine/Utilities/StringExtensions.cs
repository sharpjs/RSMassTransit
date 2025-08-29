// Copyright Subatomix Research Inc.
// SPDX-License-Identifier: MIT

namespace RSMassTransit;

internal static class StringExtensions
{
    public static string? NullIfEmpty(this string? s)
        => s == "" ? null : s;
}
