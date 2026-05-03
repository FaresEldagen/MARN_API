namespace MARN_API.Enums.RoommatePreferences
{
    public enum RoommateSearchStatus
    {
        Searching = 0, // Looking for roommate AND place
        Offering = 1,  // Has a place, looking for roommate
        Found = 2      // Has found a match, hidden from search
    }
}
