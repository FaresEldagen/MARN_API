using MARN_API.DTOs.Common;
using MARN_API.DTOs.PropertyFeedback;
using MARN_API.Enums;
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

        public PropertyCommentService(
            IPropertyCommentRepo propertyCommentRepo,
            IPropertyRepo propertyRepo,
            IContractRepo contractRepo)
        {
            _propertyCommentRepo = propertyCommentRepo;
            _propertyRepo = propertyRepo;
            _contractRepo = contractRepo;
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

        public async Task<ServiceResult<PropertyComment>> CreateAsync(long propertyId, Guid userId, CreatePropertyCommentDto dto)
        {
            var validation = await ValidatePropertyInteractionAsync(propertyId, userId);
            if (validation != null)
                return validation;

            var comment = new PropertyComment
            {
                PropertyId = propertyId,
                UserId = userId,
                Content = dto.Content.Trim()
            };

            await _propertyCommentRepo.CreateAsync(comment);
            return ServiceResult<PropertyComment>.Ok(comment, "Comment created successfully", ServiceResultType.Created);
        }

        public async Task<ServiceResult<PropertyComment>> UpdateAsync(long propertyId, long commentId, Guid userId, UpdatePropertyCommentDto dto)
        {
            var validation = await ValidatePropertyInteractionAsync(propertyId, userId);
            if (validation != null)
                return validation;

            var comment = await _propertyCommentRepo.GetByIdAsync(commentId);
            if (comment == null || comment.PropertyId != propertyId)
            {
                return ServiceResult<PropertyComment>.Fail(
                    "Comment not found",
                    resultType: ServiceResultType.NotFound);
            }

            if (comment.UserId != userId)
            {
                return ServiceResult<PropertyComment>.Fail(
                    "You are not allowed to edit this comment",
                    resultType: ServiceResultType.Forbidden);
            }

            comment.Content = dto.Content.Trim();
            comment.UpdatedAt = DateTime.UtcNow;

            await _propertyCommentRepo.UpdateAsync(comment);
            return ServiceResult<PropertyComment>.Ok(comment, "Comment updated successfully");
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
