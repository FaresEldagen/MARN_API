using MARN_API.Models;
using Microsoft.AspNetCore.Identity;

namespace MARN_API.Services.Interfaces
{
    public interface IOwnerService
    {
        public Task<ServiceResult<string>> AddOwnerRole(Guid id);
    }
}
