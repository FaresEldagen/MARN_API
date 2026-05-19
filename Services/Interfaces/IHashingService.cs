using System.IO;
using System.Threading.Tasks;

namespace MARN_API.Services.Interfaces
{
    public interface IHashingService
    {
        Task<string> ComputeSha256HashAsync(Stream fileStream);
    }
}
