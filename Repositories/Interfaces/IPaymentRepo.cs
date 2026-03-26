using MARN_API.DTOs.Dashboard;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IPaymentRepo
    {
        #region User Dashboard
        public Task<RenterNextPaymentDto?> GetNextPayment(Guid userId);

        #endregion


        #region Owner Dashboard
        public Task<List<MonthlyEarningDto>> GetEarningOverviewMonthly(Guid userId);
        public Task<List<YearlyEarningDto>> GetEarningOverviewYearly(Guid userId);
        public Task<decimal> GetWithdrawableEarnings(Guid userId);
        public Task<decimal> GetOnHoldEarnings(Guid userId);
        #endregion
    }
}
