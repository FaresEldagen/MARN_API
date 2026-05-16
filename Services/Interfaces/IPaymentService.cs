using MARN_API.Models;
using Stripe;
using System.Security.Cryptography.Xml;

namespace MARN_API.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<ServiceResult<string>> CreatePaymentIntent(Guid userId, long paymentScheduleId);
        Task<ServiceResult<string>> CreateOrGetConnectOnboardingLink(Guid userId);
        Task<ServiceResult<bool>> Withdraw(Guid ownerId);


        #region Stripe Webhook Handlers
        Task HandleSuccessfulPayment(PaymentIntent paymentIntent);
        Task HandleFailedPayment(PaymentIntent paymentIntent);

        Task HandleConnectedAccountUpdated(Account account);

        Task HandleTransferCreated(Transfer transfer);
        Task HandleTransferReversed(Transfer transfer);
        #endregion
    }
}
