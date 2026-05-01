using Microsoft.AspNetCore.Identity;

namespace MARN_API.Services.Interfaces
{
    public interface IOwnerService
    {
        public Task<IdentityResult> AddOwnerRole(Guid id);
    }
}
