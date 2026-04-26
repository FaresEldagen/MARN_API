using MARN_API.DTOs.Common;
using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<(string Url, string SessionId)> CreateCheckoutSessionAsync(decimal amount, long propertyId, Guid ownerId, string ownerAccountId, Guid renterId, string? renterEmail, string successUrl, string cancelUrl, DateTime dueDate);
        Task<Payment?> GetPaymentByIdAsync(long paymentId);
        Task<Payment?> GetPaymentBySessionIdAsync(string sessionId);
        Task<PagedResult<Payment>> GetPaymentsByUserIdAsync(Guid userId, int pageNumber, int pageSize);
        Task<PagedResult<Payment>> GetPaymentsByPropertyIdAsync(long propertyId, int pageNumber, int pageSize);
        Task<bool> FulfillPaymentAsync(string sessionId);
        Task<bool> ExpirePaymentAsync(string sessionId);
        Task<PagedResult<Payment>> GetAllPaymentsAsync(int pageNumber, int pageSize);
    }
}
