using MARN_API.DTOs.Common;
using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface IContactSupportService
    {
        Task<ServiceResult<bool>> SendContactUsEmailAsync(ContactSupportRequestDto request,Guid? userId);
    }
}
