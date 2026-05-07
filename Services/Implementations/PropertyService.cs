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
using System;
using System.Threading.Tasks;
using MARN_API.Enums.Contract;

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
        private readonly IPropertyRatingRepo _propertyRatingRepo;
        private readonly IPropertyCommentRepo _propertyCommentRepo;
        private readonly MARN_API.Data.AppDbContext _context;
        private readonly INotificationService _notificationService;

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
            IPropertyRatingRepo propertyRatingRepo,
            IPropertyCommentRepo propertyCommentRepo,
            MARN_API.Data.AppDbContext context,
            INotificationService notificationService)
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
            _propertyRatingRepo = propertyRatingRepo;
            _propertyCommentRepo = propertyCommentRepo;
            _context = context;
            _notificationService = notificationService;
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

            _logger.LogInformation("Successfully fully mapped and saved property {PropertyId}", property.Id);

            await _notificationService.SendNotificationAsync(new NotificationRequestDto
            {
                UserId = userId.ToString(),
                UserType = NotificationUserType.Owner,
                Type = NotificationType.PropertyAdded,

                Title = "Property Submitted for Review",
                Body = $"Your property \"{property.Title}\" has been submitted successfully and is now pending admin verification. " +
                       "This process may take up to 24 hours. We'll notify you once it's approved.",

                ActionType = NotificationActionType.Property,
                ActionId = property.Id.ToString()
            });

            return ServiceResult<bool>.Ok(true, "Property added successfully.");
        }

        public async Task<ServiceResult<PropertySearchResultDto>> SearchPropertiesAsync(PropertySearchFilterDto filter, Guid? userId)
        {
            _logger.LogInformation("SearchProperties called with keyword: {Keyword}, page: {Page}", filter.Keyword, filter.Page);

            var result = await _propertyRepo.SearchPropertiesAsync(filter, userId);
            return ServiceResult<PropertySearchResultDto>.Ok(result);
        }

        public async Task<ServiceResult<PropertyDetailsDto>> GetPropertyDetailsAsync(long propertyId, Guid? userId)
        {
            var dto = await _propertyRepo.GetPropertyDetailsAsync(propertyId, userId);
            if (dto == null)
            {
                return ServiceResult<PropertyDetailsDto>.Fail("Property not found.", resultType: ServiceResultType.NotFound);
            }

            bool shouldIncrementViews = !userId.HasValue || dto.HostedBy.Id != userId.Value;
            if (shouldIncrementViews)
            {
                await _propertyRepo.IncrementViewsAsync(propertyId);
                dto.ViewsCount += 1;
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
                            RenterId = r.RenterId,
                            RenterName = r.RenterName,
                            RenterProfileImage = r.RenterProfileImage
                        })
                        .ToList()
                };
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
            dto.Media = _mapper.Map<System.Collections.Generic.List<PropertyMediaDto>>(media);

            return ServiceResult<PropertyEditDataDto>.Ok(dto);
        }

        public async Task<ServiceResult<bool>> EditPropertyAsync(long propertyId, EditPropertyDto dto, Guid userId)
        {
            _logger.LogInformation("Edit property attempt {PropertyId} for user {UserId}", propertyId, userId);

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null || user.AccountStatus != AccountStatus.Verified)
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

            _logger.LogInformation("Property {PropertyId} edited successfully by user {UserId}", propertyId, userId);

            await _notificationService.SendNotificationAsync(new NotificationRequestDto
            {
                UserId = userId.ToString(),
                UserType = NotificationUserType.Owner,
                Type = NotificationType.PropertyEdited,

                Title = "Property Update Under Review",
                Body = $"Your property \"{property.Title}\" has been updated and its status is now set back to pending. " +
                       "The admin will re-verify it, which may take up to 24 hours. We'll notify you once it's approved.",

                ActionType = NotificationActionType.Property,
                ActionId = propertyId.ToString()
            });

            return ServiceResult<bool>.Ok(true, "Property updated successfully.");
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
                return ServiceResult<bool>.Ok(false, "Property unsaved successfully.");
            }
            else
            {
                var savedProperty = new SavedProperty
                {
                    UserId = userId,
                    PropertyId = propertyId
                };
                await _savedPropertyRepo.SavePropertyAsync(savedProperty);
                return ServiceResult<bool>.Ok(true, "Property saved successfully.");
            }
        }

        public async Task<ServiceResult<bool>> DeactivatePropertyAsync(long propertyId, Guid userId)
        {
            var property = await _propertyRepo.GetByIdAsync(propertyId);
            if (property == null || property.OwnerId != userId)
                return ServiceResult<bool>.Fail("Unauthorized or NotFound.", resultType: ServiceResultType.Forbidden);

            property.IsActive = !property.IsActive;
            await _propertyRepo.UpdatePropertyAsync(property);

            return ServiceResult<bool>.Ok(true, "Property activation toggled.");
        }

        public async Task<ServiceResult<bool>> DeletePropertyAsync(long propertyId, Guid userId)
        {
            var property = await _propertyRepo.GetByIdAsync(propertyId);
            if (property == null || property.OwnerId != userId)
                return ServiceResult<bool>.Fail("Unauthorized or NotFound.", resultType: ServiceResultType.Forbidden);

            if (property.Contracts != null && property.Contracts.Any(c => c.Status == ContractStatus.Active))
            {
                return ServiceResult<bool>.Fail("Property has active contracts and cannot be deleted.", resultType: ServiceResultType.BadRequest);
            }

            var filesToDelete = new System.Collections.Generic.List<string>();
            if (!string.IsNullOrEmpty(property.ProofOfOwnership))
            {
                filesToDelete.Add(property.ProofOfOwnership);
            }

            var mediaPaths = await _propertyRepo.GetMediaPathsByPropertyIdsAsync(new System.Collections.Generic.List<long> { propertyId });
            filesToDelete.AddRange(mediaPaths);

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _bookingRequestRepo.DeleteByPropertyIdAsync(propertyId);
                await _propertyCommentRepo.DeleteByPropertyIdAsync(propertyId);
                await _propertyRatingRepo.DeleteByPropertyIdAsync(propertyId);
                await _propertyRepo.DeleteMediaByPropertyIdsAsync(new System.Collections.Generic.List<long> { propertyId });

                property.DeletedAt = DateTime.UtcNow;
                await _propertyRepo.UpdatePropertyAsync(property);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Transaction failed deleting property {Id}", propertyId);
                return ServiceResult<bool>.Fail("Error deleting property.", resultType: ServiceResultType.BadRequest);
            }

            foreach (var file in filesToDelete)
            {
                try { _fileService.DeleteImage(file); }
                catch (Exception) { /* Ignored */ }
            }

            await _notificationService.SendNotificationAsync(new NotificationRequestDto
            {
                UserId = userId.ToString(),
                UserType = NotificationUserType.Owner,
                Type = NotificationType.PropertyDeleted,

                Title = "Property Deleted",
                Body = $"Your property \"{property.Title}\" has been deleted successfully. " +
                       "If this was a mistake or you'd like to restore it, please contact our support team for assistance.",
            });

            return ServiceResult<bool>.Ok(true, "Property deleted completely.");
        }
    }
}
