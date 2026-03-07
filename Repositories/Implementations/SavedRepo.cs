using MARN_API.Data;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MARN_API.Repositories.Implementations
{
    public class SavedRepo : ISavedRepo
    {
        private readonly AppDbContext Context;
        public SavedRepo(AppDbContext context)
        {
            Context = context;
        }


        public Task<List<Property>> GetSavedProperties(Guid userId)
        {
            return Context.SavedProperties
                .Where(s => s.UserId == userId)
                .Select(s => s.Property)
                .ToListAsync();
        }
    }
}
