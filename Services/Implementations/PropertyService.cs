using AutoMapper;
using System.Linq;
using MARN_API.DTOs.Dashboard;
using MARN_API.DTOs.Notification;
using MARN_API.DTOs.Property;
using MARN_API.Enums;
using MARN_API.Enums.Account;
using MARN_API.Enums.Notification;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using MARN_API.Enums.Contract;
using MARN_API.Enums.Property;
using Hangfire;
using MARN_API.Utilities;

namespace MARN_API.Services.Implementations
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepo _propertyRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<PropertyService> _logger;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IPropertyAmenityRepo _amenityRepo;
        private readonly IPropertyMediaRepo _mediaRepo;
        private readonly IPropertyRuleRepo _ruleRepo;
        private readonly IBookingRequestRepo _bookingRequestRepo;
        private readonly ISavedPropertyRepo _savedPropertyRepo;
        private readonly IContractRepo _contractRepo;
        private readonly IPropertyFeedbackRepo _propertyFeedbackRepo;
        private readonly MARN_API.Data.AppDbContext _context;
        private readonly INotificationService _notificationService;
        private readonly IRoommateMatchingService _matchingService;
        private readonly IUserActivityService _userActivityService;
        private readonly IExternalPropertyAiClient _externalPropertyAiClient;

        public PropertyService(
            IPropertyRepo propertyRepo, 
            UserManager<ApplicationUser> userManager,
            ILogger<PropertyService> logger,
            IMapper mapper,
            IFileService fileService,
            IPropertyAmenityRepo amenityRepo,
            IPropertyMediaRepo mediaRepo,
            IPropertyRuleRepo ruleRepo,
            IBookingRequestRepo bookingRequestRepo,
            ISavedPropertyRepo savedPropertyRepo,
            IContractRepo contractRepo,
            IPropertyFeedbackRepo propertyFeedbackRepo,
            MARN_API.Data.AppDbContext context,
            INotificationService notificationService,
            IRoommateMatchingService matchingService,
            IUserActivityService userActivityService,
            IExternalPropertyAiClient externalPropertyAiClient)
        {
            _propertyRepo = propertyRepo;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
            _fileService = fileService;
            _amenityRepo = amenityRepo;
            _mediaRepo = mediaRepo;
            _ruleRepo = ruleRepo;
            _bookingRequestRepo = bookingRequestRepo;
            _savedPropertyRepo = savedPropertyRepo;
            _contractRepo = contractRepo;
            _propertyFeedbackRepo = propertyFeedbackRepo;
            _context = context;
            _notificationService = notificationService;
            _matchingService = matchingService;
            _userActivityService = userActivityService;
            _externalPropertyAiClient = externalPropertyAiClient;
        }

        public async Task<ServiceResult<bool>> AddPropertyAsync(AddPropertyDto dto, Guid userId)
        {
            _logger.LogInformation("AddProperty attempt for userId: {UserId}", userId);

            if (dto.MediaFiles != null && dto.MediaFiles.Count > 9)
            {
                _logger.LogWarning("AddProperty failed: Exceeded maximum images count for user {UserId}", userId);
                return ServiceResult<bool>.Fail("You can only upload a maximum of 10 images including the primary image.", resultType: ServiceResultType.BadRequest);
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                _logger.LogWarning("AddProperty failed: User not found for userId: {userId}", userId);
                return ServiceResult<bool>.Fail("User not found", resultType: ServiceResultType.Unauthorized);
            }

            if (user.AccountStatus == AccountStatus.Banned)
            {
                _logger.LogWarning("AddProperty failed: Banned user {UserId}", userId);
                return ServiceResult<bool>.Fail("Banned accounts cannot add properties.", resultType: ServiceResultType.Forbidden);
            }

            if (user.AccountStatus != AccountStatus.Verified)
            {
                _logger.LogWarning("AddProperty failed: Account not verified for user {UserId}", userId);
                return ServiceResult<bool>.Fail("Your account must be verified to add a property.", resultType: ServiceResultType.Unauthorized);
            }

            var property = _mapper.Map<Property>(dto);
            property.OwnerId = userId;

            if (dto.ProofOfOwnership != null)
            {
                var proofPath = await _fileService.SaveImageAsync(dto.ProofOfOwnership, "documents");
                if (proofPath != null)
                {
                    property.ProofOfOwnership = proofPath;
                }
            }


            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _propertyRepo.AddPropertyAsync(property);
                _logger.LogInformation("Added Property {PropertyId} for user {UserId}", property.Id, userId);

                if (dto.Amenities != null)
                {
                    foreach (var am in dto.Amenities)
                    {
                        await _amenityRepo.AddByPropertyIdAsync(property.Id, new PropertyAmenity { Amenity = am });
                    }
                }

                if (dto.Rules != null)
                {
                    foreach (var rule in dto.Rules)
                    {
                        if (string.IsNullOrWhiteSpace(rule))
                            continue;

                        await _ruleRepo.AddByPropertyIdAsync(property.Id, new PropertyRule { Rule = rule });
                    }
                }

                if (dto.PrimaryImage != null)
                {
                    var primaryPath = await _fileService.SaveImageAsync(dto.PrimaryImage, "properties");
                    if (primaryPath != null)
                    {
                        await _mediaRepo.AddByPropertyIdAsync(property.Id, new PropertyMedia { Path = primaryPath, IsPrimary = true });
                    }
                }

                if (dto.MediaFiles != null)
                {
                    foreach (var file in dto.MediaFiles)
                    {
                        var path = await _fileService.SaveImageAsync(file, "properties");
                        if (path != null)
                        {
                            await _mediaRepo.AddByPropertyIdAsync(property.Id, new PropertyMedia { Path = path, IsPrimary = false });
                        }
                    }
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            _logger.LogInformation("Successfully fully mapped and saved property {PropertyId}", property.Id);
            await NotifyExternalPropertyAddedAsync(property.Id);

            try
            {
                await _notificationService.SendNotificationAsync(new NotificationRequestDto
                {
                    UserId = userId.ToString(),
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PropertyAdded,

                TitleKey = "NOTIFICATION_PROPERTY_SUBMITTED_TITLE",
                BodyKey = "NOTIFICATION_PROPERTY_SUBMITTED_BODY",
                LocalizationArguments = new() { property.Title },
                Title = "Property Submitted for Review",
                Body = $"Your property \"{property.Title}\" has been submitted successfully and is now pending admin verification. " +
                       "This process may take up to 24 hours. We'll notify you once it's approved.",
                    ActionType = NotificationActionType.Property,
                    ActionId = property.Id.ToString()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send property added notification for propertyId: {PropertyId}", property.Id);
            }

            return ServiceResult<bool>.Ok(true, "Property added successfully.", code: "ZZ_PROPERTY_ADDED_SUCCESSFULLY");
        }

        public async Task<ServiceResult<PropertySearchResultDto>> SearchPropertiesAsync(PropertySearchFilterDto filter, Guid? userId)
        {
            _logger.LogInformation("SearchProperties called with keyword: {Keyword}, page: {Page}", filter.Keyword, filter.Page);

            var result = await _propertyRepo.SearchPropertiesAsync(filter, userId);
            if (userId.HasValue)
            {
                await TryRecordActivityAsync(userId.Value, UserActivityTypes.Search, metadata: filter);
            }

            return ServiceResult<PropertySearchResultDto>.Ok(result);
        }

        public async Task<ServiceResult<PropertyDetailsDto>> GetPropertyDetailsAsync(long propertyId, Guid? userId)
        {
            var property = await _propertyRepo.GetByIdAsync(propertyId);
            if (property == null || property.DeletedAt != null)
            {
                return ServiceResult<PropertyDetailsDto>.Fail("Property not found.", resultType: ServiceResultType.NotFound);
            }

            var isOwner = userId.HasValue && property.OwnerId == userId.Value;
            var currentUser = userId.HasValue
                ? await _userManager.FindByIdAsync(userId.Value.ToString())
                : null;
            var isAdmin = currentUser != null && await _userManager.IsInRoleAsync(currentUser, "Admin");

            if (!isOwner && !isAdmin && (!property.IsActive || property.Status != PropertyStatus.Verified))
            {
                return ServiceResult<PropertyDetailsDto>.Fail("Property not found.", resultType: ServiceResultType.NotFound);
            }

            var dto = await _propertyRepo.GetPropertyDetailsAsync(propertyId, userId);
            if (dto == null)
            {
                return ServiceResult<PropertyDetailsDto>.Fail("Property not found.", resultType: ServiceResultType.NotFound);
            }

            dto.IsUserAllowedToFeedback = userId.HasValue
                && await _contractRepo.HasEligiblePropertyContractAsync(userId.Value, propertyId);

            bool shouldIncrementViews = !userId.HasValue || dto.HostedBy.Id != userId.Value;
            if (shouldIncrementViews)
            {
                await _propertyRepo.IncrementViewsAsync(propertyId);
                dto.ViewsCount += 1;
            }

            if (userId.HasValue && !isAdmin)
            {
                await TryRecordActivityAsync(userId.Value, UserActivityTypes.View, propertyId);
            }

            if (userId.HasValue && dto.HostedBy.Id == userId.Value)
            {
                var ownerContracts = await _contractRepo.GetContractsByProperty(userId.Value, propertyId);
                var ownerPendingRequests = await _bookingRequestRepo.GetOwnerPendingRequestsByProperty(userId.Value, propertyId);

                dto.OwnerExtras = new OwnerPropertyExtrasDto
                {
                    PropertyStatus = (await _propertyRepo.GetByIdAsync(propertyId))?.Status,
                    ContractsHistory = ownerContracts
                        .Select(c => new OwnerPropertyContractHistoryDto
                        {
                            ContractId = c.ContractId,
                            ContractStatus = c.ContractStatus,
                            ExpiryDate = c.ExpiryDate,
                            RenterId = c.RenterId,
                            RenterName = c.RenterName
                        })
                        .ToList(),
                    PendingBookingRequests = ownerPendingRequests
                        .Select(r => new OwnerPropertyPendingBookingRequestDto
                        {
                            BookingRequestId = r.BookingRequestId,
                            StartDate = r.StartDate,
                            EndDate = r.EndDate,
                            PaymentFrequency = r.PaymentFrequency,

                            RenterId = r.RenterId,
                            RenterName = r.RenterName,
                            RenterProfileImage = r.RenterProfileImage
                        })
                        .ToList()
                };
            }

            if (dto.IsShared && dto.ActiveRenters.Any())
            {
                // 1. Remove duplicate humans (if a user has multiple active contracts)
                dto.ActiveRenters = dto.ActiveRenters
                    .GroupBy(a => a.Id)
                    .Select(g => g.First())
                    .ToList();

                // 2. Remove the viewer from the roommate list
                if (userId.HasValue)
                {
                    dto.ActiveRenters = dto.ActiveRenters
                        .Where(a => a.Id != userId.Value)
                        .ToList();
                }

                // 3. Batch calculate compatibility scores
                if (userId.HasValue && userId.Value != Guid.Empty && dto.ActiveRenters.Any())
                {
                    var targetUserIds = dto.ActiveRenters.Select(a => a.Id).ToList();
                    var matchResult = await _matchingService.GetMatchScoresAsync(userId.Value, targetUserIds);

                    if (matchResult.Success && matchResult.Data != null)
                    {
                        var results = matchResult.Data;
                        foreach (var renter in dto.ActiveRenters)
                        {
                            if (results.TryGetValue(renter.Id, out var match))
                            {
                                // Only assign score if it's > 0 (meaning both sides have matching enabled)
                                if (match.CompatibilityScore > 0 || match.TopMatchingTraits.Any())
                                {
                                    renter.MatchingPercentage = match.CompatibilityScore;
                                }
                                else
                                {
                                    renter.MatchingPercentage = null;
                                }
                            }
                        }
                    }
                }
            }

            return ServiceResult<PropertyDetailsDto>.Ok(dto);
        }

        public async Task<ServiceResult<PropertyEditDataDto>> GetPropertyEditAsync(long propertyId, Guid userId)
        {
            var property = await _propertyRepo.GetByIdAsync(propertyId);
            if (property == null)
                return ServiceResult<PropertyEditDataDto>.Fail("Property not found.", resultType: ServiceResultType.NotFound);

            if (property.OwnerId != userId)
                return ServiceResult<PropertyEditDataDto>.Fail("Unauthorized access.", resultType: ServiceResultType.Forbidden);

            var dto = _mapper.Map<PropertyEditDataDto>(property);

            var amenities = await _amenityRepo.GetByPropertyIdAsync(propertyId);
            var rules = await _ruleRepo.GetByPropertyIdAsync(propertyId);
            var media = await _mediaRepo.GetByPropertyIdAsync(propertyId);

            dto.Amenities = _mapper.Map<System.Collections.Generic.List<PropertyAmenityDto>>(amenities);
            dto.Rules = _mapper.Map<System.Collections.Generic.List<PropertyRuleDto>>(rules);
            
            // Populate individual fields
            dto.PrimaryImageUrl = media.FirstOrDefault(m => m.IsPrimary)?.Path ?? string.Empty;
            dto.ProofOfOwnershipUrl = property.ProofOfOwnership;

            // Exclude primary image from media list
            var galleryMedia = media.Where(m => !m.IsPrimary).ToList();
            dto.Media = _mapper.Map<System.Collections.Generic.List<PropertyMediaDto>>(galleryMedia);

            return ServiceResult<PropertyEditDataDto>.Ok(dto);
        }

        public async Task<ServiceResult<bool>> EditPropertyAsync(long propertyId, EditPropertyDto dto, Guid userId)
        {
            _logger.LogInformation("Edit property attempt {PropertyId} for user {UserId}", propertyId, userId);

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                _logger.LogWarning("EditProperty failed: User not found for user {UserId}", userId);
                return ServiceResult<bool>.Fail("User not found.", resultType: ServiceResultType.Unauthorized);
            }

            if (user.AccountStatus != AccountStatus.Verified)
            {
                _logger.LogWarning("EditProperty failed: Account not verified for user {UserId}", userId);
                return ServiceResult<bool>.Fail("Your account must be verified to edit a property.", resultType: ServiceResultType.Unauthorized);
            }

            var property = await _propertyRepo.GetByIdAsync(propertyId);
            if (property == null)
            {
                return ServiceResult<bool>.Fail("Property not found.", resultType: ServiceResultType.NotFound);
            }

            if (dto.AddedMediaFiles != null && 
                (property.Media.Count + dto.AddedMediaFiles.Count - dto.RemovedMediaIds.Count) > 9)
            {
                return ServiceResult<bool>.Fail("You cannot add more than 10 images at once.", resultType: ServiceResultType.BadRequest);
            }

            if (property.OwnerId != userId)
            {
                return ServiceResult<bool>.Fail("Unauthorized access.", resultType: ServiceResultType.Forbidden);
            }

            if (property.Contracts != null && property.Contracts.Any(c => c.Status == ContractStatus.Active))
            {
                return ServiceResult<bool>.Fail("Cannot edit a property that has an active contract.", resultType: ServiceResultType.BadRequest);
            }

            _mapper.Map(dto, property);
            property.Status = MARN_API.Enums.Property.PropertyStatus.Pending;

            if (dto.NewProofOfOwnership != null)
            {
                if (!string.IsNullOrEmpty(property.ProofOfOwnership))
                {
                    _fileService.DeleteImage(property.ProofOfOwnership);
                }

                var proofPath = await _fileService.SaveImageAsync(dto.NewProofOfOwnership, "documents");
                if (proofPath != null)
                {
                    property.ProofOfOwnership = proofPath;
                }
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _propertyRepo.UpdatePropertyAsync(property);

                if (dto.RemovedAmenityIds != null)
                {
                    foreach(long id in dto.RemovedAmenityIds) await _amenityRepo.RemoveByObjectIdAsync(id);
                }
                if (dto.RemovedRuleIds != null)
                {
                    foreach(long id in dto.RemovedRuleIds) await _ruleRepo.RemoveByObjectIdAsync(id);
                }
                if (dto.RemovedMediaIds != null)
                {
                    var existingMedia = await _mediaRepo.GetByPropertyIdAsync(propertyId);
                    foreach(long id in dto.RemovedMediaIds) 
                    {
                        var mediaItem = existingMedia.FirstOrDefault(m => m.Id == id);
                        if (mediaItem != null)
                        {
                            _fileService.DeleteImage(mediaItem.Path);
                        }
                        await _mediaRepo.RemoveByObjectIdAsync(id);
                    }
                }

                if (dto.AddedAmenities != null)
                {
                    foreach(var am in dto.AddedAmenities) await _amenityRepo.AddByPropertyIdAsync(propertyId, new PropertyAmenity { Amenity = am });
                }
                if (dto.AddedRules != null)
                {
                    foreach(var rule in dto.AddedRules) await _ruleRepo.AddByPropertyIdAsync(propertyId, new PropertyRule { Rule = rule });
                }

                if (dto.NewPrimaryImage != null)
                {
                    var existingMedia = await _mediaRepo.GetByPropertyIdAsync(propertyId);
                    var oldPrimary = existingMedia.FirstOrDefault(m => m.IsPrimary);
                    if (oldPrimary != null)
                    {
                        _fileService.DeleteImage(oldPrimary.Path);
                        await _mediaRepo.RemoveByObjectIdAsync(oldPrimary.Id);
                    }

                    var pPath = await _fileService.SaveImageAsync(dto.NewPrimaryImage, "properties");
                    if (pPath != null)
                    {
                        await _mediaRepo.AddByPropertyIdAsync(propertyId, new PropertyMedia { Path = pPath, IsPrimary = true });
                    }
                }

                if (dto.AddedMediaFiles != null)
                {
                    foreach (var mf in dto.AddedMediaFiles)
                    {
                        var mPath = await _fileService.SaveImageAsync(mf, "properties");
                        if (mPath != null)
                        {
                            await _mediaRepo.AddByPropertyIdAsync(propertyId, new PropertyMedia { Path = mPath, IsPrimary = false });
                        }
                    }
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            _logger.LogInformation("Property {PropertyId} edited successfully by user {UserId}", propertyId, userId);
            await NotifyExternalPropertyUpdatedAsync(propertyId);

            try
            {
                await _notificationService.SendNotificationAsync(new NotificationRequestDto
                {
                    UserId = userId.ToString(),
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PropertyEdited,

                TitleKey = "NOTIFICATION_PROPERTY_UPDATED_TITLE",
                BodyKey = "NOTIFICATION_PROPERTY_UPDATED_BODY",
                LocalizationArguments = new() { property.Title },
                Title = "Property Update Under Review",
                Body = $"Your property \"{property.Title}\" has been updated and its status is now set back to pending. " +
                       "The admin will re-verify it, which may take up to 24 hours. We'll notify you once it's approved.",

                    ActionType = NotificationActionType.Property,
                    ActionId = propertyId.ToString()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send property edited notification for propertyId: {PropertyId}", propertyId);
            }

            return ServiceResult<bool>.Ok(true, "Property updated successfully.", code: "ZZ_PROPERTY_UPDATED_SUCCESSFULLY");
        }

        public async Task<ServiceResult<bool>> ToggleSavePropertyAsync(long propertyId, Guid userId)
        {
            var property = await _propertyRepo.GetByIdAsync(propertyId);
            if (property == null || property.DeletedAt != null)
                return ServiceResult<bool>.Fail("Property not found.", resultType: ServiceResultType.NotFound);

            bool isSaved = await _savedPropertyRepo.HasSavedPropertyAsync(userId, propertyId);
            if (isSaved)
            {
                await _savedPropertyRepo.UnsavePropertyAsync(userId, propertyId);
                await TryRemoveActivityAsync(userId, UserActivityTypes.Save, propertyId);
                return ServiceResult<bool>.Ok(false, "Property unsaved successfully.", code: "ZZ_PROPERTY_UNSAVED_SUCCESSFULLY");
            }
            else
            {
                var savedProperty = new SavedProperty
                {
                    UserId = userId,
                    PropertyId = propertyId
                };
                await _savedPropertyRepo.SavePropertyAsync(savedProperty);
                await TryRecordActivityAsync(userId, UserActivityTypes.Save, propertyId);
                return ServiceResult<bool>.Ok(true, "Property saved successfully.", code: "ZZ_PROPERTY_SAVED_SUCCESSFULLY");
            }
        }

        public async Task<ServiceResult<bool>> DeactivatePropertyAsync(long propertyId, Guid userId)
        {
            var property = await _propertyRepo.GetByIdAsync(propertyId);
            if (property == null || property.OwnerId != userId)
                return ServiceResult<bool>.Fail("Unauthorized or NotFound.", resultType: ServiceResultType.Forbidden);

            property.IsActive = !property.IsActive;
            await _propertyRepo.UpdatePropertyAsync(property);

            return ServiceResult<bool>.Ok(
                property.IsActive,
                property.IsActive ? "Property activated successfully." : "Property deactivated successfully.",
                code: property.IsActive ? "ZZ_PROPERTY_ACTIVATED_SUCCESSFULLY" : "ZZ_PROPERTY_DEACTIVATED_SUCCESSFULLY");
        }

        public async Task<ServiceResult<bool>> DeletePropertyAsync(long propertyId, Guid userId, bool adminInitiated = false, bool suppressNotification = false)
        {
            var property = await _propertyRepo.GetByIdAsync(propertyId);
            if (property == null || property.OwnerId != userId)
                return ServiceResult<bool>.Fail("Unauthorized or NotFound.", resultType: ServiceResultType.Forbidden);

            if (property.Contracts != null && property.Contracts.Any(c => c.Status == ContractStatus.Active))
            {
                return ServiceResult<bool>.Fail("Property has active contracts and cannot be deleted.", resultType: ServiceResultType.BadRequest);
            }

            var filesToDelete = new List<string>();
            if (!string.IsNullOrEmpty(property.ProofOfOwnership))
            {
                filesToDelete.Add(property.ProofOfOwnership);
            }

            var mediaPaths = await _propertyRepo.GetMediaPathsByPropertyIdsAsync(new System.Collections.Generic.List<long> { propertyId });
            filesToDelete.AddRange(mediaPaths);

            var ownsTransaction = _context.Database.CurrentTransaction == null;
            var transaction = ownsTransaction
                ? await _context.Database.BeginTransactionAsync()
                : null;
            try
            {
                await _bookingRequestRepo.DeleteByPropertyIdAsync(propertyId);
                await _propertyFeedbackRepo.DeleteByPropertyIdAsync(propertyId);

                string jobId = BackgroundJob.Schedule(
                    () => DeletePropertyMediaAfterGracePeriod(propertyId, filesToDelete),
                    TimeSpan.FromDays(7));

                property.DeletedAt = DateTime.UtcNow;
                property.ImagesDeletionJob = jobId;
                await _propertyRepo.UpdatePropertyAsync(property);

                if (transaction != null)
                {
                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    await transaction.RollbackAsync();
                }

                _logger.LogError(ex, "Transaction failed deleting property {Id}", propertyId);
                return ServiceResult<bool>.Fail("Error deleting property.", resultType: ServiceResultType.BadRequest);
            }

            await NotifyExternalPropertyDeletedAsync(propertyId);

            if (!suppressNotification)
            {
                await _notificationService.SendNotificationAsync(new NotificationRequestDto
                {
                    UserId = userId.ToString(),
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.PropertyDeleted,

                    TitleKey = adminInitiated
                        ? "NOTIFICATION_ADMIN_PROPERTY_DELETED_TITLE"
                        : "NOTIFICATION_PROPERTY_DELETED_TITLE",
                    BodyKey = adminInitiated
                        ? "NOTIFICATION_ADMIN_PROPERTY_DELETED_BODY"
                        : "NOTIFICATION_PROPERTY_DELETED_BODY",
                    LocalizationArguments = new() { property.Title },
                    Title = "Property Deleted",
                    Body = adminInitiated
                        ? $"An admin has deleted your property \"{property.Title}\". If you believe this is a mistake, please contact support."
                        : $"Your property \"{property.Title}\" has been deleted successfully. " +
                          "If this was a mistake or you'd like to restore it, please contact our support team for assistance.",
                });
            }

            return ServiceResult<bool>.Ok(
                true,
                "Property deleted completely.",
                code: adminInitiated ? "ZZ_ADMIN_PROPERTY_DELETED_SUCCESSFULLY" : "ZZ_PROPERTY_DELETED_SUCCESSFULLY");
        }

        public async Task DeletePropertyMediaAfterGracePeriod(long propertyId, List<string> filesToDelete)
        {
            var property = await _context.Properties
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.Id == propertyId);

            if (property == null || property.DeletedAt == null)
            {
                return;
            }

            foreach (var file in filesToDelete)
            {
                try 
                { 
                    _fileService.DeleteImage(file); 
                }
                catch (Exception ex) 
                {
                    _logger.LogWarning(ex, "Failed to delete file from storage: {FilePath}", file);
                    throw; 
                }
            }

            property.ProofOfOwnership = null;
            property.ImagesDeletionJob = null;

            await _propertyRepo.DeleteMediaByPropertyIdsAsync([propertyId]);
            await _context.SaveChangesAsync();
        }

        private async Task TryRecordActivityAsync(Guid userId, string activityType, long? propertyId = null, object? metadata = null)
        {
            try
            {
                await _userActivityService.RecordAsync(userId, activityType, propertyId, metadata);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to record user activity {ActivityType} for user {UserId} and property {PropertyId}",
                    activityType,
                    userId,
                    propertyId);
            }
        }

        private async Task TryRemoveActivityAsync(Guid userId, string activityType, long? propertyId = null)
        {
            try
            {
                await _userActivityService.RemoveAsync(userId, activityType, propertyId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to remove user activity {ActivityType} for user {UserId} and property {PropertyId}",
                    activityType,
                    userId,
                    propertyId);
            }
        }

        private Task NotifyExternalPropertyAddedAsync(long propertyId)
        {
            return _externalPropertyAiClient.NotifyPropertyAddedAsync(propertyId);
        }

        private Task NotifyExternalPropertyUpdatedAsync(long propertyId)
        {
            return _externalPropertyAiClient.NotifyPropertyUpdatedAsync(propertyId);
        }

        private Task NotifyExternalPropertyDeletedAsync(long propertyId)
        {
            return _externalPropertyAiClient.NotifyPropertyDeletedAsync(propertyId);
        }
    }
}
