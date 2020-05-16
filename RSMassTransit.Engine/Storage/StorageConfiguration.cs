/*
    Copyright (C) 2020 Jeffrey Sharp

    Permission to use, copy, modify, and distribute this software for any
    purpose with or without fee is hereby granted, provided that the above
    copyright notice and this permission notice appear in all copies.

    THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
    WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
    MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
    ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
    WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
    ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
    OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
*/

using Microsoft.Extensions.Configuration;
using RSMassTransit.Storage;
using Sharp.BlobStorage.Azure;
using Sharp.BlobStorage.File;

namespace RSMassTransit
{
    /// <summary>
    ///   Configuration for RSMassTransit report storage.
    /// </summary>
    public class StorageConfiguration : IStorageConfiguration
    {
        private const string
            DefaultFilePath                  = @"C:\Blobs",
            DefaultAzureBlobConnectionString = "UseDevelopmentStorage=true",
            DefaultAzureBlobContainerName    = "reports";

        private const StorageType
            DefaultStorageType = StorageType.File;

        /// <summary>
        ///   Gets or sets the type of report storage.
        /// </summary>
        public StorageType Type { get; set; } = DefaultStorageType;

        /// <inheritdoc/>
        public FileBlobStorageConfiguration File { get; } = new FileBlobStorageConfiguration
        {
            Path = DefaultFilePath
        };

        /// <inheritdoc/>
        public AzureBlobStorageConfiguration Azure { get; } = new AzureBlobStorageConfiguration
        {
            ConnectionString = DefaultAzureBlobConnectionString,
            ContainerName    = DefaultAzureBlobContainerName
        };

        /// <summary>
        ///   Initializes a new <see cref="StorageConfiguration"/> instance
        ///   with values from the specified configuration repository.
        /// </summary>
        /// <param name="configuration">
        ///   The configuration repository from which to load values.
        /// </param>
        public StorageConfiguration(IConfiguration configuration)
        {
            Load(configuration);
        }

        /// <summary>
        ///   Loads values from the specified configuration repository.
        /// </summary>
        /// <param name="configuration">
        ///   The configuration repository from which to load values.
        /// </param>
        public void Load(IConfiguration configuration)
        {
            Type = configuration.GetEnum<StorageType>(nameof(Type)) ?? StorageType.File;

            Load(File,  configuration.GetSection("File"     ));
            Load(Azure, configuration.GetSection("AzureBlob"));
        }

        private static void Load(FileBlobStorageConfiguration f, IConfiguration c)
        {
            f.Path = c.GetString(nameof(f.Path)) ?? DefaultFilePath;
        }

        private static void Load(AzureBlobStorageConfiguration a, IConfiguration c)
        {
            a.ConnectionString = c.GetString(nameof(a.ConnectionString)) ?? DefaultAzureBlobConnectionString;
            a.ContainerName    = c.GetString(nameof(a.ContainerName   )) ?? DefaultAzureBlobContainerName;
        }
    }
}
