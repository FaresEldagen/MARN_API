using MARN_API.DTOs.Notification;
using MARN_API.DTOs.PropertyFeedback;
using MARN_API.Enums;
using MARN_API.Enums.Notification;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;

namespace MARN_API.Services.Implementations
{
    public class PropertyFeedbackService : IPropertyFeedbackService
    {
        private readonly IPropertyFeedbackRepo _propertyFeedbackRepo;
        private readonly IPropertyRepo _propertyRepo;
        private readonly IContractRepo _contractRepo;
        private readonly INotificationService _notificationService;

        public PropertyFeedbackService(
            IPropertyFeedbackRepo propertyFeedbackRepo,
            IPropertyRepo propertyRepo,
            IContractRepo contractRepo,
            INotificationService notificationService)
        {
            _propertyFeedbackRepo = propertyFeedbackRepo;
            _propertyRepo = propertyRepo;
            _contractRepo = contractRepo;
            _notificationService = notificationService;
        }

        public async Task<ServiceResult<PropertyFeedbackSummaryDto>> GetAsync(long propertyId, Guid? currentUserId, int pageNumber, int pageSize)
        {
            if (!await _propertyRepo.ExistsAsync(propertyId))
            {
                return ServiceResult<PropertyFeedbackSummaryDto>.Fail(
                    "Property not found",
                    resultType: ServiceResultType.NotFound);
            }

            var summary = await _propertyFeedbackRepo.GetSummaryAsync(propertyId, currentUserId, pageNumber, pageSize);
            return ServiceResult<PropertyFeedbackSummaryDto>.Ok(summary);
        }

        public async Task<ServiceResult<PropertyFeedbackDto>> CreateAsync(long propertyId, Guid userId, PropertyFeedbackRequestDto dto)
        {
            var property = await _propertyRepo.GetByIdAsync(propertyId);
            if (property == null)
            {
                return ServiceResult<PropertyFeedbackDto>.Fail(
                    "Property not found",
                    resultType: ServiceResultType.NotFound);
            }

            if (!await _contractRepo.HasEligiblePropertyContractAsync(userId, propertyId))
            {
                return ServiceResult<PropertyFeedbackDto>.Fail(
                    "You are not allowed to rate or comment on this property",
                    resultType: ServiceResultType.Forbidden);
            }

            var existingFeedback = await _propertyFeedbackRepo.GetByPropertyAndUserAsync(propertyId, userId);
            if (existingFeedback != null)
            {
                return ServiceResult<PropertyFeedbackDto>.Fail(
                    "You have already submitted feedback for this property",
                    resultType: ServiceResultType.Conflict,
                    code: "ZZ_PROPERTY_FEEDBACK_ALREADY_EXISTS");
            }

            var feedback = new PropertyFeedback
            {
                PropertyId = propertyId,
                UserId = userId,
                Rating = dto.Rating,
                Content = NormalizeContent(dto.Content)
            };

            await _propertyFeedbackRepo.CreateAsync(feedback);
            await NotifyOwnerAsync(property, feedback, isUpdate: false);

            return ServiceResult<PropertyFeedbackDto>.Ok(
                ToDto(feedback),
                "Feedback created successfully",
                ServiceResultType.Created,
                code: "ZZ_PROPERTY_FEEDBACK_CREATED_SUCCESSFULLY");
        }

        public async Task<ServiceResult<PropertyFeedbackDto>> UpdateAsync(long propertyId, Guid userId, PropertyFeedbackRequestDto dto)
        {
            var property = await _propertyRepo.GetByIdAsync(propertyId);
            if (property == null)
            {
                return ServiceResult<PropertyFeedbackDto>.Fail(
                    "Property not found",
                    resultType: ServiceResultType.NotFound);
            }

            if (!await _contractRepo.HasEligiblePropertyContractAsync(userId, propertyId))
            {
                return ServiceResult<PropertyFeedbackDto>.Fail(
                    "You are not allowed to rate or comment on this property",
                    resultType: ServiceResultType.Forbidden);
            }

            var feedback = await _propertyFeedbackRepo.GetByPropertyAndUserAsync(propertyId, userId);
            if (feedback == null)
            {
                return ServiceResult<PropertyFeedbackDto>.Fail(
                    "Feedback not found",
                    resultType: ServiceResultType.NotFound);
            }

            feedback.Rating = dto.Rating;
            feedback.Content = NormalizeContent(dto.Content);
            feedback.UpdatedAt = DateTime.UtcNow;

            await _propertyFeedbackRepo.UpdateAsync(feedback);
            await NotifyOwnerAsync(property, feedback, isUpdate: true);

            return ServiceResult<PropertyFeedbackDto>.Ok(
                ToDto(feedback),
                "Feedback updated successfully",
                code: "ZZ_PROPERTY_FEEDBACK_UPDATED_SUCCESSFULLY");
        }

        public async Task<ServiceResult<bool>> DeleteAsync(long propertyId, Guid userId)
        {
            if (!await _propertyRepo.ExistsAsync(propertyId))
            {
                return ServiceResult<bool>.Fail(
                    "Property not found",
                    resultType: ServiceResultType.NotFound);
            }

            var feedback = await _propertyFeedbackRepo.GetByPropertyAndUserAsync(propertyId, userId);
            if (feedback == null)
            {
                return ServiceResult<bool>.Fail(
                    "Feedback not found",
                    resultType: ServiceResultType.NotFound);
            }

            if (!await _contractRepo.HasEligiblePropertyContractAsync(userId, propertyId))
            {
                return ServiceResult<bool>.Fail(
                    "You are not allowed to rate or comment on this property",
                    resultType: ServiceResultType.Forbidden);
            }

            await _propertyFeedbackRepo.DeleteAsync(feedback);
            return ServiceResult<bool>.Ok(true, "Feedback deleted successfully", code: "ZZ_PROPERTY_FEEDBACK_DELETED_SUCCESSFULLY");
        }

        private static string? NormalizeContent(string? content)
        {
            var trimmed = content?.Trim();
            return string.IsNullOrWhiteSpace(trimmed) ? null : trimmed;
        }

        private static PropertyFeedbackDto ToDto(PropertyFeedback feedback)
        {
            return new PropertyFeedbackDto
            {
                FeedbackId = feedback.Id,
                PropertyId = feedback.PropertyId,
                UserId = feedback.UserId,
                UserDisplayName = feedback.User == null ? string.Empty : $"{feedback.User.FirstName} {feedback.User.LastName}".Trim(),
                UserProfileImage = feedback.User?.ProfileImage,
                Rating = feedback.Rating,
                Content = feedback.Content,
                CreatedAt = feedback.CreatedAt,
                UpdatedAt = feedback.UpdatedAt
            };
        }

        private Task NotifyOwnerAsync(Property property, PropertyFeedback feedback, bool isUpdate)
        {
            var titleKey = isUpdate ? "NOTIFICATION_FEEDBACK_UPDATED_TITLE" : "NOTIFICATION_NEW_FEEDBACK_TITLE";
            var bodyKey = isUpdate ? "NOTIFICATION_FEEDBACK_UPDATED_BODY" : "NOTIFICATION_NEW_FEEDBACK_BODY";
            var title = isUpdate ? "Feedback Updated on Your Property" : "New Feedback on Your Property";
            var body = isUpdate
                ? $"A user has updated their feedback to {feedback.Rating} stars on your property \"{property.Title}\"."
                : $"A user has left {feedback.Rating} star feedback on your property \"{property.Title}\".";

            return _notificationService.SendNotificationAsync(new NotificationRequestDto
            {
                UserId = property.OwnerId.ToString(),
                UserType = NotificationUserType.Owner,
                Type = NotificationType.NewReview,
                TitleKey = titleKey,
                BodyKey = bodyKey,
                LocalizationArguments = new() { feedback.Rating.ToString(), property.Title },
                Title = title,
                Body = body,
                ActionType = NotificationActionType.Property,
                ActionId = property.Id.ToString()
            });
        }
    }
}
