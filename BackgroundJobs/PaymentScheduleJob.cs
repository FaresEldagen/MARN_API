using MARN_API.Enums;
using MARN_API.Enums.Payment;
using MARN_API.Enums.Notification;
using MARN_API.DTOs.Notification;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using MARN_API.Models;

namespace MARN_API.BackgroundJobs
{
    public class PaymentScheduleJob
    {
        private readonly IPaymentRepo _paymentRepo;
        private readonly INotificationService _notificationService;
        private readonly ILogger<PaymentScheduleJob> _logger;

        public PaymentScheduleJob(
            IPaymentRepo paymentRepo,
            INotificationService notificationService,
            ILogger<PaymentScheduleJob> logger)
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
                // Handling Payment Schedules status updates and notifications
                var skip = 0;
                List<PaymentSchedule> batch;

                do
                {
                    batch = await _paymentRepo.GetPendingPaymentSchedules(skip, BatchSize);
                    var today = DateTime.UtcNow.Date;
                    var updatedSchedules = new List<PaymentSchedule>();

                    foreach (var paymentSchedule in batch)
                    {
                        Console.WriteLine("strating chekcing paymentSchedule");

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
                        if (paymentSchedule.Status == PaymentScheduleStatus.Available)
                        {
                            var daysLeft = (int)(paymentSchedule.DueDate.Date - today).TotalDays;
                            await _notificationService.SendNotificationAsync(new NotificationRequestDto
                            {
                                UserId = paymentSchedule.Contract.RenterId.ToString(),
                                UserType = NotificationUserType.Renter,
                                Type = NotificationType.UpcomingPayment,
                                Title = "Upcoming Payment Available",
                                Body = $"Your payment of {paymentSchedule.Amount} {paymentSchedule.Currency} for \"{paymentSchedule.Contract.Property.Title}\" is now available and can be paid.\n"
                                     + $"{daysLeft} day(s) left until the due date {paymentSchedule.DueDate:yyyy-MM-dd}.",

                                ActionType = NotificationActionType.Payment,
                                ActionId = paymentSchedule.Id.ToString(),
                            });
                        }
                        else if (paymentSchedule.Status == PaymentScheduleStatus.DueToday)
                        {
                            await _notificationService.SendNotificationAsync(new NotificationRequestDto
                            {
                                UserId = paymentSchedule.Contract.RenterId.ToString(),
                                UserType = NotificationUserType.Renter,
                                Type = NotificationType.PaymentArrived,
                                Title = "Payment Due Today",
                                Body = $"Your payment of {paymentSchedule.Amount} {paymentSchedule.Currency} for \"{paymentSchedule.Contract.Property.Title}\" is due today.",

                                ActionType = NotificationActionType.Payment,
                                ActionId = paymentSchedule.Id.ToString(),
                            });
                        }
                        else if (paymentSchedule.Status == PaymentScheduleStatus.Overdue)
                        {
                            var daysLate = (int)(today - paymentSchedule.DueDate.Date).TotalDays;
                            await _notificationService.SendNotificationAsync(new NotificationRequestDto
                            {
                                UserId = paymentSchedule.Contract.RenterId.ToString(),
                                UserType = NotificationUserType.Renter,
                                Type = NotificationType.DelayedPayment,
                                Title = "Payment Overdue",
                                Body = $"Your payment of {paymentSchedule.Amount} {paymentSchedule.Currency} for \"{paymentSchedule.Contract.Property.Title}\" is overdue.\n"
                                     + $"You are {daysLate} day(s) past the due date.\n\n"
                                     + "Please complete the payment as soon as possible to avoid any service interruption or further actions in accordance with our terms.\n"
                                     + "If you have any issues, please contact support.",
                                
                                ActionType = NotificationActionType.Payment,
                                ActionId = paymentSchedule.Id.ToString(),
                            });
                        }
                    }

                    if (updatedSchedules.Any())
                    {
                        Console.WriteLine("changing paymentSchedule status");
                        await _paymentRepo.UpdatePaymentSchedules(updatedSchedules);
                    }

                    skip += BatchSize;

                } while (batch.Count == BatchSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in PaymentScheduleJob.");
                throw;
            }
        }
    }
}
