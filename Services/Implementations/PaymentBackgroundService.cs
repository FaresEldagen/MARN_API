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
            var interval = TimeSpan.FromHours(1);

            while (!stoppingToken.IsCancellationRequested)
            {
                //var now = DateTime.UtcNow;
                //var nextRun = now.Date.AddDays(1);
                //var delay = nextRun - now;

                //_logger.LogInformation("PaymentBackgroundService will run after {Delay}", delay);
                //await Task.Delay(delay, stoppingToken);

                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var repo = scope.ServiceProvider.GetRequiredService<IPaymentRepo>();
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                    var skip = 0;
                    List<PaymentSchedule> batch;

                    do
                    {
                        batch = await repo.GetPendingPaymentSchedules(skip, BatchSize);
                        var today = DateTime.UtcNow.Date;

                        foreach (var paymentSchedule in batch)
                        {
                            var statusChanged = false;

                            if (paymentSchedule.Status == PaymentScheduleStatus.NotAvailableYet &&
                                paymentSchedule.DueDate.Date - today <= TimeSpan.FromDays(7))
                            {
                                paymentSchedule.Status = PaymentScheduleStatus.Available;
                                statusChanged = true;
                            }

                            if ((paymentSchedule.Status == PaymentScheduleStatus.NotAvailableYet ||
                                paymentSchedule.Status == PaymentScheduleStatus.Available) &&
                                paymentSchedule.DueDate.Date == today)
                            {
                                paymentSchedule.Status = PaymentScheduleStatus.DueToday;
                                statusChanged = true;
                            }

                            if ((paymentSchedule.Status == PaymentScheduleStatus.NotAvailableYet ||
                                paymentSchedule.Status == PaymentScheduleStatus.Available ||
                                paymentSchedule.Status == PaymentScheduleStatus.DueToday) &&
                                paymentSchedule.DueDate.Date < today)
                            {
                                paymentSchedule.Status = PaymentScheduleStatus.Overdue;
                                statusChanged = true;
                            }

                            if (statusChanged)
                                await repo.UpdatePaymentSchedule(paymentSchedule);

                            if (paymentSchedule.Status == PaymentScheduleStatus.Available)
                            {
                                var daysLeft = (int)(paymentSchedule.DueDate.Date - today).TotalDays;
                                await notificationService.SendNotificationAsync(new NotificationRequestDto
                                {
                                    UserId = paymentSchedule.Contract.RenterId.ToString(),
                                    UserType = NotificationUserType.Renter,
                                    Type = NotificationType.UpcomingPayment,
                                    Title = "Upcoming Payment Available",
                                    Body = $"Your payment of {paymentSchedule.Amount} {paymentSchedule.Currency} for \"{paymentSchedule.Contract.Property.Title}\" is now available and can be paid.\n"
                                         + $"{daysLeft} day(s) left until the due date {paymentSchedule.DueDate:yyyy-MM-dd}."
                                });
                            }
                            else if (paymentSchedule.Status == PaymentScheduleStatus.DueToday)
                            {
                                await notificationService.SendNotificationAsync(new NotificationRequestDto
                                {
                                    UserId = paymentSchedule.Contract.RenterId.ToString(),
                                    UserType = NotificationUserType.Renter,
                                    Type = NotificationType.PaymentArrived,
                                    Title = "Payment Due Today",
                                    Body = $"Your payment of {paymentSchedule.Amount} {paymentSchedule.Currency} for \"{paymentSchedule.Contract.Property.Title}\" is due today."
                                });
                            }
                            else if (paymentSchedule.Status == PaymentScheduleStatus.Overdue)
                            {
                                var daysLate = (int)(today - paymentSchedule.DueDate.Date).TotalDays;
                                await notificationService.SendNotificationAsync(new NotificationRequestDto
                                {
                                    UserId = paymentSchedule.Contract.RenterId.ToString(),
                                    UserType = NotificationUserType.Renter,
                                    Type = NotificationType.DelayedPayment,
                                    Title = "Payment Overdue",
                                    Body = $"Your payment of {paymentSchedule.Amount} {paymentSchedule.Currency} for \"{paymentSchedule.Contract.Property.Title}\" is overdue.\n"
                                         + $"You are {daysLate} day(s) past the due date.\n\n"
                                         + "Please complete the payment as soon as possible to avoid any service interruption or further actions in accordance with our terms.\n"
                                         + "If you have any issues, please contact support."
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

                await Task.Delay(interval, stoppingToken);
            }
        }
    }
}
