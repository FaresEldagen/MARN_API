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
            //var interval = TimeSpan.FromHours(1);

            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;
                var nextRun = now.Date.AddDays(1);
                var delay = nextRun - now;

                _logger.LogInformation("PaymentBackgroundService will run after {Delay}", delay);
                await Task.Delay(delay, stoppingToken);

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

                        foreach (var payment in batch)
                        {
                            if (payment.Status == PaymentStatus.OnHold &&
                                payment.AvailableAt.Date <= today)
                            {
                                payment.Status = PaymentStatus.Available;
                                await repo.UpdatePayment(payment);

                                await notificationService.SendNotificationAsync(new NotificationRequestDto
                                {
                                    UserId = payment.PaymentSchedule.Contract.Property.OwnerId.ToString(),
                                    UserType = NotificationUserType.Owner,
                                    Type = NotificationType.AvailableForWithdrawal,
                                    Title = "Payment Available for Withdrawal",
                                    Body = $"Your payment of {payment.OwnerAmount} {payment.PaymentSchedule.Currency} from contract \"{payment.PaymentSchedule.Contract.Property.Title}\"\n" +
                                           $"that paid at {payment.PaidAt:yyyy-MM-dd} is now available for withdrawal."
                                });
                            }
                        }

                        skip += BatchSize;

                    } while (batch.Count == BatchSize);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred in PaymentBackgroundService.");
                }

                //await Task.Delay(interval, stoppingToken);
            }
        }
    }
}
