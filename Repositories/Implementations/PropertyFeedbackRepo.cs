using Microsoft.EntityFrameworkCore;
using MARN_API.Data;
using MARN_API.DTOs.Common;
using MARN_API.DTOs.PropertyFeedback;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;

namespace MARN_API.Repositories.Implementations
{
    public class PropertyFeedbackRepo : IPropertyFeedbackRepo
    {
        private readonly AppDbContext Context;

        public PropertyFeedbackRepo(AppDbContext context)
        {
            Context = context;
        }

        public async Task<PropertyFeedbackSummaryDto> GetSummaryAsync(long propertyId, Guid? currentUserId, int pageNumber, int pageSize)
        {
            var allFeedback = Context.PropertyFeedbacks
                .AsNoTracking()
                .Where(f => f.PropertyId == propertyId);

            var ratingsCount = await allFeedback.CountAsync();
            var averageRating = ratingsCount == 0
                ? 0f
                : await allFeedback.AverageAsync(f => (float)f.Rating);
            averageRating = (float)Math.Round((double)averageRating, 1);

            var visibleComments = allFeedback
                .Where(f => !f.IsHiddenByModeration && f.Content != null && f.Content != "")
                .OrderByDescending(f => f.CreatedAt);

            var commentsCount = await visibleComments.CountAsync();
            var feedbackItems = await visibleComments
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(f => new PropertyFeedbackDto
                {
                    FeedbackId = f.Id,
                    PropertyId = f.PropertyId,
                    UserId = f.UserId,
                    UserDisplayName = $"{f.User.FirstName} {f.User.LastName}".Trim(),
                    UserProfileImage = f.User.ProfileImage,
                    Rating = f.Rating,
                    Content = f.Content,
                    CreatedAt = f.CreatedAt,
                    UpdatedAt = f.UpdatedAt
                })
                .ToListAsync();

            PropertyFeedbackDto? currentUserFeedback = null;
            if (currentUserId.HasValue)
            {
                currentUserFeedback = await allFeedback
                    .Where(f => f.UserId == currentUserId.Value)
                    .Select(f => new PropertyFeedbackDto
                    {
                        FeedbackId = f.Id,
                        PropertyId = f.PropertyId,
                        UserId = f.UserId,
                        UserDisplayName = $"{f.User.FirstName} {f.User.LastName}".Trim(),
                        UserProfileImage = f.User.ProfileImage,
                        Rating = f.Rating,
                        Content = f.Content,
                        CreatedAt = f.CreatedAt,
                        UpdatedAt = f.UpdatedAt
                    })
                    .FirstOrDefaultAsync();
            }

            return new PropertyFeedbackSummaryDto
            {
                AverageRating = averageRating,
                RatingsCount = ratingsCount,
                CommentsCount = commentsCount,
                CurrentUserFeedback = currentUserFeedback,
                Feedback = new PagedResult<PropertyFeedbackDto>
                {
                    Items = feedbackItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = commentsCount,
                    TotalPages = (int)Math.Ceiling(commentsCount / (double)pageSize)
                }
            };
        }

        public Task<PropertyFeedback?> GetByPropertyAndUserAsync(long propertyId, Guid userId)
        {
            return Context.PropertyFeedbacks
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.PropertyId == propertyId && f.UserId == userId);
        }

        public Task<PropertyFeedback?> GetByIdAsync(long feedbackId)
        {
            return Context.PropertyFeedbacks
                .Include(f => f.Property)
                    .ThenInclude(p => p.Owner)
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.Id == feedbackId);
        }

        public async Task<PropertyFeedback> CreateAsync(PropertyFeedback feedback)
        {
            Context.PropertyFeedbacks.Add(feedback);
            await Context.SaveChangesAsync();
            return feedback;
        }

        public async Task<PropertyFeedback> UpdateAsync(PropertyFeedback feedback)
        {
            Context.PropertyFeedbacks.Update(feedback);
            await Context.SaveChangesAsync();
            return feedback;
        }

        public async Task DeleteAsync(PropertyFeedback feedback)
        {
            Context.PropertyFeedbacks.Remove(feedback);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteByUserIdAsync(Guid userId)
        {
            await Context.PropertyFeedbacks
                .Where(f => f.UserId == userId)
                .ExecuteDeleteAsync();
        }

        public async Task DeleteByPropertyIdAsync(long propertyId)
        {
            await Context.PropertyFeedbacks
                .Where(f => f.PropertyId == propertyId)
                .ExecuteDeleteAsync();
        }
    }
}
