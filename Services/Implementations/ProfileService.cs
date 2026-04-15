using AutoMapper;
using MARN_API.DTOs.Dashboard;
using MARN_API.DTOs.Profile;
using MARN_API.DTOs.Property;
using MARN_API.Enums;
using MARN_API.Enums.Account;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Stripe;
using System.Globalization;
using System.Threading.Channels;

namespace MARN_API.Services.Implementations
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountService> _logger;
        private readonly IMapper _mapper;
        private readonly IBookingRequestRepo _bookingRequestRepo;
        private readonly IContractRepo _contractRepo;   
        private readonly INotificationRepo _notificationRepo;
        private readonly IPaymentRepo _paymentRepo;
        private readonly IPropertyRepo _propertyRepo;
        private readonly IRoommatePreferenceRepo _roommatePreferenceRepo;
        private readonly ISavedPropertyRepo _savedPropertyRepo;
        private readonly IFileService _fileService;

        public ProfileService(
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            ILogger<AccountService> logger,
            IBookingRequestRepo bookingRequestRepo,
            IContractRepo contractRepo,
            INotificationRepo notificationRepo,
            IPaymentRepo paymentRepo,
            IPropertyRepo propertyRepo,
            IRoommatePreferenceRepo roommatePreferenceRepo,
            ISavedPropertyRepo savedPropertyRepo,
            IFileService fileService
        )
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _bookingRequestRepo = bookingRequestRepo;
            _contractRepo = contractRepo;
            _notificationRepo = notificationRepo;
            _paymentRepo = paymentRepo;
            _propertyRepo = propertyRepo;
            _roommatePreferenceRepo = roommatePreferenceRepo;
            _savedPropertyRepo = savedPropertyRepo;
            _fileService = fileService;
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

            var allContracts = await _contractRepo.GetContracts(userId);

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
                profileData.RoommatePreferencesEnabled = true;
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

            if (dto.RoommatePreferencesEnabled)
            {
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
            }
            else if (RoommatePreferences != null)
            {
                try
                {
                    RoommatePreferences.RoommatePreferencesEnabled = false;
                    var roommate_result = await _roommatePreferenceRepo.UpdateRoommatePreferences(RoommatePreferences);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        "Update Profile Data failed for userId: {userId}, Errors: Exception occurred while disabling roommate preferences. Exception: {Exception}",
                        dto.UserId,
                        ex
                    );
                    return ServiceResult<bool>.Fail(
                        "Update Profile Data failed. An error occurred while disabling roommate preferences.",
                        resultType: ServiceResultType.BadRequest
                    );
                }
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
                _logger.LogWarning("Delete User  failed: User not found for userId: {userId}", userId);
                return ServiceResult<bool>.Fail(
                    "User not found",
                    resultType: ServiceResultType.Unauthorized
                );
            }

            var hasActiveContracts = await _contractRepo.CheackActiveContractsByUserId(userId);

            if (hasActiveContracts)
            {
                _logger.LogWarning("Delete User failed: User has active contracts and cannot be deleted for userId: {userId}", userId);
                return ServiceResult<bool>.Fail(
                    "User has active contracts and cannot be deleted",
                    resultType: ServiceResultType.BadRequest
                );
            }

            user.DeletedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                _logger.LogWarning(
                    "Delete User failed: Failed to Update the database for userId: {userId}. Errors: {@Errors}", 
                    userId,
                    result.Errors.Select(e => e.Description)
                );
                return ServiceResult<bool>.Fail("Failed to delete user", result.Errors.Select(e => e.Description).ToList());
            }

            _logger.LogInformation("User {UserId} marked as deleted.", userId);
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
