using MARN_API.Data;
using MARN_API.DTOs.Dashboard;
using MARN_API.DTOs.Property;
using MARN_API.Enums;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MARN_API.Repositories.Implementations
{
    public class PropertyRepo : IPropertyRepo
    {
        private readonly AppDbContext Context;
        public PropertyRepo(AppDbContext context)
        {
            Context = context;
        }


        #region Owner Dashboard and Profile
        public Task<List<OwnerDashboardPropertyCardDto>> GetOwnerDashboardProperties(Guid userId)
        {
            return Context.Properties
                .AsNoTracking()
                .Where(p => p.OwnerId == userId)
                .Select(p => new OwnerDashboardPropertyCardDto
                {
                    Id = p.Id,
                    ImagePath = p.Media
                        .Where(m => m.IsPrimary)
                        .Select(m => m.Path)
                        .FirstOrDefault() ?? string.Empty,
                    Title = p.Title,
                    Address = p.Address,
                    Type = p.Type,
                    Views = p.Views,

                    OccupiedPlaces = p.Contracts
                        .Where(c => c.Status == ContractStatus.Active)
                        .Select(c => c.Property.IsShared ? 1 : c.Property.MaxOccupants)
                        .Sum(),
                    TotalPlaces = p.MaxOccupants,
                    
                    Price = p.Price,
                    RentalUnit = p.RentalUnit,
                    
                    AverageRating = p.Reviews.Any() ? p.Reviews.Average(r => (float?)r.Rating) ?? 0f : 0f,
                    Ratings = p.Reviews.Count,

                    ActiveContracts = p.Contracts
                        .Where(c => c.PropertyId == p.Id && c.Status == ContractStatus.Active)
                        .Select(c => new OwnerPropertyContractDto
                        {
                            ContractId = c.Id,
                            RenterId = c.RenterId,
                            RenterName = $"{c.Renter.FirstName} {c.Renter.LastName}",
                            RenterProfileImage = c.Renter.ProfileImage
                        })
                        .ToList()
                })
                .ToListAsync();
        }

        public Task<List<PropertyCardDto>> GetOwnerProfileProperties(Guid userId)
        {
            return Context.Properties
                .AsNoTracking()
                .Where(p => p.OwnerId == userId)
                .Select(p => new PropertyCardDto
                {
                    Id = p.Id,
                    ImagePath = p.Media
                        .Where(m => m.IsPrimary)
                        .Select(m => m.Path)
                        .FirstOrDefault() ?? string.Empty,
                    Title = p.Title,
                    Address = p.Address,

                    MaxOccupants = p.MaxOccupants,
                    Bedrooms = p.Bedrooms,
                    Bathrooms = p.Bathrooms,

                    Type = p.Type,
                    AverageRating = p.Reviews.Any() ? p.Reviews.Average(r => (float?)r.Rating) ?? 0f : 0f,
                    Ratings = p.Reviews.Count,

                    Price = p.Price,
                    RentalUnit = p.RentalUnit,
                })
                .ToListAsync();
        }

        public Task<int> GetOwnedPropertiesViewsCount(Guid userId)
        {
            return Context.Properties
                .Where(p => p.OwnerId == userId && p.DeletedAt == null)
                .SumAsync(p => p.Views);
        }

        public Task<int> GetOwnedPropertiesPlacesCount(Guid userId)
        {
            return Context.Properties
                .Where(p => p.OwnerId == userId && p.DeletedAt == null)
                .SumAsync(p => p.MaxOccupants);
        }

        public async Task<float> GetOwnerAverageRating(Guid userid)
        {
            var avg = await Context.Properties
                .Where(p => p.OwnerId == userid)
                .SelectMany(p => p.Reviews)
                .AverageAsync(r => (float?)r.Rating);

            return avg ?? 0f;
        }

        public Task<int> GetOwnerRatingsCount(Guid userId)
        {
            return Context.Properties
                .Where(p => p.OwnerId == userId)
                .SelectMany(p => p.Reviews)
                .CountAsync();
        }
        #endregion
    }
}
