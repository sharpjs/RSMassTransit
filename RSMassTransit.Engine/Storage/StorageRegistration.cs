// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

using System.Diagnostics;
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
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

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

        return configuration.Type switch
        {
            StorageType.File      => new FileBlobStorage(configuration.File),
            StorageType.AzureBlob => new AzureBlobStorage(configuration.Azure),
            _                     => throw new UnreachableException()
        };
    }
}
