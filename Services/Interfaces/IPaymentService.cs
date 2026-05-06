using MARN_API.Models;
using Stripe;

namespace MARN_API.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<ServiceResult<string>> CreatePaymentIntent(Guid userId, long paymentScheduleId);
        Task HandleSuccessfulPayment(PaymentIntent paymentIntent);
        Task HandleFailedPayment(PaymentIntent paymentIntent);
    }
}
