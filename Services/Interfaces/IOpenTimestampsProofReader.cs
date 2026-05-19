using MARN_API.Services.Implementations;

namespace MARN_API.Services.Interfaces
{
    public interface IOpenTimestampsProofReader
    {
        OpenTimestampsProofReader.OpenTimestampsProofExtractionResult Extract(byte[] otsBytes, byte[]? originalFileBytes = null);
    }
}
