using MARN_API.Data;
using MARN_API.DTOs.Dashboard;
using MARN_API.Enums.Contract;
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
                        (p.Status == PaymentScheduleStatus.Available ||
                        p.Status == PaymentScheduleStatus.NotAvailableYet ||
                        p.Status == PaymentScheduleStatus.Overdue))
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
                .Where(p => p.PaymentSchedule.Contract.RenterId == userId)
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
                .Where(p => p.PaymentSchedule.Contract.Property.OwnerId == userId)
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
                .Where(p => p.PaymentSchedule.Contract.Property.OwnerId == userId)
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
                    p.Status == PaymentStatus.Available)
                .SumAsync(p => p.OwnerAmount);
        }

        public Task<decimal> GetOnHoldEarnings(Guid userId)
        {
            return Context.Payments
                .Where(p =>
                    p.PaymentSchedule.Contract.Property.OwnerId == userId &&
                    p.Status == PaymentStatus.OnHold)
                .SumAsync(p => p.OwnerAmount);
        }

        public Task<List<ReceivedPaymentDto>> GetReceivedPayments(Guid userId)
        {
            return Context.Payments
                .AsNoTracking()
                .Where(p => p.PaymentSchedule.Contract.Property.OwnerId == userId)
                .OrderByDescending(p => p.PaidAt)
                .Select(p => new ReceivedPaymentDto
                {
                    AmountReceived = p.OwnerAmount,
                    ContractId = p.PaymentSchedule.ContractId,
                    PaidAt = p.PaidAt,
                    AvailableAt = p.AvailableAt,
                    Status = p.Status
                })
                .ToListAsync();
        }
        #endregion


        #region Payment Checkout
        public Task<PaymentSchedule?> GetPaymentScheduleById(long paymentScheduleId)
        {
            return Context.PaymentSchedules
                .Include(ps => ps.Contract)
                    .ThenInclude(c => c.Property)
                .FirstOrDefaultAsync(ps => ps.Id == paymentScheduleId);
        }

        public Task<List<PaymentSchedule>> GetPendingPaymentSchedules(int skip, int take)
        {
            return Context.PaymentSchedules
                .Include(ps => ps.Contract)
                    .ThenInclude(c => c.Property)
                .Where(ps => ps.Status != PaymentScheduleStatus.PaidLate &&
                             ps.Status != PaymentScheduleStatus.PaidOnTime &&
                             ps.Status != PaymentScheduleStatus.PaidEarly)
                .OrderBy(ps => ps.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public Task UpdatePaymentSchedule(PaymentSchedule paymentSchedule)
        {
            Context.PaymentSchedules.Update(paymentSchedule);
            return Context.SaveChangesAsync();
        }


        public async Task AddPayment(Payment payment, PaymentSchedule paymentSchedule)
        {
            using var transaction = await Context.Database.BeginTransactionAsync();

            try
            {
                Context.Payments.Add(payment);
                Context.PaymentSchedules.Update(paymentSchedule);

                await Context.SaveChangesAsync();

                bool hasRemainingUnpaid = await Context.PaymentSchedules
                    .AnyAsync(ps =>
                        ps.ContractId == paymentSchedule.ContractId &&
                        ps.Id != paymentSchedule.Id &&
                        ps.Status != PaymentScheduleStatus.PaidEarly &&
                        ps.Status != PaymentScheduleStatus.PaidOnTime &&
                        ps.Status != PaymentScheduleStatus.PaidLate);

                if (!hasRemainingUnpaid)
                {
                    await Context.Contracts
                        .Where(c => c.Id == paymentSchedule.ContractId)
                        .ExecuteUpdateAsync(s => s.SetProperty(c => c.Status, ContractStatus.Expired));
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public Task<bool> PaymentExistsByIntentId(string paymentIntentId)
        {
            return Context.Payments.AnyAsync(p => p.PaymentIntentId == paymentIntentId);
        }

        public Task<List<Payment>> GetOnHoldPayments(int skip, int take)
        {
            return Context.Payments
                .Where(p => p.Status == PaymentStatus.OnHold)
                .OrderBy(p => p.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public Task UpdatePayment(Payment payment)
        {
            Context.Payments.Update(payment);
            return Context.SaveChangesAsync();
        }
        #endregion
    }
}
