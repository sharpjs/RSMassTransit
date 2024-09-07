// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

namespace RSMassTransit.Storage;

/// <summary>
///   Types of storage supported by RSMassTransit.
/// </summary>
internal enum StorageType
{
    /// <summary>
    ///   Each rendered report is stored as a file in the file system.
    /// </summary>
    File,

    /// <summary>
    ///   Each rendered report is stored as a blob in an Azure storage
    ///   account.
    /// </summary>
    AzureBlob
}
