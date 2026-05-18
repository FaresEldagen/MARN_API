using MARN_API.Enums;
using MARN_API.Enums.Payment;
using MARN_API.Enums.Notification;
using MARN_API.DTOs.Notification;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using MARN_API.Models;

namespace MARN_API.Services.Implementations
{
    public class PaymentScheduleBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PaymentScheduleBackgroundService> _logger;

        public PaymentScheduleBackgroundService(IServiceProvider serviceProvider, ILogger<PaymentScheduleBackgroundService> logger)
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

                    // Handling Payment Schedules status updates and notifications
                    var skip = 0;
                    List<PaymentSchedule> batch;

                    do
                    {
                        batch = await repo.GetPendingPaymentSchedules(skip, BatchSize);
                        var today = DateTime.UtcNow.Date;
                        var updatedSchedules = new List<PaymentSchedule>();

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
                            {
                                updatedSchedules.Add(paymentSchedule);
                            }

                            // Notifications are sent based on the (potentially updated) status
                            if (paymentSchedule.Status == PaymentScheduleStatus.Available && statusChanged)
                            {
                                var daysLeft = (int)(paymentSchedule.DueDate.Date - today).TotalDays;
                                await notificationService.SendNotificationAsync(new NotificationRequestDto
                                {
                                    UserId = paymentSchedule.Contract.RenterId.ToString(),
                                    UserType = NotificationUserType.Renter,
                                    Type = NotificationType.UpcomingPayment,
                                    TitleKey = "NOTIFICATION_UPCOMING_PAYMENT_TITLE",
                                    BodyKey = "NOTIFICATION_UPCOMING_PAYMENT_BODY",
                                    LocalizationArguments = new()
                                    {
                                        paymentSchedule.Amount.ToString(),
                                        paymentSchedule.Currency,
                                        paymentSchedule.Contract.Property.Title,
                                        daysLeft.ToString(),
                                        paymentSchedule.DueDate.ToString("yyyy-MM-dd")
                                    },
                                    Title = "Upcoming Payment Available",
                                    Body = $"Your payment of {paymentSchedule.Amount} {paymentSchedule.Currency} for \"{paymentSchedule.Contract.Property.Title}\" is now available and can be paid.\n"
                                         + $"{daysLeft} day(s) left until the due date {paymentSchedule.DueDate:yyyy-MM-dd}."
                                });
                            }
                            else if (paymentSchedule.Status == PaymentScheduleStatus.DueToday && statusChanged)
                            {
                                await notificationService.SendNotificationAsync(new NotificationRequestDto
                                {
                                    UserId = paymentSchedule.Contract.RenterId.ToString(),
                                    UserType = NotificationUserType.Renter,
                                    Type = NotificationType.PaymentArrived,
                                    TitleKey = "NOTIFICATION_PAYMENT_DUE_TODAY_TITLE",
                                    BodyKey = "NOTIFICATION_PAYMENT_DUE_TODAY_BODY",
                                    LocalizationArguments = new()
                                    {
                                        paymentSchedule.Amount.ToString(),
                                        paymentSchedule.Currency,
                                        paymentSchedule.Contract.Property.Title
                                    },
                                    Title = "Payment Due Today",
                                    Body = $"Your payment of {paymentSchedule.Amount} {paymentSchedule.Currency} for \"{paymentSchedule.Contract.Property.Title}\" is due today."
                                });
                            }
                            else if (paymentSchedule.Status == PaymentScheduleStatus.Overdue && statusChanged)
                            {
                                var daysLate = (int)(today - paymentSchedule.DueDate.Date).TotalDays;
                                await notificationService.SendNotificationAsync(new NotificationRequestDto
                                {
                                    UserId = paymentSchedule.Contract.RenterId.ToString(),
                                    UserType = NotificationUserType.Renter,
                                    Type = NotificationType.DelayedPayment,
                                    TitleKey = "NOTIFICATION_PAYMENT_OVERDUE_TITLE",
                                    BodyKey = "NOTIFICATION_PAYMENT_OVERDUE_BODY",
                                    LocalizationArguments = new()
                                    {
                                        paymentSchedule.Amount.ToString(),
                                        paymentSchedule.Currency,
                                        paymentSchedule.Contract.Property.Title,
                                        daysLate.ToString()
                                    },
                                    Title = "Payment Overdue",
                                    Body = $"Your payment of {paymentSchedule.Amount} {paymentSchedule.Currency} for \"{paymentSchedule.Contract.Property.Title}\" is overdue.\n"
                                         + $"You are {daysLate} day(s) past the due date.\n\n"
                                         + "Please complete the payment as soon as possible to avoid any service interruption or further actions in accordance with our terms.\n"
                                         + "If you have any issues, please contact support."
                                });
                            }
                        }

                        if (updatedSchedules.Any())
                        {
                            await repo.UpdatePaymentSchedules(updatedSchedules);
                        }

                        skip += BatchSize;

                    } while (batch.Count == BatchSize);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("PaymentScheduleBackgroundService is stopping.");
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred in PaymentScheduleBackgroundService.");
                }

                // Schedule next run at midnight UTC
                var now = DateTime.UtcNow;
                var nextRun = now.Date.AddDays(1);
                var delay = nextRun - now;

                _logger.LogInformation("PaymentScheduleBackgroundService next run after {Delay}", delay);
                try
                {
                    await Task.Delay(delay, stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("PaymentScheduleBackgroundService is stopping.");
                    return;
                }
            }
        }
    }
}
