using MARN_API.Data;
using MARN_API.DTOs.Dashboard;
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


        public Task<List<OwnerPropertyCardDto>> GetOwnedProperties(Guid userId)
        {
            return Context.Properties
                .AsNoTracking()
                .Where(p => p.OwnerId == userId)
                .Select(p => new OwnerPropertyCardDto
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
                    
                    AverageRating = p.AverageRating,
                    Ratings = p.Reviews.Count,

                    ActiveContracts = p.Contracts
                        .Where(c => c.PropertyId == p.Id && c.Status == ContractStatus.Active)
                        .Select(c => new OwnerPropertyContractDto
                        {
                            ContractId = c.Id,
                            RenterName = $"{c.Renter.FirstName} {c.Renter.LastName}",
                        })
                        .ToList()
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
    }
}
