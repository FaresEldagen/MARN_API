using AutoMapper;
using MARN_API.DTOs.Contracts;
using MARN_API.Enums;
using MARN_API.Enums.Payment;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Stripe;


namespace MARN_API.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepo _paymentRepo;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly ILogger<PaymentService> _logger;


        public PaymentService(
            IPaymentRepo paymentRepo,
            IMapper mapper,
            INotificationService notificationService,
            ILogger<PaymentService> logger)

        {
            _paymentRepo = paymentRepo;
            _mapper = mapper;
            _notificationService = notificationService;
            _logger = logger;
        }


        public async Task<ServiceResult<string>> CreatePaymentIntent(Guid userId, long paymentScheduleId)
        {
            var paymentSchedule = await _paymentRepo.GetPaymentScheduleById(paymentScheduleId);
            if (paymentSchedule == null)
            {
                return ServiceResult<string>.Fail("Payment schedule not found.", resultType: ServiceResultType.NotFound);
            }

            if (paymentSchedule.Status != PaymentScheduleStatus.Pending && paymentSchedule.Status != PaymentScheduleStatus.Missed)
            {
                return ServiceResult<string>.Fail("Payment schedule is not pending.", resultType: ServiceResultType.BadRequest);
            }

            if (paymentSchedule.DueDate - DateTime.UtcNow > TimeSpan.FromDays(7))
            {
                return ServiceResult<string>.Fail("Payment can only be made within 7 days of the due date.", resultType: ServiceResultType.BadRequest);
            }

            if (paymentSchedule.Contract.RenterId != userId)
            {
                return ServiceResult<string>.Fail("Unauthorized access to payment schedule.", resultType: ServiceResultType.Unauthorized);
            }

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(paymentSchedule.Amount * 100),

                Currency = paymentSchedule.Currency,
                Metadata = new Dictionary<string, string>
                {
                    { "paymentScheduleId", paymentScheduleId.ToString() }
                }
            };

            var service = new PaymentIntentService();
            var intent = await service.CreateAsync(options);

            return ServiceResult<string>.Ok(intent.ClientSecret, "ClientSecret created successfully.", ServiceResultType.Success);
        }

        public Task Webhook()
        {
            throw new NotImplementedException();
        }
    }
}
