namespace MARN_API.Services.Interfaces
{
    public interface IContractDocumentStorage
    {
        Task<string> SaveContractPdfAsync(long contractId, byte[] fileBytes, CancellationToken cancellationToken = default);
        Task<string> SaveOtsProofAsync(long contractId, byte[] fileBytes, CancellationToken cancellationToken = default);
        Task<byte[]?> ReadAsync(string? relativePath, CancellationToken cancellationToken = default);
        Task DeleteAsync(string? relativePath, CancellationToken cancellationToken = default);
    }
}
