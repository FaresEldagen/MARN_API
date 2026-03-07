using MARN_API.Data;
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


        public Task<List<Property>> GetOwnedProperties(Guid userId)
        {
            return Context.Properties
                .Where(p => p.OwnerId == userId)
                .ToListAsync();
        }

        public Task<int> GetOwnedPropertiesViewsCount(Guid userId)
        {
            return Context.Properties
                .Where(p => p.OwnerId == userId)
                .SumAsync(p => p.Views);
        }

        public Task<int> GetOwnedPropertiesPlacesCount(Guid userId)
        {
            return Context.Properties
                .Where(p => p.OwnerId == userId)
                .SumAsync(p => p.MaxOccupants);
        }
    }
}
