// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

using System.Management.Automation;
using System.Net;

namespace RSMassTransit.PowerShell;

internal static class PSCredentialExtensions
{
    public static NetworkCredential? GetNetworkCredentialSafe(this PSCredential? credential)
        => credential == PSCredential.Empty
            ? null // avoid exception
            : credential?.GetNetworkCredential();
}
