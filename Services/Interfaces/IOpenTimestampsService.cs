using System.Threading.Tasks;

namespace MARN_API.Services.Interfaces
{
    public interface IOpenTimestampsService
    {
        Task<byte[]> SubmitHashAsync(string hashHex);
        Task<byte[]?> UpgradeOtsAsync(byte[] pendingOtsBytes);
        byte[] BuildDetachedOtsFile(string hashHex, byte[] serverAttestation);
        string CalculateCommitmentHash(byte[] otsFile);
        byte[] MergeOtsUpgrade(byte[] pendingOtsBytes, byte[] upgradeAttestation);
    }
}
