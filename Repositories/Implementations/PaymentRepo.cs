using MARN_API.Data;
using MARN_API.DTOs.Dashboard;
using MARN_API.Enums;
using MARN_API.Models;
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
        public Task<RenterNextPaymentDto?> GetNextPayment(Guid userId)
        {
            return Context.Payments
                .AsNoTracking()
                .Where(p => p.Contract.RenterId == userId && p.Status == PaymentStatus.Pending)
                .OrderBy(p => p.DueDate)
                .Select(p => new RenterNextPaymentDto
                {
                    Date = p.DueDate,
                    Amount = p.TotalAmount,

                    PropertyTitle = p.Contract.Property.Title,
                    PropertyId = p.Contract.PropertyId,
                })
                .FirstOrDefaultAsync();
        }
        #endregion


        #region Owner Dashboard
        public Task<List<MonthlyEarningDto>> GetEarningOverviewMonthly(Guid userId)
        {
            return Context.Payments
                .Where(p => p.Contract.OwnerId == userId && p.Status == PaymentStatus.Succeeded)
                .GroupBy(p => new { p.PaidAt!.Value.Year, p.PaidAt.Value.Month })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .Select(g => new MonthlyEarningDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Total = g.Sum(p => p.OwnerAmount)
                })
                .ToListAsync();
        }

        public Task<List<YearlyEarningDto>> GetEarningOverviewYearly(Guid userId)
        {
            return Context.Payments
                .Where(p => p.Contract.OwnerId == userId && p.Status == PaymentStatus.Succeeded)
                .GroupBy(p => p.PaidAt!.Value.Year)
                .OrderBy(g => g.Key)
                .Select(g => new YearlyEarningDto
                {
                    Year = g.Key,
                    Total = g.Sum(p => p.OwnerAmount)
                })
                .ToListAsync();
        }

        public Task<decimal> GetWithdrawableEarnings(Guid userId)
        {
            return Context.Payments
                .Where(p =>
                    p.Contract.OwnerId == userId &&
                    p.Status == PaymentStatus.Succeeded &&
                    p.AvailableAt <= DateTime.UtcNow)
                .SumAsync(p => p.OwnerAmount);
        }

        public Task<decimal> GetOnHoldEarnings(Guid userId)
        {
            return Context.Payments
                .Where(p =>
                    p.Contract.OwnerId == userId &&
                    p.Status == PaymentStatus.Succeeded &&
                    p.AvailableAt > DateTime.UtcNow)
                .SumAsync(p => p.OwnerAmount);
        }
        #endregion
    }
}
