using MARN_API.DTOs.PropertyFeedback;
using MARN_API.Enums;
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

        public PropertyRatingService(
            IPropertyRatingRepo propertyRatingRepo,
            IPropertyRepo propertyRepo,
            IContractRepo contractRepo)
        {
            _propertyRatingRepo = propertyRatingRepo;
            _propertyRepo = propertyRepo;
            _contractRepo = contractRepo;
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
            var validation = await ValidatePropertyInteractionAsync(propertyId, userId);
            if (validation != null)
                return MapFailure<PropertyRating, PropertyRatingDto>(validation);

            var existingRating = await _propertyRatingRepo.GetByPropertyAndUserAsync(propertyId, userId);
            if (existingRating != null)
            {
                existingRating.Rating = dto.Rating;
                existingRating.UpdatedAt = DateTime.UtcNow;

                await _propertyRatingRepo.UpdateAsync(existingRating);
                return ServiceResult<PropertyRatingDto>.Ok(
                    MapRating(existingRating),
                    "Rating updated successfully");
            }

            var rating = new PropertyRating
            {
                PropertyId = propertyId,
                UserId = userId,
                Rating = dto.Rating
            };

            await _propertyRatingRepo.CreateAsync(rating);
            return ServiceResult<PropertyRatingDto>.Ok(MapRating(rating), "Rating created successfully", ServiceResultType.Created);
        }

        public async Task<ServiceResult<PropertyRatingDto>> UpdateAsync(long propertyId, Guid userId, UpdatePropertyRatingDto dto)
        {
            var validation = await ValidatePropertyInteractionAsync(propertyId, userId);
            if (validation != null)
                return MapFailure<PropertyRating, PropertyRatingDto>(validation);

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
            return ServiceResult<PropertyRatingDto>.Ok(MapRating(rating), "Rating updated successfully");
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

        private async Task<ServiceResult<PropertyRating>?> ValidatePropertyInteractionAsync(long propertyId, Guid userId)
        {
            if (!await _propertyRepo.ExistsAsync(propertyId))
            {
                return ServiceResult<PropertyRating>.Fail(
                    "Property not found",
                    resultType: ServiceResultType.NotFound);
            }

            if (!await _contractRepo.HasEligiblePropertyContractAsync(userId, propertyId))
            {
                return ServiceResult<PropertyRating>.Fail(
                    "You are not allowed to rate or comment on this property",
                    resultType: ServiceResultType.Forbidden);
            }

            return null;
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

        private static PropertyRatingDto MapRating(PropertyRating rating)
        {
            return new PropertyRatingDto
            {
                RatingId = rating.Id,
                PropertyId = rating.PropertyId,
                UserId = rating.UserId,
                Rating = rating.Rating,
                CreatedAt = rating.CreatedAt,
                UpdatedAt = rating.UpdatedAt
            };
        }

        private static ServiceResult<TTarget> MapFailure<TSource, TTarget>(ServiceResult<TSource> failure)
        {
            return ServiceResult<TTarget>.Fail(failure.Message ?? "An error occurred.", failure.Errors, failure.Action, failure.ResultType);
        }
    }
}
