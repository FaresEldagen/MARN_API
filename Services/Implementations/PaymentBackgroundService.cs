using MARN_API.Enums;
using MARN_API.Enums.Payment;
using MARN_API.Enums.Notification;
using MARN_API.DTOs.Notification;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using MARN_API.Models;

namespace MARN_API.Services.Implementations
{
    public class PaymentBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PaymentBackgroundService> _logger;

        public PaymentBackgroundService(IServiceProvider serviceProvider, ILogger<PaymentBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }


        private const int BatchSize = 100;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var repo = scope.ServiceProvider.GetRequiredService<IPaymentRepo>();
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                    // Handling OnHold Payments status updates and notifications
                    var skip = 0;
                    List<Payment> batch;

                    do
                    {
                        batch = await repo.GetOnHoldPayments(skip, BatchSize);
                        var today = DateTime.UtcNow.Date;
                        var updatedPayments = new List<Payment>();

                        foreach (var payment in batch)
                        {
                            if (payment.Status == PaymentStatus.OnHold &&
                                payment.AvailableAt.Date <= today)
                            {
                                payment.Status = PaymentStatus.Available;
                                updatedPayments.Add(payment);

                                await notificationService.SendNotificationAsync(new NotificationRequestDto
                                {
                                    UserId = payment.PaymentSchedule.Contract.Property.OwnerId.ToString(),
                                    UserType = NotificationUserType.Owner,
                                    Type = NotificationType.AvailableForWithdrawal,
                                    TitleKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE",
                                    BodyKey = "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY",
                                    LocalizationArguments = new()
                                    {
                                        payment.OwnerAmount.ToString(),
                                        payment.PaymentSchedule.Currency,
                                        payment.PaymentSchedule.Contract.Property.Title,
                                        payment.PaidAt.ToString("yyyy-MM-dd")
                                    },
                                    Title = "Payment Available for Withdrawal",
                                    Body = $"Your payment of {payment.OwnerAmount} {payment.PaymentSchedule.Currency} from contract \"{payment.PaymentSchedule.Contract.Property.Title}\"\n" +
                                           $"that paid at {payment.PaidAt:yyyy-MM-dd} is now available for withdrawal."
                                });
                            }
                        }

                        if (updatedPayments.Any())
                            await repo.UpdatePayments(updatedPayments);

                        skip += BatchSize;

                    } while (batch.Count == BatchSize);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("PaymentBackgroundService is stopping.");
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred in PaymentBackgroundService.");
                }

                // Schedule next run at midnight UTC
                var now = DateTime.UtcNow;
                var nextRun = now.Date.AddDays(1);
                var delay = nextRun - now;

                _logger.LogInformation("PaymentBackgroundService next run after {Delay}", delay);
                try
                {
                    await Task.Delay(delay, stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("PaymentBackgroundService is stopping.");
                    return;
                }
            }
        }
    }
}
