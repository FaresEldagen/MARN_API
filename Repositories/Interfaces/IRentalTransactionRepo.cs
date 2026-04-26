using MARN_API.DTOs.Common;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IRentalTransactionRepo
    {
        Task AddAsync(RentalTransaction record);
        Task UpdateAsync(RentalTransaction record);
        Task<PagedResult<RentalTransaction>> GetAllAsync(int pageNumber, int pageSize);
        Task<PagedResult<RentalTransaction>> GetByUserIdAsync(Guid userId, int pageNumber, int pageSize);
        Task<PagedResult<RentalTransaction>> GetByPropertyIdAsync(long propertyId, int pageNumber, int pageSize);
        Task<RentalTransaction?> GetByIdAsync(Guid id);
        Task<RentalTransaction?> GetByPaymentRecordIdAsync(long paymentRecordId);
        Task<RentalTransaction?> GetBySessionIdAsync(string sessionId);
        Task<RentalTransaction?> GetByContractIdAsync(long contractId);
        Task<RentalTransaction?> GetActiveInitiatedSessionAsync(Guid renterId, long propertyId);
    }
}
