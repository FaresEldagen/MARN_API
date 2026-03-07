using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IRoommatePreferenceRepo
    {
        public Task<RoommatePreference?> GetRoommatePreferences(Guid userId);
    }
}
