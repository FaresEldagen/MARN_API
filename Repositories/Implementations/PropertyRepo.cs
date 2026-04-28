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
        private readonly IConfiguration _configuration;
        public PropertyRepo(AppDbContext context, IConfiguration configuration)
        {
            Context = context;
            _configuration = configuration;
        }


        #region Owner Dashboard and Profile
        public Task<List<OwnerDashboardPropertyCardDto>> GetOwnerDashboardProperties(Guid userId)
        {
            var BaseUrl = _configuration["AppSettings:BaseUrl"] ?? throw new InvalidOperationException("BaseUrl is not configured.");

            return Context.Properties
                .AsNoTracking()
                .Where(p => p.OwnerId == userId)
                .Select(p => new OwnerDashboardPropertyCardDto
                {
                    Id = p.Id,
                    ImagePath = $"{BaseUrl}{p.Media
                        .Where(m => m.IsPrimary)
                        .Select(m => m.Path)
                        .FirstOrDefault() ?? string.Empty}",
                    Title = p.Title,
                    Address = p.Address,
                    Type = p.Type,
                    Views = p.Views,
                    IsSaved = p.SavedProperty.Any(s => s.UserId == userId),

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
            var BaseUrl = _configuration["AppSettings:BaseUrl"] ?? throw new InvalidOperationException("BaseUrl is not configured.");

            return Context.Properties
                .AsNoTracking()
                .Where(p => p.OwnerId == userId)
                .Select(p => new PropertyCardDto
                {
                    Id = p.Id,
                    ImagePath = $"{BaseUrl}{p.Media
                        .Where(m => m.IsPrimary)
                        .Select(m => m.Path)
                        .FirstOrDefault() ?? string.Empty}",
                    Title = p.Title,
                    Address = p.Address,
                    IsSaved = p.SavedProperty.Any(s => s.UserId == userId),

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


        #region Deletion
        public async Task<List<long>> GetPropertyIdsByOwnerAsync(Guid ownerId)
        {
            return await Context.Properties
                .IgnoreQueryFilters()
                .Where(p => p.OwnerId == ownerId)
                .Select(p => p.Id)
                .ToListAsync();
        }

        public async Task<List<string>> GetMediaPathsByPropertyIdsAsync(List<long> propertyIds)
        {
            return await Context.PropertyMedia
                .Where(m => propertyIds.Contains(m.PropertyId))
                .Select(m => m.Path)
                .ToListAsync();
        }

        public async Task DeleteMediaByPropertyIdsAsync(List<long> propertyIds)
        {
            await Context.PropertyMedia
                .Where(m => propertyIds.Contains(m.PropertyId))
                .ExecuteDeleteAsync();
        }
        #endregion

        
        #region Property Operation
        public async Task<Property?> GetByIdAsync(long id)
        {
            return await Context.Properties.Include(p => p.Contracts).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task UpdatePropertyAsync(Property property)
        {
            Context.Properties.Update(property);
            await Context.SaveChangesAsync();
        }

        public async Task AddPropertyAsync(Property property)
        {
            await Context.Properties.AddAsync(property);
            await Context.SaveChangesAsync();
        }
        #endregion
    }
}
