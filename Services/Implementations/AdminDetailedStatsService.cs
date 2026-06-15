using MARN_API.DTOs.Admin;
using MARN_API.Enums;
using MARN_API.Enums.Contract;
using MARN_API.Enums.Notification;
using MARN_API.Enums.Payment;
using MARN_API.Enums.Property;
using MARN_API.DTOs.Notification;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using Stripe;
using Hangfire;

namespace MARN_API.Services.Implementations
{
    public class AdminDetailedStatsService : IAdminDetailedStatsService
    {
        private const int MaxPageSize = 100;
        private static readonly TimeSpan ImageRestoreGracePeriod = TimeSpan.FromDays(7);
        private readonly IAdminDetailedStatsRepo _detailedStatsRepo;
        private readonly INotificationService _notificationService;
        private readonly IAppTextLocalizer _localizer;
        private readonly ILogger<AdminDetailedStatsService> _logger;
        private readonly IPropertyService _propertyService;
        private readonly IPropertyRepo _propertyRepo;
        private readonly IExternalPropertyAiClient _externalPropertyAiClient;

        public AdminDetailedStatsService(
            IAdminDetailedStatsRepo detailedStatsRepo,
            INotificationService notificationService,
            IAppTextLocalizer localizer,
            ILogger<AdminDetailedStatsService> logger,
            IPropertyService propertyService,
            IPropertyRepo propertyRepo,
            IExternalPropertyAiClient externalPropertyAiClient)
        {
            _detailedStatsRepo = detailedStatsRepo;
            _notificationService = notificationService;
            _localizer = localizer;
            _logger = logger;
            _propertyService = propertyService;
            _propertyRepo = propertyRepo;
            _externalPropertyAiClient = externalPropertyAiClient;
        }

        public async Task<ServiceResult<AdminDetailedUsersResponseDto>> GetUsersAsync(AdminDetailedUsersQueryDto query)
        {
            var period = ResolvePeriod(query);
            if (!period.Success)
                return ServiceResult<AdminDetailedUsersResponseDto>.Fail(period.Message!, resultType: period.ResultType);

            var result = await _detailedStatsRepo.GetUsersAsync(query, period.Data!.FromUtc, period.Data.ToUtc, period.Data.GroupByDay);
            result.AppliedPeriod = period.Data.ToDto();
            return ServiceResult<AdminDetailedUsersResponseDto>.Ok(result);
        }

        public async Task<ServiceResult<AdminDetailedPropertiesResponseDto>> GetPropertiesAsync(AdminDetailedPropertiesQueryDto query)
        {
            var period = ResolvePeriod(query);
            if (!period.Success)
                return ServiceResult<AdminDetailedPropertiesResponseDto>.Fail(period.Message!, resultType: period.ResultType);

            var result = await _detailedStatsRepo.GetPropertiesAsync(query, period.Data!.FromUtc, period.Data.ToUtc);
            result.AppliedPeriod = period.Data.ToDto();
            LocalizePropertyItems(result.Properties.Items);
            return ServiceResult<AdminDetailedPropertiesResponseDto>.Ok(result);
        }

        public async Task<ServiceResult<AdminPropertyDetailsDto>> GetPropertyDetailsAsync(long propertyId)
        {
            var result = await _detailedStatsRepo.GetPropertyDetailsAsync(propertyId);
            if (result == null)
            {
                return ServiceResult<AdminPropertyDetailsDto>.Fail("Property not found.", resultType: ServiceResultType.NotFound);
            }

            LocalizePropertyDetails(result);
            return ServiceResult<AdminPropertyDetailsDto>.Ok(result);
        }

        public async Task<ServiceResult<bool>> DeletePropertyAsync(long propertyId)
        {
            var property = await _detailedStatsRepo.GetPropertyForAdminActionAsync(propertyId);
            if (property is null)
                return ServiceResult<bool>.Fail("Property not found.", resultType: ServiceResultType.NotFound);

            if (property.DeletedAt != null)
                return ServiceResult<bool>.Fail("Property is already deleted.", resultType: ServiceResultType.Conflict);

            _logger.LogInformation("Admin requested soft delete for property {PropertyId}", propertyId);
            return await _propertyService.DeletePropertyAsync(propertyId, property.OwnerId, adminInitiated: true);
        }

        public async Task<ServiceResult<AdminDetailedPropertyListItemDto>> RestoreDeletedPropertyAsync(long propertyId)
        {
            var property = await _detailedStatsRepo.GetPropertyForAdminActionAsync(propertyId);
            if (property is null)
                return ServiceResult<AdminDetailedPropertyListItemDto>.Fail("Property not found.", resultType: ServiceResultType.NotFound);

            if (property.DeletedAt == null)
            {
                return ServiceResult<AdminDetailedPropertyListItemDto>.Fail(
                    "Only deleted properties can be restored.",
                    resultType: ServiceResultType.Conflict);
            }

            var imagesWereDeleted = property.DeletedAt.Value <= DateTime.UtcNow.Subtract(ImageRestoreGracePeriod);

            if (!imagesWereDeleted && !string.IsNullOrWhiteSpace(property.ImagesDeletionJob))
            {
                BackgroundJob.Delete(property.ImagesDeletionJob);
            }

            property.DeletedAt = null;
            property.ImagesDeletionJob = null;

            if (imagesWereDeleted)
            {
                property.ProofOfOwnership = null;
                await _propertyRepo.DeleteMediaByPropertyIdsAsync([propertyId]);

                if (property.Status == PropertyStatus.Verified)
                {
                    property.Status = PropertyStatus.Pending;
                }
            }

            await _detailedStatsRepo.SaveAdminContractChangesAsync();
            await _externalPropertyAiClient.NotifyPropertyAddedAsync(propertyId);
            await NotifyDeletedPropertyRestoredAsync(property, imagesWereDeleted);

            _logger.LogInformation(
                "Admin restored deleted property {PropertyId}. Images retained: {ImagesRetained}. Current status: {Status}",
                propertyId,
                !imagesWereDeleted,
                property.Status);

            return ServiceResult<AdminDetailedPropertyListItemDto>.Ok(
                MapProperty(property),
                "Property restored successfully.",
                code: "ZZ_ADMIN_DELETED_PROPERTY_RESTORED_SUCCESSFULLY");
        }

        public async Task<ServiceResult<AdminDetailedContractsResponseDto>> GetContractsAsync(AdminDetailedContractsQueryDto query)
        {
            var period = ResolvePeriod(query);
            if (!period.Success)
                return ServiceResult<AdminDetailedContractsResponseDto>.Fail(period.Message!, resultType: period.ResultType);

            var result = await _detailedStatsRepo.GetContractsAsync(query, period.Data!.FromUtc, period.Data.ToUtc);
            result.AppliedPeriod = period.Data.ToDto();
            LocalizeContractItems(result.Contracts.Items);
            return ServiceResult<AdminDetailedContractsResponseDto>.Ok(result);
        }

        public async Task<ServiceResult<AdminDetailedRevenueResponseDto>> GetRevenueAsync(AdminDetailedRevenueQueryDto query)
        {
            var period = ResolvePeriod(query);
            if (!period.Success)
                return ServiceResult<AdminDetailedRevenueResponseDto>.Fail(period.Message!, resultType: period.ResultType);

            var result = await _detailedStatsRepo.GetRevenueAsync(query, period.Data!.FromUtc, period.Data.ToUtc, period.Data.GroupByDay);
            result.AppliedPeriod = period.Data.ToDto();
            return ServiceResult<AdminDetailedRevenueResponseDto>.Ok(result);
        }

        public async Task<ServiceResult<AdminDetailedContractListItemDto>> CancelContractAsync(long contractId)
        {
            var contract = await _detailedStatsRepo.GetContractForAdminActionAsync(contractId);
            if (contract is null)
                return ServiceResult<AdminDetailedContractListItemDto>.Fail("Contract not found.", resultType: ServiceResultType.NotFound);

            if (contract.Status != ContractStatus.Pending && contract.Status != ContractStatus.Active)
            {
                return ServiceResult<AdminDetailedContractListItemDto>.Fail(
                    "Only pending or active contracts can be cancelled by admin.",
                    resultType: ServiceResultType.Conflict);
            }

            var cancelIssuedIntentsResult = await CancelIssuedPaymentIntentsAsync(contract);
            if (!cancelIssuedIntentsResult.Success)
            {
                return ServiceResult<AdminDetailedContractListItemDto>.Fail(
                    cancelIssuedIntentsResult.Message!,
                    resultType: cancelIssuedIntentsResult.ResultType);
            }

            contract.Status = ContractStatus.Cancelled;

            foreach (var schedule in contract.PaymentSchedules.Where(IsUnpaidSchedule))
            {
                schedule.Status = PaymentScheduleStatus.Cancelled;
                schedule.PaymentIntentId = null;
            }

            await _detailedStatsRepo.SaveAdminContractChangesAsync();

            await NotifyContractCancelledAsync(contract);

            return ServiceResult<AdminDetailedContractListItemDto>.Ok(
                MapContract(contract),
                "Contract cancelled successfully.",
                code: "ZZ_ADMIN_CONTRACT_CANCELLED_SUCCESSFULLY");
        }

        private ServiceResult<ResolvedPeriod> ResolvePeriod(AdminDetailedStatsPeriodQueryDto query)
        {
            NormalizePaging(query);

            var nowUtc = DateTime.UtcNow;
            var period = (query.Period ?? "allTime").Trim();

            if (period.Equals("allTime", StringComparison.OrdinalIgnoreCase))
            {
                return ServiceResult<ResolvedPeriod>.Ok(new ResolvedPeriod(period, null, null, false));
            }

            if (period.Equals("thisMonth", StringComparison.OrdinalIgnoreCase))
            {
                var fromUtc = new DateTime(nowUtc.Year, nowUtc.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                return ServiceResult<ResolvedPeriod>.Ok(new ResolvedPeriod(period, fromUtc, nowUtc, true));
            }

            if (period.Equals("thisYear", StringComparison.OrdinalIgnoreCase))
            {
                var fromUtc = new DateTime(nowUtc.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return ServiceResult<ResolvedPeriod>.Ok(new ResolvedPeriod(period, fromUtc, nowUtc, false));
            }

            if (period.Equals("custom", StringComparison.OrdinalIgnoreCase))
            {
                if (!query.FromUtc.HasValue || !query.ToUtc.HasValue)
                    return ServiceResult<ResolvedPeriod>.Fail("Custom period requires fromUtc and toUtc.", resultType: ServiceResultType.BadRequest);

                if (query.FromUtc.Value >= query.ToUtc.Value)
                    return ServiceResult<ResolvedPeriod>.Fail("fromUtc must be earlier than toUtc.", resultType: ServiceResultType.BadRequest);

                var duration = query.ToUtc.Value - query.FromUtc.Value;
                var useDayGrouping = duration.TotalDays <= 31;
                return ServiceResult<ResolvedPeriod>.Ok(new ResolvedPeriod(period, query.FromUtc.Value, query.ToUtc.Value, useDayGrouping));
            }

            return ServiceResult<ResolvedPeriod>.Fail(
                "Invalid period. Supported values are allTime, thisMonth, thisYear, and custom.",
                resultType: ServiceResultType.BadRequest);
        }

        private static void NormalizePaging(AdminDetailedStatsPeriodQueryDto query)
        {
            if (query.PageNumber < 1)
                query.PageNumber = 1;

            if (query.PageSize < 1)
                query.PageSize = 20;

            if (query.PageSize > MaxPageSize)
                query.PageSize = MaxPageSize;
        }

        private static bool IsUnpaidSchedule(PaymentSchedule schedule)
        {
            return schedule.Status != PaymentScheduleStatus.PaidEarly &&
                   schedule.Status != PaymentScheduleStatus.PaidOnTime &&
                   schedule.Status != PaymentScheduleStatus.PaidLate;
        }

        private async Task NotifyContractCancelledAsync(Contract contract)
        {
            await _notificationService.SendNotificationAsync(new NotificationRequestDto
            {
                UserId = contract.RenterId.ToString(),
                UserType = NotificationUserType.Renter,
                Type = NotificationType.ContractCanceled,
                TitleKey = "NOTIFICATION_CONTRACT_CANCELLED_TITLE",
                BodyKey = "NOTIFICATION_ADMIN_CONTRACT_CANCELLED_BODY",
                LocalizationArguments = new() { contract.Id.ToString(), contract.Property.Title },
                Title = "Contract Cancelled",
                Body = $"An admin has cancelled contract #{contract.Id} for \"{contract.Property.Title}\".",
                ActionType = NotificationActionType.RenterDashboard
            });

            await _notificationService.SendNotificationAsync(new NotificationRequestDto
            {
                UserId = contract.Property.OwnerId.ToString(),
                UserType = NotificationUserType.Owner,
                Type = NotificationType.ContractCanceled,
                TitleKey = "NOTIFICATION_CONTRACT_CANCELLED_TITLE",
                BodyKey = "NOTIFICATION_ADMIN_CONTRACT_CANCELLED_BODY",
                LocalizationArguments = new() { contract.Id.ToString(), contract.Property.Title },
                Title = "Contract Cancelled",
                Body = $"An admin has cancelled contract #{contract.Id} for \"{contract.Property.Title}\".",
                ActionType = NotificationActionType.OwnerDashboard
            });
        }

        private async Task NotifyDeletedPropertyRestoredAsync(Property property, bool imagesWereDeleted)
        {
            await _notificationService.SendNotificationAsync(new NotificationRequestDto
            {
                UserId = property.OwnerId.ToString(),
                UserType = NotificationUserType.Owner,
                Type = NotificationType.General,
                TitleKey = "NOTIFICATION_ADMIN_PROPERTY_RESTORED_TITLE",
                BodyKey = imagesWereDeleted
                    ? "NOTIFICATION_ADMIN_PROPERTY_RESTORED_REVERIFY_BODY"
                    : "NOTIFICATION_ADMIN_PROPERTY_RESTORED_BODY",
                LocalizationArguments = new() { property.Title },
                Title = "Property Restored",
                Body = imagesWereDeleted
                    ? $"An admin has restored your property \"{property.Title}\". Its ownership files were already removed during the deletion grace period, so it may need to go through verification again."
                    : $"An admin has restored your deleted property \"{property.Title}\". It is available in your account again.",
                ActionType = NotificationActionType.Property,
                ActionId = property.Id.ToString()
            });
        }

        private async Task<ServiceResult<bool>> CancelIssuedPaymentIntentsAsync(Contract contract)
        {
            var paymentIntentService = new PaymentIntentService();

            foreach (var schedule in contract.PaymentSchedules.Where(IsUnpaidSchedule))
            {
                if (string.IsNullOrWhiteSpace(schedule.PaymentIntentId))
                    continue;

                try
                {
                    var intent = await paymentIntentService.GetAsync(schedule.PaymentIntentId);

                    if (intent.Status == "succeeded")
                    {
                        _logger.LogWarning(
                            "Admin contract cancellation blocked because payment intent {PaymentIntentId} already succeeded for contract {ContractId}",
                            schedule.PaymentIntentId,
                            contract.Id);

                        return ServiceResult<bool>.Fail(
                            $"Cannot cancel contract #{contract.Id} because payment intent {schedule.PaymentIntentId} has already succeeded. Refresh payment state and try again.",
                            resultType: ServiceResultType.Conflict);
                    }

                    if (intent.Status != "canceled")
                    {
                        await paymentIntentService.CancelAsync(schedule.PaymentIntentId);
                    }
                }
                catch (StripeException ex)
                {
                    _logger.LogWarning(
                        ex,
                        "Failed to cancel Stripe payment intent {PaymentIntentId} while admin was cancelling contract {ContractId}",
                        schedule.PaymentIntentId,
                        contract.Id);

                    return ServiceResult<bool>.Fail(
                        $"Could not cancel Stripe payment intent {schedule.PaymentIntentId}. Contract cancellation was stopped so no live payments are left behind.",
                        resultType: ServiceResultType.Conflict);
                }
            }

            return ServiceResult<bool>.Ok(true, code: "ZZ_ADMIN_CONTRACT_PAYMENT_INTENTS_CLEARED_FOR_CANCELLATION");
        }

        private AdminDetailedContractListItemDto MapContract(Contract contract)
        {
            return new AdminDetailedContractListItemDto
            {
                ContractId = contract.Id,
                Status = contract.Status,
                StatusDisplayName = _localizer.GetEnumDisplayName(contract.Status),
                TransactionId = contract.TransactionId,
                MerkleRoot = contract.MerkleRoot,
                AnchoringStatus = contract.AnchoringStatus,
                AnchoringStatusDisplayName = _localizer.GetEnumDisplayName(contract.AnchoringStatus),
                IsAnchoredToBlockChain = contract.AnchoringStatus == ContractAnchoringStatus.Anchored,
                CanCancel = contract.Status == ContractStatus.Pending || contract.Status == ContractStatus.Active,
                CreatedAt = contract.CreatedAt,
                LeaseStartDate = contract.LeaseStartDate,
                LeaseEndDate = contract.LeaseEndDate,
                TotalContractAmount = contract.TotalContractAmount,
                PaymentFrequency = contract.PaymentFrequency.ToString(),
                PaymentFrequencyDisplayName = _localizer.GetEnumDisplayName(contract.PaymentFrequency),
                PropertyId = contract.PropertyId,
                PropertyTitle = contract.Property.Title,
                OwnerId = contract.Property.OwnerId,
                OwnerName = $"{contract.Property.Owner.FirstName} {contract.Property.Owner.LastName}".Trim(),
                RenterId = contract.RenterId,
                RenterName = $"{contract.Renter.FirstName} {contract.Renter.LastName}".Trim()
            };
        }

        private AdminDetailedPropertyListItemDto MapProperty(Property property)
        {
            return new AdminDetailedPropertyListItemDto
            {
                PropertyId = property.Id,
                Title = property.Title,
                OwnerId = property.OwnerId,
                OwnerName = $"{property.Owner.FirstName} {property.Owner.LastName}".Trim(),
                Status = property.Status,
                StatusDisplayName = _localizer.GetEnumDisplayName(property.Status),
                Type = property.Type,
                TypeDisplayName = _localizer.GetEnumDisplayName(property.Type),
                City = property.City,
                CityDisplayName = GetLocationDisplayName<City>(property.City),
                Governorate = property.State,
                GovernorateDisplayName = GetLocationDisplayName<Governorate>(property.State),
                Price = property.Price,
                AverageRating = property.PropertyFeedbacks.Any()
                    ? (float)Math.Round((double)(property.PropertyFeedbacks.Average(feedback => (float?)feedback.Rating) ?? 0f), 1)
                    : 0f,
                CommentsCount = property.PropertyFeedbacks.Count(feedback => !feedback.IsHiddenByModeration && !string.IsNullOrWhiteSpace(feedback.Content)),
                IsActive = property.IsActive,
                CanDeactivate = property.IsActive && property.DeletedAt == null,
                CanRestore = false,
                IsDeleted = property.DeletedAt != null,
                CreatedAt = property.CreatedAt
            };
        }

        private void LocalizePropertyItems(IEnumerable<AdminDetailedPropertyListItemDto> items)
        {
            foreach (var item in items)
            {
                item.StatusDisplayName = _localizer.GetEnumDisplayName(item.Status);
                item.TypeDisplayName = _localizer.GetEnumDisplayName(item.Type);
                item.CityDisplayName = GetLocationDisplayName<City>(item.City);
                item.GovernorateDisplayName = GetLocationDisplayName<Governorate>(item.Governorate);
            }
        }

        private void LocalizePropertyDetails(AdminPropertyDetailsDto property)
        {
            property.StatusDisplayName = _localizer.GetEnumDisplayName(property.Status);
            property.TypeDisplayName = _localizer.GetEnumDisplayName(property.Type);
            property.RentalUnitDisplayName = _localizer.GetEnumDisplayName(property.RentalUnit);
            property.CityDisplayName = GetLocationDisplayName<City>(property.City);
            property.GovernorateDisplayName = GetLocationDisplayName<Governorate>(property.Governorate);

            foreach (var amenity in property.Amenities)
            {
                amenity.AmenityDisplayName = _localizer.GetEnumDisplayName(amenity.Amenity);
            }

            foreach (var contract in property.Contracts)
            {
                contract.StatusDisplayName = _localizer.GetEnumDisplayName(contract.Status);
                contract.AnchoringStatusDisplayName = _localizer.GetEnumDisplayName(contract.AnchoringStatus);
                contract.PaymentFrequencyDisplayName = _localizer.GetEnumDisplayName(contract.PaymentFrequency);
            }

            foreach (var bookingRequest in property.BookingRequests)
            {
                bookingRequest.PaymentFrequencyDisplayName = _localizer.GetEnumDisplayName(bookingRequest.PaymentFrequency);
            }
        }

        private void LocalizeContractItems(IEnumerable<AdminDetailedContractListItemDto> items)
        {
            foreach (var item in items)
            {
                item.StatusDisplayName = _localizer.GetEnumDisplayName(item.Status);
                item.AnchoringStatusDisplayName = _localizer.GetEnumDisplayName(item.AnchoringStatus);

                if (Enum.TryParse<PaymentFrequency>(item.PaymentFrequency, true, out var paymentFrequency))
                {
                    item.PaymentFrequencyDisplayName = _localizer.GetEnumDisplayName(paymentFrequency);
                }
                else
                {
                    item.PaymentFrequencyDisplayName = item.PaymentFrequency;
                }
            }
        }

        private string GetLocationDisplayName<TEnum>(string? rawValue) where TEnum : struct, Enum
        {
            if (!string.IsNullOrWhiteSpace(rawValue) && Enum.TryParse<TEnum>(rawValue, true, out var parsed))
            {
                return _localizer.GetEnumDisplayName(parsed);
            }

            return rawValue ?? string.Empty;
        }

        private sealed class ResolvedPeriod
        {
            public ResolvedPeriod(string period, DateTime? fromUtc, DateTime? toUtc, bool groupByDay)
            {
                Period = period;
                FromUtc = fromUtc;
                ToUtc = toUtc;
                GroupByDay = groupByDay;
            }
            public string Period { get; }
            public DateTime? FromUtc { get; }
            public DateTime? ToUtc { get; }
            public bool GroupByDay { get; }
            public string Grouping => GroupByDay ? "day" : "month";

            public AdminAppliedPeriodDto ToDto()
            {
                return new AdminAppliedPeriodDto
                {
                    Period = Period,
                    FromUtc = FromUtc,
                    ToUtc = ToUtc,
                    Grouping = Grouping
                };
            }
        }
    }
}
