using AutoMapper;
using MARN_API.Data;
using MARN_API.DTOs.Dashboard;
using MARN_API.DTOs.Notification;
using MARN_API.DTOs.Profile;
using MARN_API.DTOs.Property;
using MARN_API.Enums;
using MARN_API.Enums.Account;
using MARN_API.Enums.Notification;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Threading.Channels;

namespace MARN_API.Services.Implementations
{
    public class ProfileService : IProfileService
    {
        private readonly IBookingRequestRepo _bookingRequestRepo;
        private readonly IContractRepo _contractRepo;
        private readonly INotificationRepo _notificationRepo;
        private readonly IPaymentRepo _paymentRepo;
        private readonly IPropertyRepo _propertyRepo;
        private readonly IRoommatePreferenceRepo _roommatePreferenceRepo;
        private readonly ISavedPropertyRepo _savedPropertyRepo;
        private readonly IReportRepo _reportRepo;
        private readonly IPropertyRatingRepo _propertyRatingRepo;
        private readonly IPropertyCommentRepo _propertyCommentRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dbContext;
        private readonly IFileService _fileService;
        private readonly IEmailService _emailService;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountService> _logger;
        private readonly IPropertyService _propertyService;

        public ProfileService(
            IBookingRequestRepo bookingRequestRepo,
            IContractRepo contractRepo,
            INotificationRepo notificationRepo,
            IPaymentRepo paymentRepo,
            IPropertyRepo propertyRepo,
            IRoommatePreferenceRepo roommatePreferenceRepo,
            ISavedPropertyRepo savedPropertyRepo,
            IReportRepo reportRepo,
            IPropertyRatingRepo propertyRatingRepo,
            IPropertyCommentRepo propertyCommentRepo,
            UserManager<ApplicationUser> userManager,
            AppDbContext dbContext,
            INotificationService notificationService,
            IFileService fileService,
            IEmailService emailService,
            IMapper mapper,
            ILogger<AccountService> logger,
            IPropertyService propertyService
        )
        {
            _bookingRequestRepo = bookingRequestRepo;
            _contractRepo = contractRepo;
            _notificationRepo = notificationRepo;
            _paymentRepo = paymentRepo;
            _propertyRepo = propertyRepo;
            _roommatePreferenceRepo = roommatePreferenceRepo;
            _savedPropertyRepo = savedPropertyRepo;
            _reportRepo = reportRepo;
            _propertyRatingRepo = propertyRatingRepo;
            _propertyCommentRepo = propertyCommentRepo;
            _userManager = userManager;
            _dbContext = dbContext;
            _notificationService = notificationService;
            _fileService = fileService;
            _emailService = emailService;
            _mapper = mapper;
            _logger = logger;
            _propertyService = propertyService;
        }


        #region Profile and Dashboards
        public async Task<ServiceResult<RenterDashboardDto>> RenterDashboardAsync(Guid userId)
        {
            _logger.LogInformation("Get Renter Dashboard Data attempt for userId: {userId}", userId);

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                _logger.LogWarning("Get Renter Dashboard Data failed: User not found for userId: {userId}", userId);
                return ServiceResult<RenterDashboardDto>.Fail("User not found", resultType: ServiceResultType.Unauthorized);
            }

            var activeRentals = await _contractRepo.GetActiveRentals(userId);
            var activeRentalsCount = activeRentals == null ? 0 : activeRentals.Count;

            var pendingBookingRequests = await _bookingRequestRepo.GetRenterPendingRequests(userId);

            var nextPayment = await _paymentRepo.GetNextPayment(userId);

            var savedProperties = await _savedPropertyRepo.GetSavedProperties(userId);
            var savedPropertiesCount = savedProperties == null ? 0 : savedProperties.Count;

            List<PropertyCardDto>? recommendations = null;

            var notifications = await _notificationRepo.GetRenterDashboardNotifications(userId);
            var unreadNotificationsCount = notifications == null ? 0 : notifications.Count(n => !n.IsRead);
            
            var accountSatus = user.AccountStatus;

            var allContracts = await _contractRepo.GetRenterContracts(userId);

            var dashboardData = new RenterDashboardDto
            {
                ActiveRentals = activeRentals,
                ActiveRentalsCount = activeRentalsCount,

                PendingBookingRequests = pendingBookingRequests,

                NextPayment = nextPayment,

                Notifications = notifications,
                UnreadNotificationsCount = unreadNotificationsCount,

                SavedProperties = savedProperties,
                SavedPropertiesCount = savedPropertiesCount,

                Recommendations = recommendations,

                AccountStatus = accountSatus,
                
                AllContracts = allContracts
            };

            _logger.LogInformation("Get Renter Dashboard Data successful for userId: {userId}", userId);
            return ServiceResult<RenterDashboardDto>.Ok(dashboardData);
        }

        public async Task<ServiceResult<OwnerDashboardDto>> OwnerDashboardAsync(Guid userId)
        {
            _logger.LogInformation("Get Owner Dashboard Data attempt for userId: {userId}", userId);

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                _logger.LogWarning("Get Owner Dashboard Data failed: User not found for userId: {userId}", userId);
                return ServiceResult<OwnerDashboardDto>.Fail("User not found", resultType: ServiceResultType.Unauthorized);
            }

            var properties = await _propertyRepo.GetOwnerDashboardProperties(userId);
            var propertiesCount = properties == null ? 0 : properties.Count;

            var occupiedPlacesCount = await _contractRepo.GetOwnedPropertiesOccupiedPlacesCount(userId);
            var vacantPlacesCount = await _propertyRepo.GetOwnedPropertiesPlacesCount(userId) - occupiedPlacesCount;
            var totalViews = await _propertyRepo.GetOwnedPropertiesViewsCount(userId);

            var monthlyEearnings = await _paymentRepo.GetEarningOverviewMonthly(userId);
            var yearlyEarnings = await _paymentRepo.GetEarningOverviewYearly(userId);
            var withdrawableEarnings = await _paymentRepo.GetWithdrawableEarnings(userId);
            var onHoldEarnings = await _paymentRepo.GetOnHoldEarnings(userId);

            var averageRating = await _propertyRepo.GetOwnerAverageRating(userId);
            var ratingsCount = await _propertyRepo.GetOwnerRatingsCount(userId);

            var allContracts = await _contractRepo.GetOwnerContracts(userId);

            var notifications = await _notificationRepo.GetOwnerDashboardNotifications(userId);
            var unreadNotificationsCount = notifications == null ? 0 : notifications.Count(n => !n.IsRead);

            var pendingBookingRequests = await _bookingRequestRepo.GetOwnerPendingRequests(userId);
            var pendingBookingRequestsCount = pendingBookingRequests == null ? 0 : pendingBookingRequests.Count;

            var accountSatus = user.AccountStatus;

            var dashboardData = new OwnerDashboardDto
            {
                Properties = properties,
                PropertiesCount = propertiesCount,

                OccupiedPlaces = occupiedPlacesCount,
                VacantPlaces = vacantPlacesCount,
                TotalViews = totalViews,

                MonthlyEarning = monthlyEearnings,
                YearlyEarning = yearlyEarnings,
                WithdrawableEarnings = withdrawableEarnings,
                OnHoldEarnings = onHoldEarnings,

                AverageRating = averageRating,
                RatingsCount = ratingsCount,

                AllContracts = allContracts,

                Notifications = notifications,
                UnreadNotificationsCount = unreadNotificationsCount,

                PendingBookingRequests = pendingBookingRequests,
                PendingBookingRequestsCount = pendingBookingRequestsCount,

                AccountStatus = accountSatus,
            };

            _logger.LogInformation("Get Owner Dashboard Data successful for userId: {userId}", userId);
            return ServiceResult<OwnerDashboardDto>.Ok(dashboardData);
        }

        public async Task<ServiceResult<ProfileDto>> GetProfileAsync(Guid userId)
        {
            _logger.LogInformation("Get Profile Data attempt for userId: {userId}", userId);

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                _logger.LogWarning("Get Profile Data failed: User not found for userId: {userId}", userId);
                return ServiceResult<ProfileDto>.Fail("User not found", resultType: ServiceResultType.BadRequest);
            }

            var profileData = _mapper.Map<ProfileDto>(user);

            var isOwner = await _userManager.IsInRoleAsync(user, "Owner");
            profileData.IsOwner = isOwner;

            if (isOwner)
            {
                var averageRating = await _propertyRepo.GetOwnerAverageRating(userId);
                var ratingsCount = await _propertyRepo.GetOwnerRatingsCount(userId);
                var ownedProperties = await _propertyRepo.GetOwnerProfileProperties(userId);
                var ownedPropertiesCount = ownedProperties == null ? 0 : ownedProperties.Count;

                profileData.AverageRating = averageRating;
                profileData.RatingsCount = ratingsCount;
                profileData.OwnedProperties = ownedProperties;
                profileData.OwnedPropertiesCount = ownedProperties?.Count ?? 0;
            }

            var RoommatePreferences = await _roommatePreferenceRepo.GetRoommatePreferences(userId);

            if (RoommatePreferences != null)
            {
                _mapper.Map(RoommatePreferences, profileData);
                profileData.RoommatePreferencesEnabled = true;
            }

            _logger.LogInformation("Get Profile Data successful for userId: {userId}", userId);
            return ServiceResult<ProfileDto>.Ok(profileData);
        }
        #endregion


        #region Profile Settings
        public async Task<ServiceResult<ProfileSettingsDto>> GetProfileSettingsAsync(Guid userId)
        {
            _logger.LogInformation("Get Profile Settings Data attempt for userId: {userId}", userId);

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                _logger.LogWarning("Get Profile Settings Data failed: User not found for userId: {userId}", userId);
                return ServiceResult<ProfileSettingsDto>.Fail("User not found", resultType: ServiceResultType.BadRequest);
            }

            var profileData = _mapper.Map<ProfileSettingsDto>(user);

            var RoommatePreferences = await _roommatePreferenceRepo.GetRoommatePreferences(userId);

            if (RoommatePreferences != null)
            {
                _mapper.Map(RoommatePreferences, profileData);
            }

            _logger.LogInformation("Get Profile Settings Data successful for userId: {userId}", userId);
            return ServiceResult<ProfileSettingsDto>.Ok(profileData);
        }

        public async Task<ServiceResult<bool>> UpdateProfileBasicDataAsync(UpdateProfileDto dto)
        {
            _logger.LogInformation("Update Profile Data attempt for userId: {userId}", dto.Id);

            var user = await _userManager.FindByIdAsync(dto.Id.ToString());

            if (user == null)
            {
                _logger.LogWarning("Update Profile Data failed: User not found for userId: {userId}", dto.Id);
                return ServiceResult<bool>.Fail("User not found", resultType: ServiceResultType.BadRequest);
            }

            if (dto.ProfileImage != null)
            {
                var validationError = ValidateImage(dto.ProfileImage);
                if (validationError != null)
                    return validationError;

                var newImageUrl = await _fileService.SaveImageAsync(dto.ProfileImage, "profiles");

                if (newImageUrl == null)
                    return ServiceResult<bool>.Fail("Failed to upload image");

                _fileService.DeleteImage(user.ProfileImage);

                user.ProfileImage = newImageUrl;
            }

            // Store original values before mapping to check if critical identity fields changed
            var originalFirstName = user.FirstName;
            var originalLastName = user.LastName;
            var originalDateOfBirth = user.DateOfBirth;
            var originalPhoneNumber = user.PhoneNumber;
            var originalGender = user.Gender;
            var originalCountry = user.Country;

            user = _mapper.Map(dto, user);

            // Only reset verification if critical identity fields changed
            if (user.FirstName != originalFirstName ||
                user.LastName != originalLastName ||
                user.DateOfBirth != originalDateOfBirth ||
                user.PhoneNumber != originalPhoneNumber ||
                user.Gender != originalGender ||
                user.Country != originalCountry)
            {
                user.AccountStatus = AccountStatus.Pending;
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                _logger.LogError(
                    "Update Profile Data failed for userId: {userId}, Errors: {@Errors}",
                    dto.Id,
                    result.Errors.Select(e => e.Description)
                );
                return ServiceResult<bool>.Fail(
                    "Update Profile Data failed.",
                    result.Errors.Select(e => e.Description).ToList(),
                    resultType: ServiceResultType.BadRequest
                );
            }

            _logger.LogInformation("Update Profile Data successful for user: {UserId}", user.Id);

            if(user.AccountStatus == AccountStatus.Pending)
            {
                await _notificationService.SendNotificationAsync(new NotificationRequestDto
                {
                    UserId = user.Id.ToString(),
                    UserType = NotificationUserType.General,
                    Type = NotificationType.General,

                    Title = "Profile Updated Successfully!",
                    Body = "Your profile has been updated successfully. Our team will review your information, and your account is expected to be verified within approximately 24 hours.\n\n" +
                        "Once verified, you’ll be able to start renting properties, listing your own, and connecting with compatible roommates.",
                });
            }
            return ServiceResult<bool>.Ok(true, "Update Profile Data successful.");
        }

        public async Task<ServiceResult<bool>> UpdateProfileLegalDataAsync(UpdateLegalDto dto) 
        {
            _logger.LogInformation("Update Profile Legal Data attempt for userId: {userId}", dto.Id);

            var user = await _userManager.FindByIdAsync(dto.Id.ToString());

            if (user == null)
            {
                _logger.LogWarning("Update Legal Profile Data failed: User not found for userId: {userId}", dto.Id);
                return ServiceResult<bool>.Fail("User not found", resultType: ServiceResultType.BadRequest);
            }

            //if (user.AccountStatus == AccountStatus.Verified)
            //{
            //    _logger.LogWarning("Update Legal Profile Data failed: User is already verified for userId: {userId}", dto.Id);
            //    return ServiceResult<bool>.Fail("User is already verified", resultType: ServiceResultType.BadRequest);
            //}

            if (dto.FrontIdPhoto != null)
            {
                var frontValidationError = ValidateImage(dto.FrontIdPhoto);
                if (frontValidationError != null)
                    return frontValidationError;

                var newImageUrl = await _fileService.SaveImageAsync(dto.FrontIdPhoto, "idCards");

                if (newImageUrl == null)
                    return ServiceResult<bool>.Fail("Failed to upload image");

                _fileService.DeleteImage(user.FrontIdPhoto);

                user.FrontIdPhoto = newImageUrl;
            }

            if (dto.BackIdPhoto != null)
            {
                var backValidationError = ValidateImage(dto.BackIdPhoto);
                if (backValidationError != null)
                    return backValidationError;

                var newImageUrl = await _fileService.SaveImageAsync(dto.BackIdPhoto, "idCards");

                if (newImageUrl == null)
                    return ServiceResult<bool>.Fail("Failed to upload image");

                _fileService.DeleteImage(user.BackIdPhoto);

                user.BackIdPhoto = newImageUrl;
            }

            user = _mapper.Map(dto, user);
            user.AccountStatus = AccountStatus.Pending;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                _logger.LogError(
                    "Update Profile Legal Data failed for userId: {userId}, Errors: {@Errors}",
                    dto.Id,
                    result.Errors.Select(e => e.Description)
                );
                return ServiceResult<bool>.Fail(
                    "Update Profile Data failed.",
                    result.Errors.Select(e => e.Description).ToList(),
                    resultType: ServiceResultType.BadRequest
                );
            }

            _logger.LogInformation("Update Profile Legal Data successful for user: {UserId}", user.Id);

            await _notificationService.SendNotificationAsync(new NotificationRequestDto
            {
                UserId = user.Id.ToString(),
                UserType = NotificationUserType.General,
                Type = NotificationType.General,

                Title = "Profile Updated Successfully!",
                Body = "Your profile has been updated successfully. Our team will review your information, and your account is expected to be verified within approximately 24 hours.\n\n" +
                    "Once verified, you’ll be able to start renting properties, listing your own, and connecting with compatible roommates.",
            });
            return ServiceResult<bool>.Ok(true, "Update Profile Data successful.");
        }

        public async Task<ServiceResult<bool>> UpdateProfileRoommatePreferencesDataAsync(UpdateRoommatePreferencesDto dto)
        {
            _logger.LogInformation("Update Roommate Preferences Data attempt for userId: {userId}", dto.UserId);

            var user = await _userManager.FindByIdAsync(dto.UserId.ToString());

            if (user == null)
            {
                _logger.LogWarning("Update Roommate Preferences Data failed: User not found for userId: {userId}", dto.UserId);
                return ServiceResult<bool>.Fail("User not found", resultType: ServiceResultType.BadRequest);
            }

            var RoommatePreferences = await _roommatePreferenceRepo.GetRoommatePreferences(dto.UserId);

            try
            {
                if (RoommatePreferences != null)
                {
                    RoommatePreferences = _mapper.Map(dto, RoommatePreferences);
                    var roommate_result = await _roommatePreferenceRepo.UpdateRoommatePreferences(RoommatePreferences);
                }
                else
                {
                    RoommatePreferences = _mapper.Map<RoommatePreference>(dto);
                    RoommatePreferences.UserId = dto.UserId;
                    var roommate_result = await _roommatePreferenceRepo.CreateRoommatePreferences(RoommatePreferences);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Update Profile Data failed for userId: {userId}, Errors: Exception occurred while saving roommate preferences. Exception: {Exception}",
                    dto.UserId,
                    ex
                );
                return ServiceResult<bool>.Fail(
                    "Update Profile Data failed. An error occurred while saving roommate preferences.",
                    resultType: ServiceResultType.BadRequest
                );
            }

            _logger.LogInformation("Update Roommate Preferences Data successful for user: {UserId}", user.Id);
            return ServiceResult<bool>.Ok(true, "Update Roommate Preferences Data successful.");
        }

        public async Task<ServiceResult<bool>> ToggleTwoFactorAsync(Guid userId, string? password = null)
        {
            _logger.LogInformation("Toggle2FA attempt for userId: {userId}", userId);

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                _logger.LogWarning("Toggle2FA failed: User not found for userId: {userId}", userId);
                return ServiceResult<bool>.Fail("User not found");
            }

            // Optional: verify password before allowing toggle
            if (!user.TwoFactorEnabled)
            {
                if (string.IsNullOrEmpty(password))
                {
                    _logger.LogWarning("Toggle2FA failed: Invalid password for user: {UserId}", user.Id);
                    return ServiceResult<bool>.Fail("Invalid password");
                }
                else
                {
                    bool CheckPassword = await _userManager.CheckPasswordAsync(user, password);
                    if (!CheckPassword)
                    {
                        _logger.LogWarning("Toggle2FA failed: Invalid password for user: {UserId}", user.Id);
                        return ServiceResult<bool>.Fail("Invalid password");
                    }
                }

            }

            bool newState = !user.TwoFactorEnabled;

            var result = await _userManager.SetTwoFactorEnabledAsync(user, newState);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Toggle2FA failed: Failed to Update the database for user: {UserId}", user.Id);
                return ServiceResult<bool>.Fail("Failed to toggle 2FA", result.Errors.Select(e => e.Description).ToList());
            }

            _logger.LogInformation("User {UserId} toggled 2FA. Enabled={Enabled}", user.Id, newState);
            return ServiceResult<bool>.Ok(newState, $"Two-Factor Authentication is now {(newState ? "enabled" : "disabled")}");
        }

        public async Task<ServiceResult<bool>> ChangePasswordAsync(ChangePasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.id.ToString());
            if (user == null)
            {
                _logger.LogWarning("Change Password failed: User not found for userId: {userId}", dto.id);
                return ServiceResult<bool>.Fail("User not found");
            }

            if (!await _userManager.CheckPasswordAsync(user, dto.CurrentPassword))
            {
                _logger.LogWarning("Change Password failed: Current password is incorrect for userId: {userId}", dto.id);
                return ServiceResult<bool>.Fail("Current password is incorrect");
            }

            var result = await _userManager.ChangePasswordAsync(
                user,
                dto.CurrentPassword,
                dto.NewPassword);

            if (!result.Succeeded)
            {
                _logger.LogError(
                    "Change Password failed for userId: {userId}, Errors: {@Errors}",
                    dto.id,
                    result.Errors.Select(e => e.Description)
                );
                return ServiceResult<bool>.Fail(
                    "Change Password failed. An error occurred while Changing the password.",
                    resultType: ServiceResultType.BadRequest
                );
            }

            _logger.LogInformation("Password Changed successfully for user: {UserId}", dto.id);
            return ServiceResult<bool>.Ok(true, "Password Changed successfully.");
        }

        public async Task<ServiceResult<bool>> DeleteUserAsync(Guid userId)
        {
            _logger.LogInformation("Delete User attempt for userId: {userId}", userId);

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                _logger.LogWarning("Delete User failed: User not found for userId: {userId}", userId);
                return ServiceResult<bool>.Fail(
                    "User not found",
                    resultType: ServiceResultType.Unauthorized
                );
            }

            // Check for active contracts before proceeding
            var hasActiveContracts = await _contractRepo.HasActiveContractsAsync(userId);

            if (hasActiveContracts)
            {
                _logger.LogWarning("Delete User failed: User has active contracts and cannot be deleted for userId: {userId}", userId);
                return ServiceResult<bool>.Fail(
                    "User has active contracts and cannot be deleted",
                    resultType: ServiceResultType.BadRequest
                );
            }

            // Collect file paths to delete BEFORE the transaction
            var filesToDelete = new List<string>();

            // Collect user photos for deletion
            if (!string.IsNullOrEmpty(user.ProfileImage))
                filesToDelete.Add(user.ProfileImage);
            if (!string.IsNullOrEmpty(user.FrontIdPhoto))
                filesToDelete.Add(user.FrontIdPhoto);
            if (!string.IsNullOrEmpty(user.BackIdPhoto))
                filesToDelete.Add(user.BackIdPhoto);


            // Begin transactional deletion
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // 1. Hard delete booking requests created by this user
                _logger.LogInformation("Deleting booking requests for userId: {userId}", userId);
                await _bookingRequestRepo.DeleteByUserIdAsync(userId);

                // 2. Hard delete all user device tokens
                _logger.LogInformation("Deleting user devices for userId: {userId}", userId);
                await _notificationRepo.DeleteDevicesByUserIdAsync(userId.ToString());

                // 3. Hard delete all notifications
                _logger.LogInformation("Deleting notifications for userId: {userId}", userId);
                await _notificationRepo.DeleteNotificationsByUserIdAsync(userId);

                // 4. Hard delete all reports filed by this user
                _logger.LogInformation("Deleting reports for userId: {userId}", userId);
                await _reportRepo.DeleteByReporterIdAsync(userId);

                // 5. Delete user properties via PropertyService to correctly execute all property cleanup logic
                var ownedPropertyIds = await _propertyRepo.GetPropertyIdsByOwnerAsync(userId);
                foreach(var propertyId in ownedPropertyIds)
                {
                    var propertyDeleteResult = await _propertyService.DeletePropertyAsync(propertyId, userId);
                    if (!propertyDeleteResult.Success)
                    {
                        _logger.LogWarning("Delete User failed: Could not delete owned property {PropertyId} for userId: {userId}. Error: {Error}", 
                            propertyId, 
                            userId,
                            propertyDeleteResult.Message);
                        throw new Exception($"Failed to delete user's property with ID {propertyId}: {propertyDeleteResult.Message}");
                    }
                }

                // 6. Hard delete all property ratings and comments written by this user
                _logger.LogInformation("Deleting property ratings for userId: {userId}", userId);
                await _propertyRatingRepo.DeleteByUserIdAsync(userId);

                _logger.LogInformation("Deleting property comments for userId: {userId}", userId);
                await _propertyCommentRepo.DeleteByUserIdAsync(userId);

                // 7. Soft delete the user
                user.DeletedAt = DateTime.UtcNow;
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    _logger.LogError(
                        "Delete User failed: Failed to update user record for userId: {userId}. Errors: {@Errors}",
                        userId,
                        result.Errors.Select(e => e.Description)
                    );
                    await transaction.RollbackAsync();
                    return ServiceResult<bool>.Fail("Failed to delete user", result.Errors.Select(e => e.Description).ToList());
                }

                await transaction.CommitAsync();
                _logger.LogInformation("User {UserId} soft-deleted successfully. Transaction committed.", userId);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Delete User failed: Transaction rolled back for userId: {userId}", userId);
                return ServiceResult<bool>.Fail("An error occurred while deleting the user. All changes have been rolled back.");
            }

            // Delete files from storage AFTER successful transaction commit
            foreach (var filePath in filesToDelete)
            {
                try
                {
                    _fileService.DeleteImage(filePath);
                }
                catch (Exception ex)
                {
                    // Log but do not fail — DB is already committed
                    _logger.LogWarning(ex, "Failed to delete file from storage: {FilePath}", filePath);
                }
            }

            // Send deletion email (outside transaction)
            await _emailService.SendAccountDeletionEmailAsync(user.Email!, user.FirstName);

            return ServiceResult<bool>.Ok(true, "User deleted successfully", ServiceResultType.Success);
        }
        #endregion


        #region Private Helpers
        private static ServiceResult<bool>? ValidateImage(IFormFile image)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(image.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                return ServiceResult<bool>.Fail("Invalid image format");

            if (image.Length > 2 * 1024 * 1024)
                return ServiceResult<bool>.Fail("Image size exceeds 2MB");

            return null;
        }
        #endregion
    }
}
