using AutoMapper;
using MARN_API.DTOs.Notification;
using MARN_API.DTOs.PropertyFeedback;
using MARN_API.Enums;
using MARN_API.Enums.Notification;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;

namespace MARN_API.Services.Implementations
{
    public class PropertyRatingService : IPropertyRatingService
    {
        private readonly IPropertyRatingRepo _propertyRatingRepo;
        private readonly IPropertyRepo _propertyRepo;
        private readonly IContractRepo _contractRepo;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public PropertyRatingService(
            IPropertyRatingRepo propertyRatingRepo,
            IPropertyRepo propertyRepo,
            IContractRepo contractRepo,
            IMapper mapper,
            INotificationService notificationService)
        {
            _propertyRatingRepo = propertyRatingRepo;
            _propertyRepo = propertyRepo;
            _contractRepo = contractRepo;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<ServiceResult<PropertyRatingSummaryDto>> GetSummaryAsync(long propertyId, Guid? currentUserId = null)
        {
            if (!await _propertyRepo.ExistsAsync(propertyId))
            {
                return ServiceResult<PropertyRatingSummaryDto>.Fail(
                    "Property not found",
                    resultType: ServiceResultType.NotFound);
            }

            var summary = await _propertyRatingRepo.GetSummaryAsync(propertyId, currentUserId);
            return ServiceResult<PropertyRatingSummaryDto>.Ok(summary);
        }

        public async Task<ServiceResult<PropertyRatingDto>> CreateAsync(long propertyId, Guid userId, CreatePropertyRatingDto dto)
        {
            var property = await _propertyRepo.GetByIdAsync(propertyId);
            if (property == null)
            {
                return ServiceResult<PropertyRatingDto>.Fail(
                    "Property not found",
                    resultType: ServiceResultType.NotFound);
            }

            if (!await _contractRepo.HasEligiblePropertyContractAsync(userId, propertyId))
            {
                return ServiceResult<PropertyRatingDto>.Fail(
                    "You are not allowed to rate or comment on this property",
                    resultType: ServiceResultType.Forbidden);
            }

            var existingRating = await _propertyRatingRepo.GetByPropertyAndUserAsync(propertyId, userId);
            if (existingRating != null)
            {
                existingRating.Rating = dto.Rating;
                existingRating.UpdatedAt = DateTime.UtcNow;

                await _propertyRatingRepo.UpdateAsync(existingRating);

                await _notificationService.SendNotificationAsync(new NotificationRequestDto
                {
                    UserId = property.OwnerId.ToString(),
                    UserType = NotificationUserType.Owner,
                    Type = NotificationType.NewReview,

                    Title = "Rating Updated on Your Property",
                    Body = $"A user has updated their rating to {dto.Rating} stars on your property \"{property.Title}\".",

                    ActionType = NotificationActionType.Property,
                    ActionId = propertyId.ToString()
                });

                return ServiceResult<PropertyRatingDto>.Ok(
                    _mapper.Map<PropertyRatingDto>(existingRating),
                    "Rating updated successfully");
            }

            var rating = new PropertyRating
            {
                PropertyId = propertyId,
                UserId = userId,
                Rating = dto.Rating
            };

            await _propertyRatingRepo.CreateAsync(rating);

            await _notificationService.SendNotificationAsync(new NotificationRequestDto
            {
                UserId = property.OwnerId.ToString(),
                UserType = NotificationUserType.Owner,
                Type = NotificationType.NewReview,

                Title = "New Rating on Your Property",
                Body = $"A user has rated your property with {dto.Rating} stars: \"{property.Title}\".",

                ActionType = NotificationActionType.Property,
                ActionId = propertyId.ToString()
            });

            return ServiceResult<PropertyRatingDto>.Ok(_mapper.Map<PropertyRatingDto>(rating), "Rating created successfully", ServiceResultType.Created);
        }

        public async Task<ServiceResult<PropertyRatingDto>> UpdateAsync(long propertyId, Guid userId, UpdatePropertyRatingDto dto)
        {
            var property = await _propertyRepo.GetByIdAsync(propertyId);
            if (property == null)
            {
                return ServiceResult<PropertyRatingDto>.Fail(
                    "Property not found",
                    resultType: ServiceResultType.NotFound);
            }

            if (!await _contractRepo.HasEligiblePropertyContractAsync(userId, propertyId))
            {
                return ServiceResult<PropertyRatingDto>.Fail(
                    "You are not allowed to rate or comment on this property",
                    resultType: ServiceResultType.Forbidden);
            }

            var rating = await _propertyRatingRepo.GetByPropertyAndUserAsync(propertyId, userId);
            if (rating == null)
            {
                return ServiceResult<PropertyRatingDto>.Fail(
                    "Rating not found",
                    resultType: ServiceResultType.NotFound);
            }

            rating.Rating = dto.Rating;
            rating.UpdatedAt = DateTime.UtcNow;

            await _propertyRatingRepo.UpdateAsync(rating);

            await _notificationService.SendNotificationAsync(new NotificationRequestDto
            {
                UserId = property.OwnerId.ToString(),
                UserType = NotificationUserType.Owner,
                Type = NotificationType.NewReview,

                Title = "Rating Updated on Your Property",
                Body = $"A user has updated their rating to {dto.Rating} stars on your property \"{property.Title}\".",

                ActionType = NotificationActionType.Property,
                ActionId = propertyId.ToString()
            });

            return ServiceResult<PropertyRatingDto>.Ok(_mapper.Map<PropertyRatingDto>(rating), "Rating updated successfully");
        }

        public async Task<ServiceResult<bool>> DeleteAsync(long propertyId, Guid userId)
        {
            var validation = await ValidatePropertyAccessAsync(propertyId);
            if (validation != null)
                return validation;

            var rating = await _propertyRatingRepo.GetByPropertyAndUserAsync(propertyId, userId);
            if (rating == null)
            {
                return ServiceResult<bool>.Fail(
                    "Rating not found",
                    resultType: ServiceResultType.NotFound);
            }

            if (!await _contractRepo.HasEligiblePropertyContractAsync(userId, propertyId))
            {
                return ServiceResult<bool>.Fail(
                    "You are not allowed to rate or comment on this property",
                    resultType: ServiceResultType.Forbidden);
            }

            await _propertyRatingRepo.DeleteAsync(rating);
            return ServiceResult<bool>.Ok(true, "Rating deleted successfully");
        }

        private async Task<ServiceResult<bool>?> ValidatePropertyAccessAsync(long propertyId)
        {
            if (!await _propertyRepo.ExistsAsync(propertyId))
            {
                return ServiceResult<bool>.Fail(
                    "Property not found",
                    resultType: ServiceResultType.NotFound);
            }

            return null;
        }
    }
}
