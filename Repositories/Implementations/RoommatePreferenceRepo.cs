using MARN_API.Data;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MARN_API.Repositories.Implementations
{
    public class RoommatePreferenceRepo : IRoommatePreferenceRepo
    {
        private readonly AppDbContext Context;
        public RoommatePreferenceRepo(AppDbContext context)
        {
            Context = context;
        }


        public Task<RoommatePreference?> GetRoommatePreferences(Guid userId)
        {
            return Context.RoommatePreferences.FirstOrDefaultAsync(r => r.UserId == userId);
        }
    }
}
