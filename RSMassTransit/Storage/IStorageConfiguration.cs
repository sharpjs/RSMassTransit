// Copyright (C) 2018 (to be determined)

namespace RSMassTransit.Storage
{
    internal interface IStorageConfiguration
    {
        StorageType StorageType                  { get; }
        string      FileSystemPath               { get; }
        string      AzureStorageConnectionString { get; }
        string      AzureStorageContainer        { get; }
    }
}
