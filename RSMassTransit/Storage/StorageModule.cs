using System;
using Autofac;

namespace RSMassTransit.Storage
{
    internal class StorageModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .Register(CreateRepository)
                .SingleInstance()
                .As<IBlobRepository>();
        }

        private IBlobRepository CreateRepository(IComponentContext context)
        {
            var configuration = context.Resolve<IStorageConfiguration>();

            switch (configuration.StorageType)
            {
                case StorageType.FileSystem:
                    return new FileSystemRepository(
                        configuration.FileSystemPath
                    );

                case StorageType.AzureStorageBlob:
                    return new AzureStorageBlobRepository(
                        configuration.AzureStorageConnectionString,
                        configuration.AzureStorageContainer
                    );

                default:
                    // Should be unreachable
                    throw new NotSupportedException();
            }
        }
    }
}
