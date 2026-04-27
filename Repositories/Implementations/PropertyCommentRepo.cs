using Microsoft.EntityFrameworkCore;
using MARN_API.Data;
using MARN_API.DTOs.Common;
using MARN_API.DTOs.PropertyFeedback;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;

namespace MARN_API.Repositories.Implementations
{
    public class PropertyCommentRepo : IPropertyCommentRepo
    {
        private readonly AppDbContext Context;

        public PropertyCommentRepo(AppDbContext context)
        {
            Context = context;
        }

        public async Task<PagedResult<PropertyCommentDto>> GetByPropertyIdAsync(long propertyId, int pageNumber, int pageSize)
        {
            var query = Context.PropertyComments
                .AsNoTracking()
                .Where(c => c.PropertyId == propertyId)
                .OrderByDescending(c => c.CreatedAt);

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new PropertyCommentDto
                {
                    CommentId = c.Id,
                    UserId = c.UserId,
                    UserDisplayName = $"{c.User.FirstName} {c.User.LastName}".Trim(),
                    UserProfileImage = c.User.ProfileImage,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .ToListAsync();

            return new PagedResult<PropertyCommentDto>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        public Task<PropertyComment?> GetByIdAsync(long commentId)
        {
            return Context.PropertyComments
                .FirstOrDefaultAsync(c => c.Id == commentId);
        }

        public async Task<PropertyComment> CreateAsync(PropertyComment comment)
        {
            Context.PropertyComments.Add(comment);
            await Context.SaveChangesAsync();
            return comment;
        }

        public async Task<PropertyComment> UpdateAsync(PropertyComment comment)
        {
            Context.PropertyComments.Update(comment);
            await Context.SaveChangesAsync();
            return comment;
        }

        public async Task DeleteAsync(PropertyComment comment)
        {
            Context.PropertyComments.Remove(comment);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteByUserIdAsync(Guid userId)
        {
            await Context.PropertyComments
                .Where(c => c.UserId == userId)
                .ExecuteDeleteAsync();
        }
    }
}
