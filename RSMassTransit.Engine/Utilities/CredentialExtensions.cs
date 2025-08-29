// Copyright Subatomix Research Inc.
// SPDX-License-Identifier: MIT

using System.Net;
using RSMassTransit.Messages;

namespace RSMassTransit;

internal static class CredentialExtensions
{
    public static NetworkCredential GetNetworkCredential(this ICredential credential)
        => new(credential.UserName, credential.Password);
}
