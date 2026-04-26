using System.Security.Cryptography;

namespace MARN_API.Services.Implementations
{
    public class HashingService
    {
        public async Task<string> ComputeSha256HashAsync(Stream fileStream)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = await sha256.ComputeHashAsync(fileStream);
            return Convert.ToHexString(hashBytes).ToLowerInvariant();
        }
    }
}
