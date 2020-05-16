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

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sharp.BlobStorage;
using Sharp.BlobStorage.Azure;
using Sharp.BlobStorage.File;

namespace RSMassTransit.Storage
{
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
}
