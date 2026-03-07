using MARN_API.Data;
using MARN_API.DTOs;
using MARN_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MARN_API.Repositories.Implementations
{
    public class PaymentRepo : IPaymentRepo
    {
        private readonly AppDbContext Context;
        public PaymentRepo(AppDbContext context)
        {
            Context = context;
        }


        #region User Dashboard
        public Task<DateTime?> GetNextPayment(Guid userId)
        {
            return Context.Payments
                .Where(p => p.Contract.RenterId == userId && !p.IsPaid)
                .OrderBy(p => p.DueDate)
                .Select(p => (DateTime?)p.DueDate)
                .FirstOrDefaultAsync();
        }
        #endregion


        #region Owner Dashboard
        public Task<List<MonthlyEarningDto>> GetEarningOverviewMonthly(Guid userId)
        {
            return Context.Payments
                .Where(p => p.Contract.OwnerId == userId && p.IsPaid)
                .GroupBy(p => new { p.PaidAt!.Value.Year, p.PaidAt.Value.Month })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .Select(g => new MonthlyEarningDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Total = g.Sum(p => p.Amount)
                })
                .ToListAsync();
        }

        public Task<List<YearlyEarningDto>> GetEarningOverviewYearly(Guid userId)
        {
            return Context.Payments
                .Where(p => p.Contract.OwnerId == userId && p.IsPaid)
                .GroupBy(p => p.PaidAt!.Value.Year)
                .OrderBy(g => g.Key)
                .Select(g => new YearlyEarningDto
                {
                    Year = g.Key,
                    Total = g.Sum(p => p.Amount)
                })
                .ToListAsync();
        }
        #endregion
    }
}
