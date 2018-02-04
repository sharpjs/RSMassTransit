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
