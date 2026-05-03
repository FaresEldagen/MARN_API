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

        public async Task<RoommatePreference> UpdateRoommatePreferences(RoommatePreference updatedPreferences)
        {
            try
            {
                Context.RoommatePreferences.Update(updatedPreferences);
                await Context.SaveChangesAsync();
                return updatedPreferences;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update roommate preferences", ex);
            }
        }

        public async Task<RoommatePreference> CreateRoommatePreferences(RoommatePreference newPreferences)
        {
            try
            {
                Context.RoommatePreferences.Add(newPreferences);
                await Context.SaveChangesAsync();
                return newPreferences;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update roommate preferences", ex);
            }
        }

        public async Task<List<RoommatePreference>> GetPotentialMatchesAsync(Guid currentUserId, MARN_API.Enums.Property.Governorate governorate, MARN_API.Enums.Account.Gender currentGender)
        {
            return await Context.RoommatePreferences
                .Include(rp => rp.User)
                .Where(rp => rp.RoommatePreferencesEnabled 
                          && rp.UserId != currentUserId 
                          && rp.Governorate == governorate
                          && rp.User.Gender == currentGender
                          && rp.SearchStatus != MARN_API.Enums.RoommatePreferences.RoommateSearchStatus.Found)
                .ToListAsync();
        }
    }
}
