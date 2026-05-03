using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MARN_API.DTOs.Roommate;

namespace MARN_API.Services.Interfaces
{
    public interface IRoommateMatchingService
    {
        Task<IEnumerable<RoommateMatchDto>> GetTopMatchesAsync(Guid currentUserId, int k = 10);
    }
}
