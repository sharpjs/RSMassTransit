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
using Microsoft.Extensions.DependencyInjection;
using Sharp.BlobStorage;
using Sharp.BlobStorage.Azure;
using Sharp.BlobStorage.File;

namespace RSMassTransit.Storage
{
    internal static class StorageExtensions
    {
        internal static void AddBlobStorage(this IServiceCollection services)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            services.AddSingleton(CreateRepository);
        }

        [ExcludeFromCodeCoverage]
        private static IBlobStorage CreateRepository(IServiceProvider context)
        {
            var configuration = context.GetRequiredService<IStorageConfiguration>();

            switch (configuration.StorageType)
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
