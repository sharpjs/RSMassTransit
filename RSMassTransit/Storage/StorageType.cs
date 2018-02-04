// Copyright (C) 2018 (to be determined)

namespace RSMassTransit.Storage
{
    /// <summary>
    ///   Types of storage supported by RSMassTransit.
    /// </summary>
    internal enum StorageType
    {
        /// <summary>
        ///   A directory in the file system.
        /// </summary>
        FileSystem,

        /// <summary>
        ///   A blob container in an Azure Storage account.
        /// </summary>
        AzureStorageBlob
    }
}
