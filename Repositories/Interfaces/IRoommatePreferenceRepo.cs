using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IRoommatePreferenceRepo
    {
        public Task<RoommatePreference?> GetRoommatePreferences(Guid userId);
        public Task<RoommatePreference> UpdateRoommatePreferences(RoommatePreference updatedPreferences);
        public Task<RoommatePreference> CreateRoommatePreferences(RoommatePreference newPreferences);
    }
}
