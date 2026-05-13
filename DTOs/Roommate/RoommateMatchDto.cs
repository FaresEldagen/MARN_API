using System;
using System.Collections.Generic;
using MARN_API.Enums.RoommatePreferences;

namespace MARN_API.DTOs.Roommate
{
    public class RoommateMatchDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? ProfileImage { get; set; }
        public double CompatibilityScore { get; set; }
        public RoommateSearchStatus SearchStatus { get; set; }
        public string Badge { get; set; } = string.Empty;
        public List<string> TopMatchingTraits { get; set; } = new List<string>();
        public List<string> MismatchedTraits { get; set; } = new();
        public List<string> DealbreakersFound { get; set; } = new List<string>();
    }
}
