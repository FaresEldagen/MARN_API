using Microsoft.EntityFrameworkCore;
using MARN_API.Data;
using MARN_API.DTOs.PropertyFeedback;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;

namespace MARN_API.Repositories.Implementations
{
    public class PropertyRatingRepo : IPropertyRatingRepo
    {
        private readonly AppDbContext Context;

        public PropertyRatingRepo(AppDbContext context)
        {
            Context = context;
        }

        public async Task<PropertyRatingSummaryDto> GetSummaryAsync(long propertyId, Guid? currentUserId = null)
        {
            var query = Context.PropertyRatings
                .AsNoTracking()
                .Where(r => r.PropertyId == propertyId);

            var ratingsCount = await query.CountAsync();
            var averageRating = ratingsCount == 0
                ? 0f
                : await query.AverageAsync(r => (float)r.Rating);

            int? currentUserRating = null;
            if (currentUserId.HasValue)
            {
                currentUserRating = await query
                    .Where(r => r.UserId == currentUserId.Value)
                    .Select(r => (int?)r.Rating)
                    .FirstOrDefaultAsync();
            }

            return new PropertyRatingSummaryDto
            {
                AverageRating = averageRating,
                RatingsCount = ratingsCount,
                CurrentUserRating = currentUserRating
            };
        }

        public Task<PropertyRating?> GetByPropertyAndUserAsync(long propertyId, Guid userId)
        {
            return Context.PropertyRatings
                .FirstOrDefaultAsync(r => r.PropertyId == propertyId && r.UserId == userId);
        }

        public Task<bool> ExistsAsync(long propertyId, Guid userId)
        {
            return Context.PropertyRatings
                .AnyAsync(r => r.PropertyId == propertyId && r.UserId == userId);
        }

        public async Task<PropertyRating> CreateAsync(PropertyRating rating)
        {
            Context.PropertyRatings.Add(rating);
            await Context.SaveChangesAsync();
            return rating;
        }

        public async Task<PropertyRating> UpdateAsync(PropertyRating rating)
        {
            Context.PropertyRatings.Update(rating);
            await Context.SaveChangesAsync();
            return rating;
        }

        public async Task DeleteAsync(PropertyRating rating)
        {
            Context.PropertyRatings.Remove(rating);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteByUserIdAsync(Guid userId)
        {
            await Context.PropertyRatings
                .Where(r => r.UserId == userId)
                .ExecuteDeleteAsync();
        }
    }
}
