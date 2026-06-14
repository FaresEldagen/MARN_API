using MARN_API.DTOs.Admin;
using MARN_API.DTOs.Common;
using MARN_API.DTOs.Notification;
using MARN_API.Enums;
using MARN_API.Enums.Account;
using MARN_API.Enums.Notification;
using MARN_API.Enums.Property;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;

namespace MARN_API.Services.Implementations
{
    public class AdminVerificationService : IAdminVerificationService
    {
        private const int MaxPageSize = 100;
        private readonly IAdminVerificationRepo _verificationRepo;
        private readonly IAppTextLocalizer _localizer;
        private readonly INotificationService _notificationService;
        private readonly ILogger<AdminVerificationService> _logger;

        public AdminVerificationService(
            IAdminVerificationRepo verificationRepo,
            IAppTextLocalizer localizer,
            INotificationService notificationService,
            ILogger<AdminVerificationService> logger)
        {
            _verificationRepo = verificationRepo;
            _localizer = localizer;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<ServiceResult<PagedResult<AdminUserVerificationDto>>> GetPendingUserVerificationsAsync(AdminVerificationQueryDto query)
        {
            var (pageNumber, pageSize) = NormalizePaging(query);
            var result = await _verificationRepo.GetPendingUserVerificationsAsync(pageNumber, pageSize);
            return ServiceResult<PagedResult<AdminUserVerificationDto>>.Ok(result);
        }

        public async Task<ServiceResult<AdminUserVerificationDto>> GetUserVerificationDetailsAsync(Guid userId)
        {
            var user = await _verificationRepo.GetUserVerificationDetailsAsync(userId);
            return user == null
                ? ServiceResult<AdminUserVerificationDto>.Fail("User verification request not found.", resultType: ServiceResultType.NotFound)
                : ServiceResult<AdminUserVerificationDto>.Ok(user);
        }

        public async Task<ServiceResult<bool>> ApproveUserVerificationAsync(Guid userId)
        {
            var user = await _verificationRepo.GetUserForVerificationAsync(userId);
            if (user == null)
                return ServiceResult<bool>.Fail("User verification request not found.", resultType: ServiceResultType.NotFound);

            if (user.AccountStatus != AccountStatus.Pending)
                return ServiceResult<bool>.Fail(
                    "User verification cannot be approved while account status is {0}.",
                    resultType: ServiceResultType.Conflict,
                    code: "USER_VERIFICATION_APPROVAL_STATUS_CONFLICT",
                    messageArguments: [_localizer.GetEnumDisplayName(user.AccountStatus)]);

            user.AccountStatus = AccountStatus.Verified;
            await _verificationRepo.SaveChangesAsync();

            _logger.LogInformation("Admin approved user verification for user {UserId}", userId);
            return ServiceResult<bool>.Ok(true, "User verification approved.", code: "ZZ_ADMIN_USER_VERIFICATION_APPROVED");
        }

        public async Task<ServiceResult<bool>> DeclineUserVerificationAsync(Guid userId, AdminVerificationDecisionDto decision)
        {
            decision ??= new AdminVerificationDecisionDto();

            var user = await _verificationRepo.GetUserForVerificationAsync(userId);
            if (user == null)
                return ServiceResult<bool>.Fail("User verification request not found.", resultType: ServiceResultType.NotFound);

            if (user.AccountStatus != AccountStatus.Pending)
                return ServiceResult<bool>.Fail(
                    "User verification cannot be declined while account status is {0}.",
                    resultType: ServiceResultType.Conflict,
                    code: "USER_VERIFICATION_DECLINE_STATUS_CONFLICT",
                    messageArguments: [_localizer.GetEnumDisplayName(user.AccountStatus)]);

            user.AccountStatus = AccountStatus.Declined;
            await _verificationRepo.SaveChangesAsync();

            _logger.LogInformation("Admin declined user verification for user {UserId}. Reason: {Reason}", userId, decision.Reason);
            return ServiceResult<bool>.Ok(true, "User verification declined.", code: "ZZ_ADMIN_USER_VERIFICATION_DECLINED");
        }

        public async Task<ServiceResult<PagedResult<AdminPropertyVerificationDto>>> GetPendingPropertyVerificationsAsync(AdminVerificationQueryDto query)
        {
            var (pageNumber, pageSize) = NormalizePaging(query);
            var result = await _verificationRepo.GetPendingPropertyVerificationsAsync(pageNumber, pageSize);
            return ServiceResult<PagedResult<AdminPropertyVerificationDto>>.Ok(result);
        }

        public async Task<ServiceResult<AdminPropertyVerificationDto>> GetPropertyVerificationDetailsAsync(long propertyId)
        {
            var property = await _verificationRepo.GetPropertyVerificationDetailsAsync(propertyId);
            return property == null
                ? ServiceResult<AdminPropertyVerificationDto>.Fail("Property verification request not found.", resultType: ServiceResultType.NotFound)
                : ServiceResult<AdminPropertyVerificationDto>.Ok(property);
        }

        public async Task<ServiceResult<bool>> ApprovePropertyVerificationAsync(long propertyId)
        {
            var property = await _verificationRepo.GetPropertyForVerificationAsync(propertyId);
            if (property == null)
                return ServiceResult<bool>.Fail("Property verification request not found.", resultType: ServiceResultType.NotFound);

            if (property.Status != PropertyStatus.Pending)
                return ServiceResult<bool>.Fail(
                    "Property verification cannot be approved while property status is {0}.",
                    resultType: ServiceResultType.Conflict,
                    code: "PROPERTY_VERIFICATION_APPROVAL_STATUS_CONFLICT",
                    messageArguments: [_localizer.GetEnumDisplayName(property.Status)]);

            property.Status = PropertyStatus.Verified;
            await _verificationRepo.SaveChangesAsync();
            await TrySendPropertyAcceptedNotificationAsync(property);

            _logger.LogInformation("Admin approved property verification for property {PropertyId}", propertyId);
            return ServiceResult<bool>.Ok(true, "Property verification approved.", code: "ZZ_ADMIN_PROPERTY_VERIFICATION_APPROVED");
        }

        public async Task<ServiceResult<bool>> DeclinePropertyVerificationAsync(long propertyId, AdminVerificationDecisionDto decision)
        {
            decision ??= new AdminVerificationDecisionDto();

            var property = await _verificationRepo.GetPropertyForVerificationAsync(propertyId);
            if (property == null)
                return ServiceResult<bool>.Fail("Property verification request not found.", resultType: ServiceResultType.NotFound);

            if (property.Status != PropertyStatus.Pending)
                return ServiceResult<bool>.Fail(
                    "Property verification cannot be declined while property status is {0}.",
                    resultType: ServiceResultType.Conflict,
                    code: "PROPERTY_VERIFICATION_DECLINE_STATUS_CONFLICT",
                    messageArguments: [_localizer.GetEnumDisplayName(property.Status)]);

            property.Status = PropertyStatus.Declined;
            await _verificationRepo.SaveChangesAsync();
            await TrySendPropertyRejectedNotificationAsync(property, decision.Reason);

            _logger.LogInformation("Admin declined property verification for property {PropertyId}. Reason: {Reason}", propertyId, decision.Reason);
            return ServiceResult<bool>.Ok(true, "Property verification declined.", code: "ZZ_ADMIN_PROPERTY_VERIFICATION_DECLINED");
        }

        private async Task TrySendPropertyAcceptedNotificationAsync(Property property)
        {
            try
            {
                await _notificationService.SendNotificationAsync(new NotificationRequestDto
                {
                    UserId = property.OwnerId.ToString(),
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PropertyAccepted,
                    TitleKey = "NOTIFICATION_PROPERTY_ACCEPTED_TITLE",
                    BodyKey = "NOTIFICATION_PROPERTY_ACCEPTED_BODY",
                    LocalizationArguments = new() { property.Title },
                    Title = "Property Verification Accepted",
                    Body = $"Your property \"{property.Title}\" has been approved and is now visible to renters.",
                    ActionType = NotificationActionType.Property,
                    ActionId = property.Id.ToString()
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to send property acceptance notification for property {PropertyId}", property.Id);
            }
        }

        private async Task TrySendPropertyRejectedNotificationAsync(Property property, string? reason)
        {
            var trimmedReason = string.IsNullOrWhiteSpace(reason) ? null : reason.Trim();

            try
            {
                await _notificationService.SendNotificationAsync(new NotificationRequestDto
                {
                    UserId = property.OwnerId.ToString(),
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PropertyRejected,
                    TitleKey = "NOTIFICATION_PROPERTY_REJECTED_TITLE",
                    BodyKey = trimmedReason == null
                        ? "NOTIFICATION_PROPERTY_REJECTED_BODY"
                        : "NOTIFICATION_PROPERTY_REJECTED_WITH_REASON_BODY",
                    LocalizationArguments = trimmedReason == null
                        ? new() { property.Title }
                        : new() { property.Title, trimmedReason },
                    Title = "Property Verification Rejected",
                    Body = trimmedReason == null
                        ? $"Your property \"{property.Title}\" was rejected during admin verification. Please review your property details and submit it again."
                        : $"Your property \"{property.Title}\" was rejected during admin verification. Reason: {trimmedReason}",
                    ActionType = NotificationActionType.Property,
                    ActionId = property.Id.ToString()
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to send property rejection notification for property {PropertyId}", property.Id);
            }
        }

        private static (int PageNumber, int PageSize) NormalizePaging(AdminVerificationQueryDto query)
        {
            var pageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;
            var pageSize = query.PageSize < 1 ? 20 : query.PageSize;
            pageSize = Math.Min(pageSize, MaxPageSize);

            return (pageNumber, pageSize);
        }
    }
}
