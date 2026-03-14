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

                    MaxOccupants = s.Property.MaxOccupants,
                    Bedrooms = s.Property.Bedrooms,
                    Bathrooms = s.Property.Bathrooms,

                    Type = s.Property.Type,
                    AverageRating = s.Property.AverageRating,
                    Ratings = s.Property.Reviews.Count,

                    Price = s.Property.Price,
                    RentalUnit = s.Property.RentalUnit,
                })
                .ToListAsync();
        }
    }
}
