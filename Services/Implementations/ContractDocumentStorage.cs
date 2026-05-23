using MARN_API.Services.Interfaces;
using MARN_API.Utilities;

namespace MARN_API.Services.Implementations
{
    public class ContractDocumentStorage : IContractDocumentStorage
    {
        private readonly IWebHostEnvironment _environment;

        public ContractDocumentStorage(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public Task<string> SaveContractPdfAsync(long contractId, byte[] fileBytes, CancellationToken cancellationToken = default)
        {
            return SaveAsync(ContractDocumentPathBuilder.BuildPdfRelativePath(contractId), fileBytes, cancellationToken);
        }

        public Task<string> SaveOtsProofAsync(long contractId, byte[] fileBytes, CancellationToken cancellationToken = default)
        {
            return SaveAsync(ContractDocumentPathBuilder.BuildOtsRelativePath(contractId), fileBytes, cancellationToken);
        }

        public async Task<byte[]?> ReadAsync(string? relativePath, CancellationToken cancellationToken = default)
        {
            if (!TryResolveAbsolutePath(relativePath, out var absolutePath))
            {
                return null;
            }

            if (!File.Exists(absolutePath))
            {
                return null;
            }

            return await File.ReadAllBytesAsync(absolutePath, cancellationToken);
        }

        public Task DeleteAsync(string? relativePath, CancellationToken cancellationToken = default)
        {
            if (!TryResolveAbsolutePath(relativePath, out var absolutePath))
            {
                return Task.CompletedTask;
            }

            if (File.Exists(absolutePath))
            {
                File.Delete(absolutePath);
            }

            var directoryPath = Path.GetDirectoryName(absolutePath);
            if (!string.IsNullOrWhiteSpace(directoryPath) &&
                Directory.Exists(directoryPath) &&
                !Directory.EnumerateFileSystemEntries(directoryPath).Any())
            {
                Directory.Delete(directoryPath);
            }

            return Task.CompletedTask;
        }

        private async Task<string> SaveAsync(string relativePath, byte[] fileBytes, CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(ContractDocumentPathBuilder.BuildContractsRootAbsolutePath(_environment.ContentRootPath));

            if (!TryResolveAbsolutePath(relativePath, out var absolutePath))
            {
                throw new InvalidOperationException("Could not resolve contract document storage path.");
            }

            var directoryPath = Path.GetDirectoryName(absolutePath);
            if (string.IsNullOrWhiteSpace(directoryPath))
            {
                throw new InvalidOperationException("Could not resolve the contract document directory.");
            }

            Directory.CreateDirectory(directoryPath);
            await File.WriteAllBytesAsync(absolutePath, fileBytes, cancellationToken);

            return relativePath;
        }

        private bool TryResolveAbsolutePath(string? relativePath, out string absolutePath)
        {
            absolutePath = string.Empty;
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                return false;
            }

            var contentRoot = EnsureTrailingSeparator(Path.GetFullPath(_environment.ContentRootPath));
            absolutePath = Path.GetFullPath(Path.Combine(contentRoot, relativePath.Replace('/', Path.DirectorySeparatorChar)));

            return absolutePath.StartsWith(contentRoot, StringComparison.OrdinalIgnoreCase);
        }

        private static string EnsureTrailingSeparator(string path)
        {
            return path.EndsWith(Path.DirectorySeparatorChar)
                ? path
                : path + Path.DirectorySeparatorChar;
        }
    }
}
