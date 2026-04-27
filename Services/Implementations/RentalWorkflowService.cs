using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MARN_API.Data;
using MARN_API.DTOs.Common;
using MARN_API.DTOs.Contracts;
using MARN_API.Enums;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using Stripe.Checkout;

namespace MARN_API.Services.Implementations
{
    public class RentalWorkflowService : IRentalWorkflowService
    {
        private readonly AppDbContext _dbContext;
        private readonly IRentalTransactionRepo _rentalTransactionRepo;
        private readonly IConnectedAccountRepo _connectedAccountRepo;
        private readonly IPaymentService _paymentService;
        private readonly IContractService _contractService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RentalWorkflowService> _logger;

        public RentalWorkflowService(
            AppDbContext dbContext,
            IRentalTransactionRepo rentalTransactionRepo,
            IConnectedAccountRepo connectedAccountRepo,
            IPaymentService paymentService,
            IContractService contractService,
            UserManager<ApplicationUser> userManager,
            ILogger<RentalWorkflowService> logger)
        {
            _dbContext = dbContext;
            _rentalTransactionRepo = rentalTransactionRepo;
            _connectedAccountRepo = connectedAccountRepo;
            _paymentService = paymentService;
            _contractService = contractService;
            _userManager = userManager;
            _logger = logger;
        }

        public Task<PagedResult<RentalTransaction>> GetAllTransactionsAsync(int pageNumber, int pageSize)
        {
            return _rentalTransactionRepo.GetAllAsync(pageNumber, pageSize);
        }

        public Task<PagedResult<RentalTransaction>> GetTransactionsByUserIdAsync(Guid userId, int pageNumber, int pageSize)
        {
            return _rentalTransactionRepo.GetByUserIdAsync(userId, pageNumber, pageSize);
        }

        public Task<PagedResult<RentalTransaction>> GetTransactionsByPropertyIdAsync(long propertyId, int pageNumber, int pageSize)
        {
            return _rentalTransactionRepo.GetByPropertyIdAsync(propertyId, pageNumber, pageSize);
        }

        public Task<RentalTransaction?> GetTransactionByIdAsync(Guid id) => _rentalTransactionRepo.GetByIdAsync(id);
        public Task<RentalTransaction?> GetTransactionByPaymentRecordIdAsync(long paymentRecordId) => _rentalTransactionRepo.GetByPaymentRecordIdAsync(paymentRecordId);
        public Task<RentalTransaction?> GetTransactionByContractIdAsync(long contractId) => _rentalTransactionRepo.GetByContractIdAsync(contractId);

        public async Task<string> StartCheckoutAsync(long propertyId, Guid renterId, string successUrl, string cancelUrl)
        {
            var property = await _dbContext.Properties
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == propertyId && p.DeletedAt == null && p.IsActive);

            if (property is null)
            {
                throw new InvalidOperationException("Property not found.");
            }

            if (property.OwnerId == renterId)
            {
                throw new InvalidOperationException("Owners cannot rent their own properties.");
            }

            var renter = await _userManager.FindByIdAsync(renterId.ToString())
                ?? throw new InvalidOperationException("Authenticated renter was not found.");

            var connectedAccount = await _connectedAccountRepo.GetByApplicationUserIdAsync(property.OwnerId);
            if (connectedAccount is null || string.IsNullOrWhiteSpace(connectedAccount.StripeAccountId))
            {
                throw new InvalidOperationException("The owner has not created a connected Stripe account yet.");
            }

            if (!connectedAccount.IsOnboardingComplete)
            {
                throw new InvalidOperationException("The owner has not completed Stripe onboarding yet.");
            }

            var (leaseStartDate, leaseEndDate) = GetDefaultLeaseWindow();
            var isPropertyAvailable = await IsPropertyAvailableForLeaseWindowAsync(property.Id, leaseStartDate, leaseEndDate);
            if (!isPropertyAvailable)
            {
                throw new InvalidOperationException("This property is already rented for the requested period.");
            }

            var activeSession = await _rentalTransactionRepo.GetActiveInitiatedSessionAsync(renterId, propertyId);
            if (activeSession is not null && !string.IsNullOrWhiteSpace(activeSession.StripeSessionId))
            {
                try
                {
                    var sessionService = new SessionService();
                    var session = await sessionService.GetAsync(activeSession.StripeSessionId);
                    if (session.Status == "open" && !string.IsNullOrWhiteSpace(session.Url))
                    {
                        return session.Url;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to retrieve the previous Stripe session. A new session will be created.");
                }
            }

            var dueDate = DateTime.UtcNow;
            var checkoutResult = await _paymentService.CreateCheckoutSessionAsync(
                property.Price,
                property.Id,
                property.OwnerId,
                connectedAccount.StripeAccountId,
                renterId,
                renter.Email,
                successUrl,
                cancelUrl,
                dueDate);

            var payment = await _paymentService.GetPaymentBySessionIdAsync(checkoutResult.SessionId);

            var trackingRecord = new RentalTransaction
            {
                RenterId = renterId,
                OwnerId = property.OwnerId,
                PropertyId = property.Id,
                StripeSessionId = checkoutResult.SessionId,
                PaymentId = payment?.Id,
                Status = RentalTransactionStatus.Initiated,
                PaymentStatus = RentalPaymentStatus.NotPaid
            };

            await _rentalTransactionRepo.AddAsync(trackingRecord);
            return checkoutResult.Url;
        }

        public async Task<bool> CompleteLeaseFulfillmentAsync(string stripeSessionId, string? paymentIntentId, string? receiptUrl)
        {
            var trackingRecord = await _rentalTransactionRepo.GetBySessionIdAsync(stripeSessionId);
            if (trackingRecord is null)
            {
                _logger.LogWarning("Could not find RentalTransaction for session {SessionId}", stripeSessionId);
                return false;
            }

            if (trackingRecord.Status == RentalTransactionStatus.Anchored || trackingRecord.Status == RentalTransactionStatus.PendingAnchoring)
            {
                return true;
            }

            var property = await _dbContext.Properties
                .FirstOrDefaultAsync(p => p.Id == trackingRecord.PropertyId)
                ?? throw new InvalidOperationException("Property not found for rental workflow.");

            var owner = await _userManager.FindByIdAsync(property.OwnerId.ToString())
                ?? throw new InvalidOperationException("Owner not found for rental workflow.");

            var renter = await _userManager.FindByIdAsync(trackingRecord.RenterId.ToString())
                ?? throw new InvalidOperationException("Renter not found for rental workflow.");

            var payment = await _paymentService.GetPaymentBySessionIdAsync(stripeSessionId);
            if (payment is null)
            {
                throw new InvalidOperationException("Payment record not found for rental workflow.");
            }

            trackingRecord.Status = RentalTransactionStatus.Paid;
            trackingRecord.PaymentStatus = RentalPaymentStatus.Paid;
            trackingRecord.PaymentId = payment.Id;
            await _rentalTransactionRepo.UpdateAsync(trackingRecord);

            var (leaseStartDate, leaseEndDate) = GetDefaultLeaseWindow();
            var isPropertyAvailable = await IsPropertyAvailableForLeaseWindowAsync(property.Id, leaseStartDate, leaseEndDate);
            if (!isPropertyAvailable)
            {
                trackingRecord.Status = RentalTransactionStatus.Failed;
                trackingRecord.CompletedAt = DateTime.UtcNow;
                await _rentalTransactionRepo.UpdateAsync(trackingRecord);

                _logger.LogWarning(
                    "Payment succeeded for session {SessionId}, but property {PropertyId} is no longer available for lease window {LeaseStartDate} - {LeaseEndDate}. Manual review may be required.",
                    stripeSessionId,
                    property.Id,
                    leaseStartDate,
                    leaseEndDate);

                return false;
            }

            trackingRecord.Status = RentalTransactionStatus.ContractGenerated;
            await _rentalTransactionRepo.UpdateAsync(trackingRecord);

            var contractRequest = BuildContractRequest(property, owner, renter, payment, paymentIntentId, receiptUrl);
            var contract = await _contractService.CreateWorkflowContractAsync(
                contractRequest,
                property.OwnerId,
                renter.Id,
                property.Id,
                leaseStartDate,
                leaseEndDate,
                PaymentFrequency.Monthly);

            contract.SignedByRenterAt = payment.PaidAt;
            contract.UpdatedAt = DateTime.UtcNow;

            payment.ContractId = contract.Id;
            _dbContext.Payments.Update(payment);
            _dbContext.Contracts.Update(contract);
            await _dbContext.SaveChangesAsync();

            trackingRecord.ContractId = contract.Id;
            trackingRecord.Status = RentalTransactionStatus.PendingAnchoring;
            trackingRecord.CompletedAt = DateTime.UtcNow;
            await _rentalTransactionRepo.UpdateAsync(trackingRecord);

            return true;
        }

        public async Task<bool> ExpireLeaseFulfillmentAsync(string stripeSessionId)
        {
            var trackingRecord = await _rentalTransactionRepo.GetBySessionIdAsync(stripeSessionId);
            if (trackingRecord is null)
            {
                return false;
            }

            if (trackingRecord.Status == RentalTransactionStatus.Initiated)
            {
                trackingRecord.Status = RentalTransactionStatus.Expired;
                trackingRecord.PaymentStatus = RentalPaymentStatus.NotPaid;
                await _rentalTransactionRepo.UpdateAsync(trackingRecord);
            }

            return true;
        }

        private async Task<bool> IsPropertyAvailableForLeaseWindowAsync(long propertyId, DateOnly leaseStartDate, DateOnly leaseEndDate)
        {
            return !await _dbContext.Contracts.AnyAsync(contract =>
                contract.PropertyId == propertyId &&
                (contract.Status == ContractStatus.Active || contract.Status == ContractStatus.Pending) &&
                contract.LeaseStartDate != null &&
                contract.LeaseEndDate != null &&
                !(leaseEndDate < contract.LeaseStartDate.Value || leaseStartDate > contract.LeaseEndDate.Value));
        }

        private static (DateOnly LeaseStartDate, DateOnly LeaseEndDate) GetDefaultLeaseWindow()
        {
            var leaseStartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));
            var leaseEndDate = leaseStartDate.AddYears(1);
            return (leaseStartDate, leaseEndDate);
        }

        private static ContractPdfRequest BuildContractRequest(Property property, ApplicationUser owner, ApplicationUser renter, Payment payment, string? paymentIntentId, string? receiptUrl)
        {
            var (leaseStartDate, leaseEndDate) = GetDefaultLeaseWindow();

            return new ContractPdfRequest
            {
                IssuedAtUtc = DateTime.UtcNow,
                Landlord = new PartyInfo
                {
                    FullName = $"{owner.FirstName} {owner.LastName}".Trim(),
                    NationalId = owner.NationalIDNumber,
                    Email = owner.Email,
                    PhoneNumber = owner.PhoneNumber,
                    Address = owner.ArabicAddress ?? owner.Country.ToString()
                },
                Tenant = new PartyInfo
                {
                    FullName = $"{renter.FirstName} {renter.LastName}".Trim(),
                    NationalId = renter.NationalIDNumber,
                    Email = renter.Email,
                    PhoneNumber = renter.PhoneNumber,
                    Address = renter.ArabicAddress ?? renter.Country.ToString()
                },
                Property = new PropertyInfo
                {
                    ListingTitle = property.Title,
                    AddressLine = property.Address,
                    UnitNumber = property.Id.ToString(),
                    City = "Cairo",
                    Country = "Egypt",
                    Description = property.Description
                },
                RentalTerms = new RentalTermsInfo
                {
                    MonthlyRentAmount = payment.AmountTotal,
                    PlatformFeeAmount = payment.PlatformFee,
                    Currency = payment.Currency,
                    LeaseStartDate = leaseStartDate,
                    LeaseEndDate = leaseEndDate,
                    PaymentDueDay = payment.DueDate.Day,
                    PaymentMethod = "Stripe Credit Card",
                    CheckInWindow = "2:00 PM - 5:00 PM",
                    CheckOutWindow = "Until 11:00 AM"
                },
                ElectronicSignature = new ElectronicSignatureInfo
                {
                    SignerName = $"{renter.FirstName} {renter.LastName}".Trim(),
                    SignerNationalId = renter.NationalIDNumber,
                    SignedAtUtc = DateTime.UtcNow,
                    PaymentIntentId = paymentIntentId,
                    ReceiptUrl = receiptUrl ?? "Receipt unavailable",
                    ConsentStatement = "I digitally agree to all terms outlined in this agreement."
                },
                AdditionalTerms =
                [
                    "No smoking inside the premises.",
                    "Pets are subject to prior approval."
                ],
                GoverningLawNote = "The laws of Egypt govern this contract."
            };
        }
    }
}
