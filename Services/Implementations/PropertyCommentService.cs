using AutoMapper;
using MARN_API.DTOs.Common;
using MARN_API.DTOs.Notification;
using MARN_API.DTOs.PropertyFeedback;
using MARN_API.Enums;
using MARN_API.Enums.Notification;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;

namespace MARN_API.Services.Implementations
{
    public class PropertyCommentService : IPropertyCommentService
    {
        private readonly IPropertyCommentRepo _propertyCommentRepo;
        private readonly IPropertyRepo _propertyRepo;
        private readonly IContractRepo _contractRepo;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public PropertyCommentService(
            IPropertyCommentRepo propertyCommentRepo,
            IPropertyRepo propertyRepo,
            IContractRepo contractRepo,
            IMapper mapper,
            INotificationService notificationService)
        {
            _propertyCommentRepo = propertyCommentRepo;
            _propertyRepo = propertyRepo;
            _contractRepo = contractRepo;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<ServiceResult<PagedResult<PropertyCommentDto>>> GetByPropertyIdAsync(long propertyId, int pageNumber, int pageSize)
        {
            if (!await _propertyRepo.ExistsAsync(propertyId))
            {
                return ServiceResult<PagedResult<PropertyCommentDto>>.Fail(
                    "Property not found",
                    resultType: ServiceResultType.NotFound);
            }

            var comments = await _propertyCommentRepo.GetByPropertyIdAsync(propertyId, pageNumber, pageSize);
            return ServiceResult<PagedResult<PropertyCommentDto>>.Ok(comments);
        }

        public async Task<ServiceResult<PropertyCommentMutationDto>> CreateAsync(long propertyId, Guid userId, CreatePropertyCommentDto dto)
        {
            var property = await _propertyRepo.GetByIdAsync(propertyId);
            if (property == null)
            {
                return ServiceResult<PropertyCommentMutationDto>.Fail(
                    "Property not found",
                    resultType: ServiceResultType.NotFound);
            }

            if (!await _contractRepo.HasEligiblePropertyContractAsync(userId, propertyId))
            {
                return ServiceResult<PropertyCommentMutationDto>.Fail(
                    "You are not allowed to rate or comment on this property",
                    resultType: ServiceResultType.Forbidden);
            }

            var trimmedContent = dto.Content.Trim();
            if (string.IsNullOrWhiteSpace(trimmedContent))
            {
                return ServiceResult<PropertyCommentMutationDto>.Fail(
                    "Comment content is required.",
                    resultType: ServiceResultType.BadRequest);
            }

            var comment = new PropertyComment
            {
                PropertyId = propertyId,
                UserId = userId,
                Content = trimmedContent
            };

            await _propertyCommentRepo.CreateAsync(comment);

            await _notificationService.SendNotificationAsync(new NotificationRequestDto
            {
                UserId = property.OwnerId.ToString(),
                UserType = NotificationUserType.Owner,
                Type = NotificationType.NewReview,

                Title = "New Comment on Your Property",
                Body = $"A user has left a new comment on your property \"{property.Title}\".",

                ActionType = NotificationActionType.Property,
                ActionId = propertyId.ToString()
            });

            return ServiceResult<PropertyCommentMutationDto>.Ok(_mapper.Map<PropertyCommentMutationDto>(comment), "Comment created successfully", ServiceResultType.Created);
        }

        public async Task<ServiceResult<PropertyCommentMutationDto>> UpdateAsync(long propertyId, long commentId, Guid userId, UpdatePropertyCommentDto dto)
        {
            var property = await _propertyRepo.GetByIdAsync(propertyId);
            if (property == null)
            {
                return ServiceResult<PropertyCommentMutationDto>.Fail(
                    "Property not found",
                    resultType: ServiceResultType.NotFound);
            }

            if (!await _contractRepo.HasEligiblePropertyContractAsync(userId, propertyId))
            {
                return ServiceResult<PropertyCommentMutationDto>.Fail(
                    "You are not allowed to rate or comment on this property",
                    resultType: ServiceResultType.Forbidden);
            }

            var comment = await _propertyCommentRepo.GetByIdAsync(commentId);
            if (comment == null || comment.PropertyId != propertyId)
            {
                return ServiceResult<PropertyCommentMutationDto>.Fail(
                    "Comment not found",
                    resultType: ServiceResultType.NotFound);
            }

            if (comment.UserId != userId)
            {
                return ServiceResult<PropertyCommentMutationDto>.Fail(
                    "You are not allowed to edit this comment",
                    resultType: ServiceResultType.Forbidden);
            }

            var trimmedContent = dto.Content.Trim();
            if (string.IsNullOrWhiteSpace(trimmedContent))
            {
                return ServiceResult<PropertyCommentMutationDto>.Fail(
                    "Comment content is required.",
                    resultType: ServiceResultType.BadRequest);
            }

            comment.Content = trimmedContent;
            comment.UpdatedAt = DateTime.UtcNow;

            await _propertyCommentRepo.UpdateAsync(comment);

            await _notificationService.SendNotificationAsync(new NotificationRequestDto
            {
                UserId = property.OwnerId.ToString(),
                UserType = NotificationUserType.Owner,
                Type = NotificationType.NewReview,

                Title = "Comment Updated on Your Property",
                Body = $"A user has updated their comment on your property \"{property.Title}\".",

                ActionType = NotificationActionType.Property,
                ActionId = propertyId.ToString()
            });

            return ServiceResult<PropertyCommentMutationDto>.Ok(_mapper.Map<PropertyCommentMutationDto>(comment), "Comment updated successfully");
        }

        public async Task<ServiceResult<bool>> DeleteAsync(long propertyId, long commentId, Guid userId)
        {
            if (!await _propertyRepo.ExistsAsync(propertyId))
            {
                return ServiceResult<bool>.Fail(
                    "Property not found",
                    resultType: ServiceResultType.NotFound);
            }

            var comment = await _propertyCommentRepo.GetByIdAsync(commentId);
            if (comment == null || comment.PropertyId != propertyId)
            {
                return ServiceResult<bool>.Fail(
                    "Comment not found",
                    resultType: ServiceResultType.NotFound);
            }

            if (comment.UserId != userId)
            {
                return ServiceResult<bool>.Fail(
                    "You are not allowed to delete this comment",
                    resultType: ServiceResultType.Forbidden);
            }

            if (!await _contractRepo.HasEligiblePropertyContractAsync(userId, propertyId))
            {
                return ServiceResult<bool>.Fail(
                    "You are not allowed to rate or comment on this property",
                    resultType: ServiceResultType.Forbidden);
            }

            await _propertyCommentRepo.DeleteAsync(comment);
            return ServiceResult<bool>.Ok(true, "Comment deleted successfully");
        }

        private async Task<ServiceResult<PropertyComment>?> ValidatePropertyInteractionAsync(long propertyId, Guid userId)
        {
            if (!await _propertyRepo.ExistsAsync(propertyId))
            {
                return ServiceResult<PropertyComment>.Fail(
                    "Property not found",
                    resultType: ServiceResultType.NotFound);
            }

            if (!await _contractRepo.HasEligiblePropertyContractAsync(userId, propertyId))
            {
                return ServiceResult<PropertyComment>.Fail(
                    "You are not allowed to rate or comment on this property",
                    resultType: ServiceResultType.Forbidden);
            }

            return null;
        }
    }
}
