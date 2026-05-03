using MARN_API.Data;
using MARN_API.DTOs.Dashboard;
using MARN_API.Enums.Payment;
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
            return Context.PaymentSchedules
                .AsNoTracking()
                .Where(p => p.Contract.RenterId == userId && 
                        p.Status == PaymentScheduleStatus.Pending)
                .OrderBy(p => p.DueDate)
                .Select(p => new RenterNextPaymentDto
                {
                    Date = p.DueDate,
                    Amount = p.Amount,

                    PropertyId = p.Contract.PropertyId,
                    PropertyTitle = p.Contract.Property.Title,
                })
                .FirstOrDefaultAsync();
        }
        public Task<List<PaidPaymentDto>> GetPaidPayments(Guid userId)
        {
            return Context.Payments
                .AsNoTracking()
                .Where(p => p.PaymentSchedule.Contract.RenterId == userId && p.Status == PaymentStatus.Succeeded)
                .OrderByDescending(p => p.PaidAt)
                .Select(p => new PaidPaymentDto
                {
                    AmountPaid = p.AmountTotal,
                    ContractId = p.PaymentSchedule.ContractId,
                    PaidAt = p.PaidAt
                })
                .ToListAsync();
        }
        #endregion



        #region Owner Dashboard
        public Task<List<MonthlyEarningDto>> GetEarningOverviewMonthly(Guid userId)
        {
            return Context.Payments
                .Where(p => p.PaymentSchedule.Contract.Property.OwnerId == userId && p.Status == PaymentStatus.Succeeded)
                .GroupBy(p => new { p.PaidAt.Year, p.PaidAt.Month })
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
                .Where(p => p.PaymentSchedule.Contract.Property.OwnerId == userId && p.Status == PaymentStatus.Succeeded)
                .GroupBy(p => p.PaidAt.Year)
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
                    p.PaymentSchedule.Contract.Property.OwnerId == userId &&
                    p.Status == PaymentStatus.Succeeded &&
                    p.AvailableAt <= DateTime.UtcNow)
                .SumAsync(p => p.OwnerAmount);
        }

        public Task<decimal> GetOnHoldEarnings(Guid userId)
        {
            return Context.Payments
                .Where(p =>
                    p.PaymentSchedule.Contract.Property.OwnerId == userId &&
                    p.Status == PaymentStatus.Succeeded &&
                    p.AvailableAt > DateTime.UtcNow)
                .SumAsync(p => p.OwnerAmount);
        }
        public Task<List<ReceivedPaymentDto>> GetReceivedPayments(Guid userId)
        {
            return Context.Payments
                .AsNoTracking()
                .Where(p => p.PaymentSchedule.Contract.Property.OwnerId == userId && p.Status == PaymentStatus.Succeeded)
                .OrderByDescending(p => p.PaidAt)
                .Select(p => new ReceivedPaymentDto
                {
                    AmountReceived = p.OwnerAmount,
                    ContractId = p.PaymentSchedule.ContractId,
                    PaidAt = p.PaidAt,
                    AvailableAt = p.AvailableAt
                })
                .ToListAsync();
        }
        #endregion



        #region Payment Checkout
        public Task<PaymentSchedule?> GetPaymentScheduleById(long paymentScheduleId)
        {
            return Context.PaymentSchedules
                .AsNoTracking()
                .Include(ps => ps.Contract)
                .FirstOrDefaultAsync(ps => ps.Id == paymentScheduleId);
        }
        #endregion
    }
}
