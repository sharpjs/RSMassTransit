// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

namespace RSMassTransit.Messages;

/// <summary>
///   Credential to authenticate with the report server.
/// </summary>
public interface ICredential
{
    /// <summary>
    ///   Username to authenticate with the report server.
    /// </summary>
    string? UserName { get; set; }

    /// <summary>
    ///   Password to authenticate with the report server.
    /// </summary>
    string? Password { get; set; }
}
