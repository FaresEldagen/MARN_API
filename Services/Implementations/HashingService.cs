using System.Security.Cryptography;

using MARN_API.Services.Interfaces;

namespace MARN_API.Services.Implementations
{
    public class HashingService : IHashingService
    {
        public async Task<string> ComputeSha256HashAsync(Stream fileStream)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = await sha256.ComputeHashAsync(fileStream);
            return Convert.ToHexString(hashBytes).ToLowerInvariant();
        }
    }
}
