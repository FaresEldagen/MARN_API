using MARN_API.DTOs.Common;
using MARN_API.Enums;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using Stripe;
using Stripe.Checkout;

namespace MARN_API.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IWorkflowPaymentRepo _paymentRepo;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public PaymentService(IWorkflowPaymentRepo paymentRepo, IConfiguration configuration, ILogger<PaymentService> logger, IServiceProvider serviceProvider)
        {
            _paymentRepo = paymentRepo;
            _configuration = configuration;
            _logger = logger;
            _serviceProvider = serviceProvider;
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        public async Task<(string Url, string SessionId)> CreateCheckoutSessionAsync(decimal amount, long propertyId, Guid ownerId, string ownerAccountId, Guid renterId, string? renterEmail, string successUrl, string cancelUrl, DateTime dueDate)
        {
            var feePercentage = _configuration.GetValue<decimal?>("Stripe:PlatformFeePercentage") ?? 10m;
            var currency = (_configuration["Stripe:Currency"] ?? "egp").ToLowerInvariant();

            var amountInCents = (long)(amount * 100);
            var platformFeeInCents = (long)(amountInCents * (feePercentage / 100m));
            var platformFee = platformFeeInCents / 100m;
            var ownerAmount = amount - platformFee;

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = ["card"],
                LineItems =
                [
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = amountInCents,
                            Currency = currency,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Rental Payment for Property {propertyId}"
                            }
                        },
                        Quantity = 1
                    }
                ],
                Mode = "payment",
                CustomerEmail = renterEmail,
                ClientReferenceId = renterId.ToString(),
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl
            };

            if (!string.IsNullOrWhiteSpace(ownerAccountId) && ownerAccountId.StartsWith("acct_", StringComparison.OrdinalIgnoreCase))
            {
                options.PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    ApplicationFeeAmount = platformFeeInCents,
                    TransferData = new SessionPaymentIntentDataTransferDataOptions
                    {
                        Destination = ownerAccountId
                    }
                };
            }

            var sessionService = new SessionService();
            var session = await sessionService.CreateAsync(options);

            var payment = new Payment
            {
                StripeSessionId = session.Id,
                AmountTotal = amount,
                PlatformFee = platformFee,
                OwnerAmount = ownerAmount,
                OwnerId = ownerId,
                OwnerStripeAccountId = ownerAccountId,
                PropertyId = propertyId,
                RenterId = renterId,
                RenterEmail = renterEmail,
                Status = PaymentStatus.Pending,
                DueDate = dueDate,
                Currency = currency.ToUpperInvariant()
            };

            await _paymentRepo.CreatePaymentRecordAsync(payment);
            return (session.Url ?? string.Empty, session.Id);
        }

        public Task<Payment?> GetPaymentByIdAsync(long paymentId) => _paymentRepo.GetPaymentByIdAsync(paymentId);
        public Task<Payment?> GetPaymentBySessionIdAsync(string sessionId) => _paymentRepo.GetPaymentBySessionIdAsync(sessionId);
        public Task<PagedResult<Payment>> GetPaymentsByUserIdAsync(Guid userId, int pageNumber, int pageSize) => _paymentRepo.GetPaymentsByUserIdAsync(userId, pageNumber, pageSize);
        public Task<PagedResult<Payment>> GetPaymentsByPropertyIdAsync(long propertyId, int pageNumber, int pageSize) => _paymentRepo.GetPaymentsByPropertyIdAsync(propertyId, pageNumber, pageSize);
        public Task<PagedResult<Payment>> GetAllPaymentsAsync(int pageNumber, int pageSize) => _paymentRepo.GetAllPaymentsAsync(pageNumber, pageSize);

        public async Task<bool> FulfillPaymentAsync(string sessionId)
        {
            var payment = await _paymentRepo.GetPaymentBySessionIdAsync(sessionId);
            if (payment is null)
            {
                _logger.LogWarning("Webhook received completion for unknown session {SessionId}", sessionId);
                return false;
            }

            var alreadyMarkedSucceeded = payment.Status == PaymentStatus.Succeeded;

            if (!alreadyMarkedSucceeded)
            {
                var sessionService = new SessionService();
                var session = await sessionService.GetAsync(sessionId);
                payment.PaymentIntentId = session.PaymentIntentId;

                if (!string.IsNullOrWhiteSpace(session.PaymentIntentId))
                {
                    var paymentIntentService = new PaymentIntentService();
                    var paymentIntent = await paymentIntentService.GetAsync(session.PaymentIntentId);

                    payment.Status = paymentIntent.Status switch
                    {
                        "succeeded" => PaymentStatus.Succeeded,
                        "processing" => PaymentStatus.Pending,
                        "canceled" => PaymentStatus.Failed,
                        _ => PaymentStatus.Pending
                    };

                    if (!string.IsNullOrWhiteSpace(paymentIntent.LatestChargeId))
                    {
                        var chargeService = new ChargeService();
                        var charge = await chargeService.GetAsync(paymentIntent.LatestChargeId);
                        payment.ReceiptUrl = charge.ReceiptUrl;
                    }
                }
                else
                {
                    payment.Status = session.PaymentStatus == "paid" ? PaymentStatus.Succeeded : PaymentStatus.Pending;
                }
            }

            if (payment.Status != PaymentStatus.Succeeded)
            {
                payment.PaidAt = null;
                payment.AvailableAt = null;
                await _paymentRepo.UpdatePaymentRecordAsync(payment);
                _logger.LogInformation("Skipping lease fulfillment for session {SessionId} because payment status is {PaymentStatus}", sessionId, payment.Status);
                return true;
            }

            payment.PaidAt = DateTime.UtcNow;
            payment.AvailableAt = DateTime.UtcNow;

            await _paymentRepo.UpdatePaymentRecordAsync(payment);

            using var scope = _serviceProvider.CreateScope();
            var rentalWorkflowService = scope.ServiceProvider.GetRequiredService<IRentalWorkflowService>();
            _logger.LogInformation(
                "Continuing lease fulfillment for succeeded payment session {SessionId}. Existing contract id: {ContractId}",
                sessionId,
                payment.ContractId);
            await rentalWorkflowService.CompleteLeaseFulfillmentAsync(sessionId, payment.PaymentIntentId, payment.ReceiptUrl);

            return true;
        }

        public async Task<bool> ExpirePaymentAsync(string sessionId)
        {
            var payment = await _paymentRepo.GetPaymentBySessionIdAsync(sessionId);
            if (payment is null)
            {
                _logger.LogWarning("Payment record not found for expiration session {SessionId}", sessionId);
                return false;
            }

            if (payment.Status == PaymentStatus.Succeeded)
            {
                return true;
            }

            payment.Status = PaymentStatus.Expired;
            await _paymentRepo.UpdatePaymentRecordAsync(payment);

            using var scope = _serviceProvider.CreateScope();
            var rentalWorkflowService = scope.ServiceProvider.GetRequiredService<IRentalWorkflowService>();
            await rentalWorkflowService.ExpireLeaseFulfillmentAsync(sessionId);

            return true;
        }
    }
}
