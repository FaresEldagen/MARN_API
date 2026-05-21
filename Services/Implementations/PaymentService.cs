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
using MARN_API.DTOs.Dashboard;
using MARN_API.Enums.Account;


namespace MARN_API.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepo _paymentRepo;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<PaymentService> _logger;
        private readonly IConfiguration _configuration;
        private readonly ICurrencyExchangeService _currencyExchangeService;
        private readonly MARN_API.Data.AppDbContext _context;

        public PaymentService(
            IPaymentRepo paymentRepo,
            IMapper mapper,
            INotificationService notificationService,
            UserManager<ApplicationUser> userManager,
            ILogger<PaymentService> logger,
            IConfiguration configuration,
            ICurrencyExchangeService currencyExchangeService,
            MARN_API.Data.AppDbContext context)

        {
            _paymentRepo = paymentRepo;
            _mapper = mapper;
            _notificationService = notificationService;
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
            _currencyExchangeService = currencyExchangeService;
            _context = context;
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

            if (paymentSchedule.Contract.Status != Enums.Contract.ContractStatus.Active ||
                paymentSchedule.Status == PaymentScheduleStatus.Cancelled)
            {
                _logger.LogWarning("Create Payment Intent failed: Contract or payment schedule is cancelled for paymentScheduleId: {paymentScheduleId}", paymentScheduleId);
                return ServiceResult<string>.Fail("This payment is no longer available because the contract has been cancelled.", resultType: ServiceResultType.Forbidden);
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
                    ServiceResultType.Success,
                    code: "ZZ_EXISTING_PAYMENT_INTENT_CLIENT_SECRET_RETURNED"
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
                return ServiceResult<string>.Ok(
                    intent.ClientSecret,
                    "ClientSecret created successfully.",
                    ServiceResultType.Success,
                    code: "ZZ_PAYMENT_INTENT_CLIENT_SECRET_CREATED_SUCCESSFULLY");
            }
            catch (StripeException e)
            {
                _logger.LogError(e, "Stripe API Error while creating PaymentIntent for ScheduleId: {ScheduleId}", paymentScheduleId);
                return ServiceResult<string>.Fail(e.StripeError?.Message ?? "Payment provider error.", resultType: ServiceResultType.BadRequest);
            }
        }

        public async Task<ServiceResult<string>> CreateOrGetConnectOnboardingLink(Guid userId)
        {
            var owner = await _userManager.FindByIdAsync(userId.ToString());
            if (owner == null)
            {
                _logger.LogWarning("Create Connect Account failed: Owner not found for userId: {userId}", userId);
                return ServiceResult<string>.Fail("Owner not found", resultType: ServiceResultType.Unauthorized);
            }

            var accountService = new Stripe.AccountService();

            try
            {
                if (string.IsNullOrEmpty(owner.StripeAccountId))
                {
                    var accountOptions = new AccountCreateOptions
                    {
                        Type = "express",
                        Country = "EG",
                        Email = owner.Email,
                        Capabilities = new AccountCapabilitiesOptions
                        {
                            Transfers = new AccountCapabilitiesTransfersOptions
                            {
                                Requested = true
                            }
                        },
                        TosAcceptance = new AccountTosAcceptanceOptions
                        {
                            ServiceAgreement = "recipient"
                        }
                    };

                    var account = await accountService.CreateAsync(accountOptions);

                    owner.StripeAccountId = account.Id;

                    var updateResult = await _userManager.UpdateAsync(owner);
                    if (!updateResult.Succeeded)
                    {
                        _logger.LogError("Failed to save StripeAccountId for ownerId: {OwnerId}. Errors: {Errors}",
                            userId, string.Join(", ", updateResult.Errors.Select(e => e.Description)));
                        return ServiceResult<string>.Fail("Failed to save Stripe account. Please try again.", resultType: ServiceResultType.BadRequest);
                    }
                }

                // Create onboarding link
                var accountLinkService = new AccountLinkService();

                var linkOptions = new AccountLinkCreateOptions
                {
                    Account = owner.StripeAccountId,
                    RefreshUrl = _configuration["Stripe:RefreshUrl"],
                    ReturnUrl = _configuration["Stripe:ReturnUrl"],
                    Type = "account_onboarding"
                };

                var accountLink = await accountLinkService.CreateAsync(linkOptions);

                return ServiceResult<string>.Ok(
                    accountLink.Url,
                    "Onboarding link created successfully.",
                    ServiceResultType.Success,
                    code: "ZZ_STRIPE_CONNECT_ONBOARDING_LINK_CREATED_SUCCESSFULLY");
            }
            catch (StripeException e)
            {
                _logger.LogError(e, "Stripe API Error while creating Connect account or link for userId: {UserId}. Error: {Message}", userId, e.StripeError?.Message);
                return ServiceResult<string>.Fail(e.StripeError?.Message ?? "Payment provider error.", resultType: ServiceResultType.BadRequest);
            }
        }

        public async Task<ServiceResult<bool>> Withdraw(Guid ownerId)
        {
            var owner = await _userManager.FindByIdAsync(ownerId.ToString());
            if (owner == null)
            {
                _logger.LogWarning("Withdraw failed: Owner not found for ownerId: {ownerId}", ownerId);
                return ServiceResult<bool>.Fail("Owner not found", resultType: ServiceResultType.Unauthorized);
            }

            if (string.IsNullOrEmpty(owner.StripeAccountId) ||
                !owner.StripePayoutsEnabled ||
                !owner.StripeChargesEnabled)
            {
                _logger.LogWarning("Withdraw failed: Stripe account not connected for ownerId: {ownerId}", ownerId);
                return ServiceResult<bool>.Fail("Stripe account not connected. Please connect your Stripe account first.", resultType: ServiceResultType.BadRequest);
            }

            var accountService = new Stripe.AccountService();
            var account = await accountService.GetAsync(owner.StripeAccountId);
            if (!account.PayoutsEnabled || !account.ChargesEnabled)
            {
                owner.StripePayoutsEnabled = account.PayoutsEnabled;
                owner.StripeChargesEnabled = account.ChargesEnabled;
                var result = await _userManager.UpdateAsync(owner);

                _logger.LogWarning("Withdraw failed: Stripe account not fully activated for ownerId: {ownerId}", ownerId);
                return ServiceResult<bool>.Fail("Stripe account not fully activated. Please complete the onboarding process to enable charges and payouts.", resultType: ServiceResultType.BadRequest);
            }

            var withdrawablePayments = await _paymentRepo.GetWithdrawablePayments(ownerId);

            if (!withdrawablePayments.Any())
            {
                _logger.LogWarning("Withdraw failed: No available funds for ownerId: {ownerId}", ownerId);
                return ServiceResult<bool>.Fail("No funds available for withdrawal.", resultType: ServiceResultType.BadRequest);
            }

            var amount = withdrawablePayments.Sum(p => p.OwnerAmount);

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Mark payments as Withdrawn before calling Stripe to prevent double-withdraw on concurrent requests
                foreach (var payment in withdrawablePayments)
                    payment.Status = PaymentStatus.Withdrawn;

                await _paymentRepo.UpdatePayments(withdrawablePayments);

                var transferService = new TransferService();
                // Detect platform's available currency to avoid "insufficient funds" if currencies mismatch
                var balanceService = new BalanceService();
                var platformBalance = await balanceService.GetAsync();

                var usdBalance = platformBalance.Available
                    .FirstOrDefault(b => b.Currency.ToLower() == "usd");
                if (usdBalance == null || usdBalance.Amount <= 0)
                {
                    await transaction.RollbackAsync();
                    return ServiceResult<bool>.Fail(
                        "No USD balance available.",
                        resultType: ServiceResultType.BadRequest);
                }

                var transferCurrency = "usd";
                var transferAmount = await _currencyExchangeService.ConvertAsync(amount, "EGP", "USD");

                var transferOptions = new TransferCreateOptions
                {
                    Amount = (long)(transferAmount * 100),
                    Currency = transferCurrency,
                    Destination = owner.StripeAccountId,
                    SourceType = "card",
                };
                var transfer = await transferService.CreateAsync(transferOptions);

                await transaction.CommitAsync();

                _logger.LogInformation("Withdraw successful for ownerId: {ownerId}, amount: {amount} {currency}", ownerId, transferAmount, transferCurrency);
                return ServiceResult<bool>.Ok(
                    true,
                    $"Withdrawal of {transferAmount} {transferCurrency.ToUpper()} initiated successfully.",
                    ServiceResultType.Success,
                    code: "ZZ_WITHDRAWAL_INITIATED_SUCCESSFULLY");
            }
            catch (StripeException e)
            {
                await transaction.RollbackAsync();
                _logger.LogError(e, "Stripe API Error while creating transfer for ownerId: {ownerId}. Payment statuses reverted.", ownerId);
                return ServiceResult<bool>.Fail(e.StripeError?.Message ?? "Payment provider error.", resultType: ServiceResultType.BadRequest);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Unexpected Error while creating transfer for ownerId: {ownerId}", ownerId);
                return ServiceResult<bool>.Fail("An unexpected error occurred.", resultType: ServiceResultType.InternalError);
            }
        }



        #region Stripe Webhook Handlers
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

            var wasCancelledDuringProcessing =
                paymentSchedule.Contract.Status != Enums.Contract.ContractStatus.Active ||
                paymentSchedule.Status == PaymentScheduleStatus.Cancelled;

            if (wasCancelledDuringProcessing)
            {
                _logger.LogWarning(
                    "Handle Successful Payment continuing for paymentScheduleId: {paymentScheduleId} even though contract/schedule is cancelled, to preserve ledger consistency.",
                    scheduleIdString);
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
                TitleKey = "NOTIFICATION_PAYMENT_RECEIVED_TITLE",
                BodyKey = "NOTIFICATION_PAYMENT_RECEIVED_BODY",
                LocalizationArguments = new()
                {
                    payment.OwnerAmount.ToString(),
                    payment.Currency,
                    paymentSchedule.Contract.Property.Title,
                    paymentSchedule.DueDate.ToString("yyyy-MM-dd"),
                    payment.AvailableAt.ToString("yyyy-MM-dd")
                },
                Title = "Payment Received",
                Body = $"You have received a payment of {payment.OwnerAmount} {payment.Currency} for \"{paymentSchedule.Contract.Property.Title}\".\n" +
                       $"This payment is for the due date {paymentSchedule.DueDate:yyyy-MM-dd}.\n\n" +
                       $"You can withdraw this amount after {payment.AvailableAt.ToString("yyyy-MM-dd")}.",

                ActionType = NotificationActionType.OwnerDashboard
            });

            await _notificationService.SendNotificationAsync(new NotificationRequestDto
            {
                UserId = paymentSchedule.Contract.RenterId.ToString(),
                UserType = NotificationUserType.Renter,
                Type = NotificationType.PaymentSuccessful,
                TitleKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE",
                BodyKey = "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY",
                LocalizationArguments = new()
                {
                    payment.AmountTotal.ToString(),
                    payment.Currency,
                    paymentSchedule.Contract.Property.Title,
                    paymentSchedule.DueDate.ToString("yyyy-MM-dd")
                },
                Title = "Payment Successful",
                Body = $"Your payment of {payment.AmountTotal} {payment.Currency} for \"{paymentSchedule.Contract.Property.Title}\" has been successful.\n" +
                       $"This payment is for the due date {paymentSchedule.DueDate:yyyy-MM-dd}.",

                ActionType = NotificationActionType.RenterDashboard
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
                    TitleKey = "NOTIFICATION_PAYMENT_FAILED_TITLE",
                    BodyKey = "NOTIFICATION_PAYMENT_FAILED_BODY",
                    LocalizationArguments = new() { paymentSchedule.Contract.Property.Title },
                    Title = "Payment Failed",
                    Body = $"Your payment for \"{paymentSchedule.Contract.Property.Title}\" has failed. \n Please try again.",

                    ActionType = NotificationActionType.RenterDashboard
                });

                _logger.LogInformation("Handle Failed Payment processed for paymentScheduleId: {paymentScheduleId}. Error: {Error}", scheduleIdString, errorMessage);
            }
            else
            {
                _logger.LogError("Handle Failed Payment failed: Payment schedule not found for Id: {paymentScheduleId}", scheduleIdString);
            }
        }


        public async Task HandleConnectedAccountUpdated(Account account)
        {
            _logger.LogInformation("Handle Connected Account Updated attempt for StripeAccountId: {StripeAccountId}", account.Id);

            var owner = await _userManager.Users.FirstOrDefaultAsync(o => o.StripeAccountId == account.Id);
            if (owner == null)
            {
                _logger.LogError("Handle Connected Account Updated failed: Owner not found for StripeAccountId: {StripeAccountId}", account.Id);
                return;
            }

            var wasReady =
                owner.StripeChargesEnabled &&
                owner.StripePayoutsEnabled;

            var isReady =
                account.ChargesEnabled &&
                account.PayoutsEnabled;


            owner.StripeChargesEnabled = account.ChargesEnabled;
            owner.StripePayoutsEnabled = account.PayoutsEnabled;

            await _userManager.UpdateAsync(owner);


            // First time for the account to be ready
            if (!wasReady && isReady)
            {

                await _notificationService.SendNotificationAsync(new NotificationRequestDto
                {
                    UserId = owner.Id.ToString(),
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.ConnectAccountSuccess,
                    TitleKey = "NOTIFICATION_CONNECT_SUCCESS_TITLE",
                    BodyKey = "NOTIFICATION_CONNECT_SUCCESS_BODY",
                    Title = "Connect Account Activated",
                    Body = "Your Stripe Connect account has been activated and is now ready to withdraw your payments.",
                        ActionType = NotificationActionType.OwnerDashboard
                    });


                _logger.LogInformation(
                    "Connect account activated for ownerId: {OwnerId}",
                    owner.Id);
            }

            // Problem happened
            else if (!isReady &&
                     !string.IsNullOrEmpty(account.Requirements?.DisabledReason))
            {

                await _notificationService.SendNotificationAsync(new NotificationRequestDto
                {
                    UserId = owner.Id.ToString(),
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.ConnectAccountFailed,
                    TitleKey = "NOTIFICATION_CONNECT_FAILED_TITLE",
                    BodyKey = "NOTIFICATION_CONNECT_FAILED_BODY",
                    Title = "Connect Account Failed",
                    Body = "Your Stripe Connect account is not fully activated. Please complete the onboarding process to enable charges and payouts.",

                        ActionType = NotificationActionType.OwnerDashboard
                    });


                _logger.LogInformation(
                    "Handle Connected Account Updated: Charges and payouts not enabled for StripeAccountId: {StripeAccountId}", 
                    account.Id);
            }
        }


        public async Task HandleTransferCreated(Transfer transfer)
        {
            var owner = await _userManager.Users
                .FirstOrDefaultAsync(o => o.StripeAccountId == transfer.DestinationId);

            if (owner == null)
            {
                _logger.LogError("HandleTransferCreated: Owner not found for StripeAccountId: {StripeAccountId}", transfer.DestinationId);
                return;
            }

            await _notificationService.SendNotificationAsync(new NotificationRequestDto
            {
                UserId = owner.Id.ToString(),
                UserType = NotificationUserType.Owner,
                Type = NotificationType.WithdrawSuccess,
                TitleKey = "NOTIFICATION_WITHDRAW_SUCCESS_TITLE",
                BodyKey = "NOTIFICATION_WITHDRAW_SUCCESS_BODY",
                LocalizationArguments = new()
                {
                    (transfer.Amount / 100m).ToString(),
                    transfer.Currency.ToUpperInvariant()
                },
                Title = "Withdrawal Initiated",
                Body = $"A withdrawal of {(transfer.Amount / 100m)} {transfer.Currency.ToUpper()} has been initiated to your connected account.\n" +
                $"It should reflect in your bank account within a few business days.",

                ActionType = NotificationActionType.OwnerDashboard
            });
        }

        public async Task HandleTransferReversed(Transfer transfer)
        {
            var owner = await _userManager.Users
                .FirstOrDefaultAsync(o => o.StripeAccountId == transfer.DestinationId);

            if (owner == null)
            {
                _logger.LogError("HandleTransferReversed: Owner not found for StripeAccountId: {StripeAccountId}", transfer.DestinationId);
                return;
            }

            await _notificationService.SendNotificationAsync(new NotificationRequestDto
            {
                UserId = owner.Id.ToString(),
                UserType = NotificationUserType.Owner,
                Type = NotificationType.WithdrawFailed,
                TitleKey = "NOTIFICATION_WITHDRAW_FAILED_TITLE",
                BodyKey = "NOTIFICATION_WITHDRAW_FAILED_BODY",
                LocalizationArguments = new()
                {
                    (transfer.Amount / 100m).ToString(),
                    transfer.Currency.ToUpperInvariant()
                },
                Title = "Withdrawal Failed",
                Body = $"A withdrawal of {(transfer.Amount / 100m)} {transfer.Currency.ToUpper()} to your connected account has failed. Please check your Stripe dashboard for details.",
                ActionType = NotificationActionType.OwnerDashboard
            });
        }
        #endregion
    }
}
