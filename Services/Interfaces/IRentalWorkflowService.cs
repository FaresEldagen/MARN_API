using MARN_API.DTOs.Common;
using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface IRentalWorkflowService
    {
        Task<PagedResult<RentalTransaction>> GetAllTransactionsAsync(int pageNumber, int pageSize);
        Task<PagedResult<RentalTransaction>> GetTransactionsByUserIdAsync(Guid userId, int pageNumber, int pageSize);
        Task<PagedResult<RentalTransaction>> GetTransactionsByPropertyIdAsync(long propertyId, int pageNumber, int pageSize);
        Task<RentalTransaction?> GetTransactionByIdAsync(Guid id);
        Task<RentalTransaction?> GetTransactionByPaymentRecordIdAsync(long paymentRecordId);
        Task<RentalTransaction?> GetTransactionByContractIdAsync(long contractId);
        Task<string> StartCheckoutAsync(long propertyId, Guid renterId, string successUrl, string cancelUrl);
        Task<bool> CompleteLeaseFulfillmentAsync(string stripeSessionId, string? paymentIntentId, string? receiptUrl);
        Task<bool> ExpireLeaseFulfillmentAsync(string stripeSessionId);
    }
}
