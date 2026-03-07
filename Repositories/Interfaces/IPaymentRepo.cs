using MARN_API.DTOs;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IPaymentRepo
    {
        #region User Dashboard
        public Task<DateTime?> GetNextPayment(Guid userId);
        #endregion


        #region Owner Dashboard
        public Task<List<MonthlyEarningDto>> GetEarningOverviewMonthly(Guid userId);
        public Task<List<YearlyEarningDto>> GetEarningOverviewYearly(Guid userId);
        #endregion
    }
}
