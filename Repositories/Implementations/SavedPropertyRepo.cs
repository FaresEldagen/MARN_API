using MARN_API.Data;
using MARN_API.DTOs.Property;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MARN_API.Repositories.Implementations
{
    public class SavedPropertyRepo : ISavedPropertyRepo
    {
        private readonly AppDbContext Context;
        public SavedPropertyRepo(AppDbContext context)
        {
            Context = context;
        }


        public Task<List<PropertyCardDto>> GetSavedProperties(Guid userId)
        {
            return Context.SavedProperties
                .AsNoTracking()
                .Where(s => s.UserId == userId)
                .Select(s => new PropertyCardDto
                {
                    Id = s.Property.Id,
                    ImagePath = s.Property.Media
                        .Where(m => m.IsPrimary)
                        .Select(m => m.Path)
                        .FirstOrDefault() ?? string.Empty,
                    Title = s.Property.Title,
                    Address = s.Property.Address,
                    IsSaved = true,

                    MaxOccupants = s.Property.MaxOccupants,
                    Bedrooms = s.Property.Bedrooms,
                    Bathrooms = s.Property.Bathrooms,

                    Type = s.Property.Type,
                    AverageRating = s.Property.PropertyRatings.Any() ? s.Property.PropertyRatings.Average(r => (float?)r.Rating) ?? 0f : 0f,
                    Ratings = s.Property.PropertyRatings.Count,

                    Price = s.Property.Price,
                    RentalUnit = s.Property.RentalUnit,
                })
                .ToListAsync();
        }

        public async Task<bool> HasSavedPropertyAsync(Guid userId, long propertyId)
        {
            return await Context.SavedProperties
                .AnyAsync(s => s.UserId == userId && s.PropertyId == propertyId);
        }

        public async Task SavePropertyAsync(SavedProperty savedProperty)
        {
            await Context.SavedProperties.AddAsync(savedProperty);
            await Context.SaveChangesAsync();
        }

        public async Task UnsavePropertyAsync(Guid userId, long propertyId)
        {
            var savedProperty = await Context.SavedProperties
                .FirstOrDefaultAsync(s => s.UserId == userId && s.PropertyId == propertyId);
            
            if (savedProperty != null)
            {
                Context.SavedProperties.Remove(savedProperty);
                await Context.SaveChangesAsync();
            }
        }
    }
}
