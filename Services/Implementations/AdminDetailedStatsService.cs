using MARN_API.DTOs.Admin;
using MARN_API.Enums;
using MARN_API.Enums.Contract;
using MARN_API.Enums.Notification;
using MARN_API.Enums.Payment;
using MARN_API.DTOs.Notification;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using Stripe;

namespace MARN_API.Services.Implementations
{
    public class AdminDetailedStatsService : IAdminDetailedStatsService
    {
        private const int MaxPageSize = 100;
        private readonly IAdminDetailedStatsRepo _detailedStatsRepo;
        private readonly INotificationService _notificationService;
        private readonly ILogger<AdminDetailedStatsService> _logger;

        public AdminDetailedStatsService(
            IAdminDetailedStatsRepo detailedStatsRepo,
            INotificationService notificationService,
            ILogger<AdminDetailedStatsService> logger)
        {
            _detailedStatsRepo = detailedStatsRepo;
            _notificationService = notificationService;
            _logger = logger;
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
            return ServiceResult<AdminDetailedPropertiesResponseDto>.Ok(result);
        }

        public async Task<ServiceResult<AdminDetailedPropertyListItemDto>> DeactivatePropertyAsync(long propertyId)
        {
            var property = await _detailedStatsRepo.GetPropertyForAdminActionAsync(propertyId);
            if (property is null)
                return ServiceResult<AdminDetailedPropertyListItemDto>.Fail("Property not found.", resultType: ServiceResultType.NotFound);

            if (property.DeletedAt != null)
            {
                return ServiceResult<AdminDetailedPropertyListItemDto>.Fail(
                    "Deleted properties cannot be deactivated.",
                    resultType: ServiceResultType.Conflict);
            }

            if (!property.IsActive)
            {
                return ServiceResult<AdminDetailedPropertyListItemDto>.Fail(
                    "Property is already deactivated.",
                    resultType: ServiceResultType.Conflict);
            }

            property.IsActive = false;
            await _detailedStatsRepo.SaveAdminContractChangesAsync();

            await NotifyPropertyAvailabilityChangedAsync(property, restored: false);

            return ServiceResult<AdminDetailedPropertyListItemDto>.Ok(
                MapProperty(property),
                "Property deactivated successfully.");
        }

        public async Task<ServiceResult<AdminDetailedPropertyListItemDto>> RestorePropertyAsync(long propertyId)
        {
            var property = await _detailedStatsRepo.GetPropertyForAdminActionAsync(propertyId);
            if (property is null)
                return ServiceResult<AdminDetailedPropertyListItemDto>.Fail("Property not found.", resultType: ServiceResultType.NotFound);

            if (property.DeletedAt != null)
            {
                return ServiceResult<AdminDetailedPropertyListItemDto>.Fail(
                    "Deleted properties cannot be restored.",
                    resultType: ServiceResultType.Conflict);
            }

            if (property.IsActive)
            {
                return ServiceResult<AdminDetailedPropertyListItemDto>.Fail(
                    "Property is already active.",
                    resultType: ServiceResultType.Conflict);
            }

            property.IsActive = true;
            await _detailedStatsRepo.SaveAdminContractChangesAsync();

            await NotifyPropertyAvailabilityChangedAsync(property, restored: true);

            return ServiceResult<AdminDetailedPropertyListItemDto>.Ok(
                MapProperty(property),
                "Property restored successfully.");
        }

        public async Task<ServiceResult<AdminDetailedContractsResponseDto>> GetContractsAsync(AdminDetailedContractsQueryDto query)
        {
            var period = ResolvePeriod(query);
            if (!period.Success)
                return ServiceResult<AdminDetailedContractsResponseDto>.Fail(period.Message!, resultType: period.ResultType);

            var result = await _detailedStatsRepo.GetContractsAsync(query, period.Data!.FromUtc, period.Data.ToUtc);
            result.AppliedPeriod = period.Data.ToDto();
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
                "Contract cancelled successfully.");
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
                Title = "Contract Cancelled",
                Body = $"An admin has cancelled contract #{contract.Id} for \"{contract.Property.Title}\".",
                ActionType = NotificationActionType.RenterDashboard
            });

            await _notificationService.SendNotificationAsync(new NotificationRequestDto
            {
                UserId = contract.Property.OwnerId.ToString(),
                UserType = NotificationUserType.Owner,
                Type = NotificationType.ContractCanceled,
                Title = "Contract Cancelled",
                Body = $"An admin has cancelled contract #{contract.Id} for \"{contract.Property.Title}\".",
                ActionType = NotificationActionType.OwnerDashboard
            });
        }

        private async Task NotifyPropertyAvailabilityChangedAsync(Property property, bool restored)
        {
            await _notificationService.SendNotificationAsync(new NotificationRequestDto
            {
                UserId = property.OwnerId.ToString(),
                UserType = NotificationUserType.Owner,
                Type = NotificationType.General,
                Title = restored ? "Property Restored" : "Property Deactivated",
                Body = restored
                    ? $"An admin has restored your property \"{property.Title}\" and made it active again."
                    : $"An admin has deactivated your property \"{property.Title}\". It is no longer publicly available.",
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

            return ServiceResult<bool>.Ok(true);
        }

        private static AdminDetailedContractListItemDto MapContract(Contract contract)
        {
            return new AdminDetailedContractListItemDto
            {
                ContractId = contract.Id,
                Status = contract.Status,
                CanCancel = contract.Status == ContractStatus.Pending || contract.Status == ContractStatus.Active,
                CreatedAt = contract.CreatedAt,
                LeaseStartDate = contract.LeaseStartDate,
                LeaseEndDate = contract.LeaseEndDate,
                TotalContractAmount = contract.TotalContractAmount,
                PaymentFrequency = contract.PaymentFrequency.ToString(),
                PropertyId = contract.PropertyId,
                PropertyTitle = contract.Property.Title,
                OwnerId = contract.Property.OwnerId,
                OwnerName = $"{contract.Property.Owner.FirstName} {contract.Property.Owner.LastName}".Trim(),
                RenterId = contract.RenterId,
                RenterName = $"{contract.Renter.FirstName} {contract.Renter.LastName}".Trim()
            };
        }

        private static AdminDetailedPropertyListItemDto MapProperty(Property property)
        {
            return new AdminDetailedPropertyListItemDto
            {
                PropertyId = property.Id,
                Title = property.Title,
                OwnerId = property.OwnerId,
                OwnerName = $"{property.Owner.FirstName} {property.Owner.LastName}".Trim(),
                Status = property.Status,
                Type = property.Type,
                City = property.City,
                State = property.State,
                Price = property.Price,
                IsActive = property.IsActive,
                CanDeactivate = property.IsActive && property.DeletedAt == null,
                CanRestore = !property.IsActive && property.DeletedAt == null,
                IsDeleted = property.DeletedAt != null,
                CreatedAt = property.CreatedAt
            };
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
