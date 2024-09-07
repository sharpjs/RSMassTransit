// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

using Sharp.BlobStorage.Azure;
using Sharp.BlobStorage.File;

namespace RSMassTransit.Storage;

/// <summary>
///   Configuration for report storage.
/// </summary>
internal interface IStorageConfiguration
{
    /// <summary>
    ///   Gets the type of report storage.
    /// </summary>
    StorageType Type { get; }

    /// <summary>
    ///   Gets the configuration for report storage in files.
    /// </summary>
    FileBlobStorageConfiguration File { get; }

    /// <summary>
    ///   Gets the configuration for report storage in an Azure Blob
    ///   container.
    /// </summary>
    AzureBlobStorageConfiguration Azure { get; }
}
