using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MARN_API.DTOs.Roommate;
using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface IRoommateMatchingService
    {
        Task<ServiceResult<IEnumerable<RoommateMatchDto>>> GetTopMatchesAsync(Guid currentUserId, int k = 10);
        Task<ServiceResult<Dictionary<Guid, RoommateMatchDto>>> GetMatchScoresAsync(Guid currentUserId, List<Guid> targetUserIds);
    }
}
