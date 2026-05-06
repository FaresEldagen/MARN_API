using MARN_API.DTOs.Dashboard;
using MARN_API.Models;

namespace MARN_API.Repositories.Interfaces
{
    public interface IPaymentRepo
    {
        #region User Dashboard
        public Task<RenterNextPaymentDto?> GetNextPayment(Guid userId);

        public Task<List<PaidPaymentDto>> GetPaidPayments(Guid userId);
        #endregion



        #region Owner Dashboard
        public Task<List<MonthlyEarningDto>> GetEarningOverviewMonthly(Guid userId);
        public Task<List<YearlyEarningDto>> GetEarningOverviewYearly(Guid userId);
        public Task<decimal> GetWithdrawableEarnings(Guid userId);
        public Task<decimal> GetOnHoldEarnings(Guid userId);
        public Task<List<ReceivedPaymentDto>> GetReceivedPayments(Guid userId);
        #endregion



        #region Payment Checkout
        public Task<PaymentSchedule?> GetPaymentScheduleById(long paymentScheduleId);
        public Task<List<PaymentSchedule>> GetPendingPaymentSchedules(int skip, int take);
        public Task UpdatePaymentSchedule(PaymentSchedule paymentSchedule);

        public Task AddPayment (Payment payment, PaymentSchedule paymentSchedule);
        public Task<bool> PaymentExistsByIntentId(string paymentIntentId);
        #endregion
    }
}
