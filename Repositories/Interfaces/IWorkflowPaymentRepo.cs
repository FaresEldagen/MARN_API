using MARN_API.DTOs.Common;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IWorkflowPaymentRepo
    {
        Task<Payment> CreatePaymentRecordAsync(Payment record);
        Task<Payment?> GetPaymentByIdAsync(long paymentId);
        Task<Payment?> GetPaymentBySessionIdAsync(string sessionId);
        Task<PagedResult<Payment>> GetPaymentsByUserIdAsync(Guid userId, int pageNumber, int pageSize);
        Task<PagedResult<Payment>> GetPaymentsByPropertyIdAsync(long propertyId, int pageNumber, int pageSize);
        Task<PagedResult<Payment>> GetAllPaymentsAsync(int pageNumber, int pageSize);
        Task UpdatePaymentRecordAsync(Payment record);
    }
}
