using MARN_API.DTOs.Admin;
using MARN_API.Models;

namespace MARN_API.Services.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<ServiceResult<AdminDashboardOverviewDto>> GetOverviewAsync();
    }
}
