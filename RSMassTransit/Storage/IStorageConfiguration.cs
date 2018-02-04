namespace RSMassTransit.Storage
{
    internal interface IStorageConfiguration
    {
        StorageType StorageType                  { get; }
        string      FileSystemPath               { get; }
        string      AzureStorageContainer        { get; }
        string      AzureStorageConnectionString { get; }
    }
}
