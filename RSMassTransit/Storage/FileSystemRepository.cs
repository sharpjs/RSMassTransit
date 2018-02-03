using System;
using System.IO;
using System.Threading.Tasks;

using static System.IO.Directory;
using static System.IO.Path;

namespace RSMassTransit.Storage
{
    internal class FileSystemRepository : IBlobRepository
    {
        private const int BufferSizeBytes = 1 * 1024 * 1024; // 1 MB

        private readonly string _basePath;

        public FileSystemRepository(string path)
        {
            _basePath = CreateDirectory(path).FullName;

            Log.Information("Using filesystem repository: {0}", _basePath);
        }

        public async Task<Uri> PutAsync(Stream stream)
        {
            // Compute paths
            var realPath   = Combine(_basePath, GenerateFileName());
            var tempPath   = ChangeExtension(realPath, ".upl");
            var parentPath = GetDirectoryName(realPath);

            Log.Information("Writing file: {0}", realPath);

            try
            {
                // Ensure directory exists to contain the file
                CreateDirectory(parentPath);

                // Use temp file to prevent access during write
                var file = new FileInfo(tempPath);

                // Write temp file
                using (var target = file.Open(FileMode.CreateNew, FileAccess.Write, FileShare.None))
                    await stream.CopyToAsync(target, BufferSizeBytes);

                // Rename fully-written temp file to final path
                // NOTE: throws if the final path exists
                file.MoveTo(realPath);

                // Convert to 'file:' URI
                return new Uri(realPath);
            }
            finally
            {
                // Make best effort to clean up, but do not allow an exception
                // thrown here to obscure any exception from the try block.
                try { File.Delete(tempPath); } catch {  }
            }
        }

        private static string GenerateFileName()
            => RandomFileNames.Next(separator: '\\', extension: ".dat");
    }
}
