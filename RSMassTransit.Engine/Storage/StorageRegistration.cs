// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sharp.BlobStorage;
using Sharp.BlobStorage.Azure;
using Sharp.BlobStorage.File;

namespace RSMassTransit.Storage;

internal static class StorageRegistration
{
    public static void AddBlobStorage(
        this IServiceCollection services,
        IConfiguration          configuration)
    {
        if (services is null)
            throw new ArgumentNullException(nameof(services));
        if (configuration is null)
            throw new ArgumentNullException(nameof(configuration));

        services.AddSingleton(LoadConfiguration(configuration));
        services.AddSingleton(CreateRepository);
    }

    private static IStorageConfiguration LoadConfiguration(IConfiguration configuration)
    {
        return new StorageConfiguration(configuration);
    }

    [ExcludeFromCodeCoverage]
    private static IBlobStorage CreateRepository(IServiceProvider services)
    {
        var configuration = services.GetRequiredService<IStorageConfiguration>();

        switch (configuration.Type)
        {
            case StorageType.File:
                return new FileBlobStorage(configuration.File);

            case StorageType.AzureBlob:
                return new AzureBlobStorage(configuration.Azure);

            default:
                // Should be unreachable
                throw new NotSupportedException();
        }
    }
}
