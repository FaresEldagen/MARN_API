using AutoMapper;
using MARN_API.DTOs.Dashboard;
using MARN_API.DTOs.Profile;
using MARN_API.DTOs.Property;
using MARN_API.Enums;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

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
            ISavedPropertyRepo savedPropertyRepo
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
            _logger.LogInformation("Get Renter Dashboard Data attempt for userId: {userId}", userId);

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                _logger.LogWarning("Get Renter Dashboard Data failed: User not found for userId: {userId}", userId);
                return ServiceResult<OwnerDashboardDto>.Fail("User not found", resultType: ServiceResultType.Unauthorized);
            }

            var properties = await _propertyRepo.GetOwnedProperties(userId);
            var propertiesCount = properties == null ? 0 : properties.Count;

            var occupiedPlacesCount = await _contractRepo.GetOwnedPropertiesOccupiedPlacesCount(userId);
            var vacantPlacesCount = await _propertyRepo.GetOwnedPropertiesPlacesCount(userId) - occupiedPlacesCount;
            var totalViews = await _propertyRepo.GetOwnedPropertiesViewsCount(userId);

            var monthlyEearnings = await _paymentRepo.GetEarningOverviewMonthly(userId);
            var yearlyEarnings = await _paymentRepo.GetEarningOverviewYearly(userId);
            var withdrawableEarnings = await _paymentRepo.GetWithdrawableEarnings(userId);
            var onHoldEarnings = await _paymentRepo.GetOnHoldEarnings(userId);

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

                AllContracts = allContracts,

                Notifications = notifications,
                UnreadNotificationsCount = unreadNotificationsCount,

                PendingBookingRequests = pendingBookingRequests,
                PendingBookingRequestsCount = pendingBookingRequestsCount,

                AccountStatus = accountSatus,
            };

            _logger.LogInformation("Get Renter Dashboard Data successful for userId: {userId}", userId);
            return ServiceResult<OwnerDashboardDto>.Ok(dashboardData);
        }

        public async Task<ServiceResult<ProfileDto>> GetProfileAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region Profile Settings
        public async Task<ServiceResult<ProfileSettingsDto>> GetProfileSettingsAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<bool>> UpdateProfileDataAsync(UpdateProfileDto updateProfileDto)
        {
            //    var user = await _userManager.FindByIdAsync(updateUserDto.id.ToString());
            //    if (user == null)
            //        return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            //    user = _mapper.Map(updateUserDto, user);

            //    return await _userManager.UpdateAsync(user);
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<bool>> UpdateLegalDataAsync(UpdateLegalDto updateLegalDto)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<bool>> UpdateRoommatePrefrencesAsync(UpdateRoommatePrefrencesDto updateRoommatePrefrencesDto)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<bool>> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            //    var user = await _userManager.FindByIdAsync(changePasswordDto.id.ToString());
            //    if (user == null)
            //        return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            //    if (!await _userManager.CheckPasswordAsync(user, changePasswordDto.CurrentPassword))
            //        return IdentityResult.Failed(new IdentityError { Description = "Current password is incorrect." });

            //    return await _userManager.ChangePasswordAsync(
            //        user,
            //        changePasswordDto.CurrentPassword,
            //        changePasswordDto.NewPassword);
            throw new NotImplementedException();
        }

        public async Task<ServiceResult<bool>> ToggleTwoFactorAsync(string userId, string? password = null)
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

            user.DeletedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            // Need to handle potential cascading deletes or orphaned data in related tables (e.g., Bookings, Reviews, etc.) if necessary

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
    }
}
