using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace RSMassTransit.Storage
{
    internal class AzureStorageBlobRepository : IBlobRepository
    {
        private readonly CloudStorageAccount _account;
        private readonly CloudBlobClient     _client;
        private readonly CloudBlobContainer  _container;

        public AzureStorageBlobRepository(string connectionString, string containerName)
        {
            _account = CloudStorageAccount.Parse(connectionString);

            _client = _account.CreateCloudBlobClient();
            _client.DefaultRequestOptions = RequestOptions;

            _container = _client.GetContainerReference(containerName);

            Log.Information("Using Azure Storage blob repository: {0}", _container.Uri);
        }

        public Task InitializeAsync()
        {
            Log.Information("Ensuring blob container '{0}' exists.", _container.Name);

            return _container.CreateIfNotExistsAsync();
        }

        public async Task<Uri> PutAsync(Stream stream)
        {
            var name = GenerateFileName();

            Log.Information("Uploading blob: {0}", name);

            var blob = _container.GetBlockBlobReference(name);
            blob.StreamWriteSizeInBytes = UploadBlockSizeInBytes;

            await blob.UploadFromStreamAsync(
                stream,
                AccessCondition.GenerateIfNotExistsCondition(),
                options: null,
                operationContext: null
            );

            return new Uri("");
        }

        private static string GenerateFileName()
            => RandomFileNames.Next(separator: '/', extension: ".dat");

        private const int
            UploadBlockSizeInBytes = 1 * 1024 * 1024; // block size in multi-block upload (see below)

        private static readonly BlobRequestOptions
            RequestOptions = new BlobRequestOptions
            {
                // Official documentation:
                // https://docs.microsoft.com/en-us/dotnet/api/microsoft.windowsazure.storage.blob.blobrequestoptions?view=azure-dotnet
                //
                // Better documentation:
                // https://www.red-gate.com/simple-talk/cloud/platform-as-a-service/azure-blob-storage-part-4-uploading-large-blobs/

                // Minimum upload speeds to prevent timeout:
                // * 68 KB/s for blobs <  2MB
                // * 34 KB/s for blobs >= 2MB

                // Splitting
                ParallelOperationThreadCount     = 1,               // concurrent uploads
                SingleBlobUploadThresholdInBytes = 2 * 1024 * 1024, // threshold for multi-block upload
                //blob.StreamWriteSizeInBytes    = 1 * 1024 * 1024, // block size in multi-block upload

                // Timeouts
                ServerTimeout        = TimeSpan.FromSeconds(30),    // individual request timeout
                MaximumExecutionTime = null,                        // overall operation timeout

                // Retries
                RetryPolicy = new ExponentialRetry(
                    deltaBackoff: TimeSpan.FromSeconds(5),
                    maxAttempts:  3
                ),

                // Hashing
                StoreBlobContentMD5  = true,    // calculate and store hash of uploaded blob
                UseTransactionalMD5  = false,   // do not validate hash of every request; HTTPS does this

                // Encryption
                RequireEncryption    = false,   // \__ no client-side
                EncryptionPolicy     = null,    // /     encryption

                // Defaults taken for other properties:
                // * AbsorbConditionalErrorsOnRetry (not applicable)
                // * DisableContentMD5Validation    (not applicable)
                // * LocationMode                   (not applicable)
            };
    }
}
