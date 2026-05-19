using MARN_API.Enums;
using MARN_API.Enums.Payment;
using MARN_API.Enums.Notification;
using MARN_API.DTOs.Notification;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using MARN_API.Models;

namespace MARN_API.BackgroundJobs
{
    public class PaymentJob
    {
        private readonly IPaymentRepo _paymentRepo;
        private readonly INotificationService _notificationService;
        private readonly ILogger<PaymentJob> _logger;

        public PaymentJob(
            IPaymentRepo paymentRepo,
            INotificationService notificationService,
            ILogger<PaymentJob> logger)
        {
            _paymentRepo = paymentRepo;
            _notificationService = notificationService;
            _logger = logger;
        }


        private const int BatchSize = 100;

        public async Task ExecuteAsync()
        {
            try
            {
                // Handling OnHold Payments status updates and notifications
                var skip = 0;
                List<Payment> batch;

                do
                {
                    batch = await _paymentRepo.GetOnHoldPayments(skip, BatchSize);
                    var today = DateTime.UtcNow.Date;
                    var updatedPayments = new List<Payment>();

                    foreach (var payment in batch)
                    {
                        Console.WriteLine("strating chekcing payment");
                        if (payment.Status == PaymentStatus.OnHold &&
                            payment.AvailableAt.Date <= today)
                        {
                            Console.WriteLine("changing payment status");
                            payment.Status = PaymentStatus.Available;
                            updatedPayments.Add(payment);

                            await _notificationService.SendNotificationAsync(new NotificationRequestDto
                            {
                                UserId = payment.PaymentSchedule.Contract.Property.OwnerId.ToString(),
                                UserType = NotificationUserType.Owner,
                                Type = NotificationType.AvailableForWithdrawal,
                                Title = "Payment Available for Withdrawal",
                                Body = $"Your payment of {payment.OwnerAmount} {payment.PaymentSchedule.Currency} from contract \"{payment.PaymentSchedule.Contract.Property.Title}\"\n"
                                     + $"that paid at {payment.PaidAt:yyyy-MM-dd} is now available for withdrawal.",

                                ActionType = NotificationActionType.OwnerDashboard,
                            });
                        }
                    }

                    if (updatedPayments.Any())
                        await _paymentRepo.UpdatePayments(updatedPayments);

                    // Since updated payments are removed from the 'OnHold' query results,
                    // we only advance 'skip' by the number of payments we didn't update.
                    skip += batch.Count - updatedPayments.Count;

                } while (batch.Count == BatchSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in PaymentJob.");
                throw;
            }
        }
    }
}
