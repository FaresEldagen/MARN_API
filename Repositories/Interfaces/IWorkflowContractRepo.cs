using MARN_API.DTOs.Common;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IWorkflowContractRepo
    {
        Task<PagedResult<Contract>> GetAllAsync(int pageNumber, int pageSize);
        Task<Contract?> GetByIdAsync(long contractId);
        Task<PagedResult<Contract>> GetByUserIdAsync(Guid userId, int pageNumber, int pageSize);
        Task<PagedResult<Contract>> GetByPropertyIdAsync(long propertyId, int pageNumber, int pageSize);
        Task<IEnumerable<Contract>> GetPendingContractsAsync();
        Task AddAsync(Contract contract);
        Task UpdateAsync(Contract contract);
    }
}
