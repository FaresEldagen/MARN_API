using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<ServiceResult<string>> CreatePaymentIntent(Guid userId, long paymentScheduleId);
        Task Webhook();
    }
}
