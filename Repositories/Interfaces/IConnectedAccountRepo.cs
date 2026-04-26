using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IConnectedAccountRepo
    {
        Task AddAsync(ConnectedAccount record);
        Task UpdateAsync(ConnectedAccount record);
        Task<ConnectedAccount?> GetByIdAsync(Guid id);
        Task<ConnectedAccount?> GetByApplicationUserIdAsync(Guid applicationUserId);
        Task<IEnumerable<ConnectedAccount>> GetAllAsync();
    }
}
