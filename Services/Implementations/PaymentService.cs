using AutoMapper;
using Google.Api.Gax;
using MARN_API.DTOs.Contracts;
using MARN_API.Enums;
using MARN_API.Enums.Payment;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MARN_API.Enums.Notification;
using MARN_API.DTOs.Notification;
using Stripe;


namespace MARN_API.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepo _paymentRepo;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly ILogger<PaymentService> _logger;
        private readonly IConfiguration _configuration;

        public PaymentService(
            IPaymentRepo paymentRepo,
            IMapper mapper,
            INotificationService notificationService,
            ILogger<PaymentService> logger,
            IConfiguration configuration)

        {
            _paymentRepo = paymentRepo;
            _mapper = mapper;
            _notificationService = notificationService;
            _logger = logger;
            _configuration = configuration;
        }


        public async Task<ServiceResult<string>> CreatePaymentIntent(Guid userId, long paymentScheduleId)
        {
            _logger.LogInformation("Create Payment Intent attempt for userId: {userId}, paymentScheduleId: {paymentScheduleId}", userId, paymentScheduleId);

            var paymentSchedule = await _paymentRepo.GetPaymentScheduleById(paymentScheduleId);
            if (paymentSchedule == null)
            {
                _logger.LogWarning("Create Payment Intent failed: Payment schedule not found for paymentScheduleId: {paymentScheduleId}", paymentScheduleId);
                return ServiceResult<string>.Fail("Payment schedule not found.", resultType: ServiceResultType.NotFound);
            }

            if (paymentSchedule.Status == PaymentScheduleStatus.NotAvailableYet)
            {
                _logger.LogWarning("Create Payment Intent failed: Payment can only be made within 7 days of due date for paymentScheduleId: {paymentScheduleId}", paymentScheduleId);
                return ServiceResult<string>.Fail("Payment can only be made within 7 days of the due date.", resultType: ServiceResultType.BadRequest);
            }

            if (paymentSchedule.Status == PaymentScheduleStatus.PaidLate ||
                paymentSchedule.Status == PaymentScheduleStatus.PaidOnTime ||
                paymentSchedule.Status == PaymentScheduleStatus.PaidEarly)
            {
                _logger.LogWarning("Create Payment Intent failed: Payment already completed for paymentScheduleId: {paymentScheduleId}", paymentScheduleId);
                return ServiceResult<string>.Fail("Payment has already been done", resultType: ServiceResultType.BadRequest);
            }

            if (paymentSchedule.Contract.RenterId != userId)
            {
                _logger.LogWarning("Create Payment Intent failed: Unauthorized access for userId: {userId}, paymentScheduleId: {paymentScheduleId}", userId, paymentScheduleId);
                return ServiceResult<string>.Fail("Unauthorized access to payment schedule.", resultType: ServiceResultType.Unauthorized);
            }

            // To prevent duplicate PaymentIntents for the same schedule
            if (!string.IsNullOrEmpty(paymentSchedule.PaymentIntentId))
            {
                var service = new PaymentIntentService();

                var existingIntent = await service.GetAsync(paymentSchedule.PaymentIntentId);

                _logger.LogInformation("Create Payment Intent successful (existing intent) for paymentScheduleId: {paymentScheduleId}", paymentScheduleId);
                return ServiceResult<string>.Ok(
                    existingIntent.ClientSecret,
                    "Existing ClientSecret returned.",
                    ServiceResultType.Success
                );
            }

            try
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)(paymentSchedule.Amount * 100),
                    Currency = paymentSchedule.Currency,
                    Metadata = new Dictionary<string, string>
                    {
                        { "paymentScheduleId", paymentScheduleId.ToString() }
                    }
                };

                // To prevent duplicate PaymentIntents for the same schedule on stripe
                var requestOptions = new RequestOptions
                {
                    IdempotencyKey = $"pi_{paymentScheduleId}"
                };

                var service = new PaymentIntentService();
                var intent = await service.CreateAsync(options, requestOptions);

                paymentSchedule.PaymentIntentId = intent.Id;
                await _paymentRepo.UpdatePaymentSchedule(paymentSchedule);

                _logger.LogInformation("Create Payment Intent successful for paymentScheduleId: {paymentScheduleId}", paymentScheduleId);
                return ServiceResult<string>.Ok(intent.ClientSecret, "ClientSecret created successfully.", ServiceResultType.Success);
            }
            catch (StripeException e)
            {
                _logger.LogError(e, "Stripe API Error while creating PaymentIntent for ScheduleId: {ScheduleId}", paymentScheduleId);
                return ServiceResult<string>.Fail(e.StripeError?.Message ?? "Payment provider error.", resultType: ServiceResultType.BadRequest);
            }
        }

        public async Task HandleSuccessfulPayment(PaymentIntent paymentIntent)
        {
            _logger.LogInformation("Handle Successful Payment attempt for PaymentIntentId: {PaymentIntentId}", paymentIntent.Id);

            if (await _paymentRepo.PaymentExistsByIntentId(paymentIntent.Id))
            {
                _logger.LogWarning("Handle Successful Payment: PaymentIntent {PaymentIntentId} already processed. Skipping.", paymentIntent.Id);
                return;
            }

            var scheduleIdString = paymentIntent.Metadata["paymentScheduleId"];
            _logger.LogInformation("Handling successful payment for PaymentScheduleId: {PaymentScheduleId}", scheduleIdString);

            var paymentSchedule = await _paymentRepo.GetPaymentScheduleById(long.Parse(scheduleIdString));

            if (paymentSchedule == null)
            {
                _logger.LogError("Handle Successful Payment failed: Payment schedule not found for Id: {paymentScheduleId}", scheduleIdString);
                return;
            }

            var platformFeePercentage = _configuration.GetValue<decimal>("Stripe:PlatformFeePercentage", 0.1m);
            var fundHoldDays = _configuration.GetValue<int>("Stripe:FundHoldDays", 10);

            Payment payment = new Payment
            {
                PaymentScheduleId = paymentSchedule.Id,
                AmountTotal = paymentSchedule.Amount,
                PlatformFee = paymentSchedule.Amount * platformFeePercentage,
                OwnerAmount = paymentSchedule.Amount * (1 - platformFeePercentage),
                Currency = paymentSchedule.Currency,
                PaymentIntentId = paymentIntent.Id,
                PaidAt = DateTime.UtcNow,
                AvailableAt = DateTime.UtcNow.AddDays(fundHoldDays),
            };

            var today = DateTime.UtcNow.Date;
            var dueDate = paymentSchedule.DueDate.Date;

            if (today < dueDate)
                paymentSchedule.Status = PaymentScheduleStatus.PaidEarly;
            else if (today > dueDate)
                paymentSchedule.Status = PaymentScheduleStatus.PaidLate;
            else
                paymentSchedule.Status = PaymentScheduleStatus.PaidOnTime;

            await _paymentRepo.AddPayment(payment, paymentSchedule);

            _logger.LogInformation("Handle Successful Payment successful for paymentScheduleId: {paymentScheduleId}", scheduleIdString);

            await _notificationService.SendNotificationAsync(new NotificationRequestDto
            {
                UserId = paymentSchedule.Contract.Property.OwnerId.ToString(),
                UserType = NotificationUserType.Owner,
                Type = NotificationType.PaymentReceived,
                Title = "Payment Received",
                Body = $"You have received a payment of {payment.OwnerAmount} {payment.Currency} for \"{paymentSchedule.Contract.Property.Title}\".\n" +
                       $"This payment is for the due date {paymentSchedule.DueDate:yyyy-MM-dd}.\n\n" +
                       $"You can withdraw this amount after {payment.AvailableAt.ToString("yyyy-MM-dd")}."
            });

            await _notificationService.SendNotificationAsync(new NotificationRequestDto
            {
                UserId = paymentSchedule.Contract.RenterId.ToString(),
                UserType = NotificationUserType.Renter,
                Type = NotificationType.PaymentSuccessful,
                Title = "Payment Successful",
                Body = $"Your payment of {payment.AmountTotal} {payment.Currency} for \"{paymentSchedule.Contract.Property.Title}\" has been successful.\n" +
                       $"This payment is for the due date {paymentSchedule.DueDate:yyyy-MM-dd}."
            });
        }

        public async Task HandleFailedPayment(PaymentIntent paymentIntent)
        {
            _logger.LogInformation("Handle Failed Payment attempt for PaymentIntentId: {PaymentIntentId}", paymentIntent.Id);

            var scheduleIdString = paymentIntent.Metadata["paymentScheduleId"];
            var errorMessage = paymentIntent.LastPaymentError?.Message ?? "Unknown payment error";
            
            var paymentSchedule = await _paymentRepo.GetPaymentScheduleById(long.Parse(scheduleIdString));
            if (paymentSchedule != null)
            {
                await _notificationService.SendNotificationAsync(new NotificationRequestDto
                {
                    UserId = paymentSchedule.Contract.RenterId.ToString(),
                    UserType = NotificationUserType.Renter,
                    Type = NotificationType.PaymentFailed,
                    Title = "Payment Failed",
                    Body = $"Your payment for \"{paymentSchedule.Contract.Property.Title}\" has failed. \n Please try again."
                });

                _logger.LogInformation("Handle Failed Payment processed for paymentScheduleId: {paymentScheduleId}. Error: {Error}", scheduleIdString, errorMessage);
            }
            else
            {
                _logger.LogError("Handle Failed Payment failed: Payment schedule not found for Id: {paymentScheduleId}", scheduleIdString);
            }
        }
    }
}
