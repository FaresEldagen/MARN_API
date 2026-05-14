using MARN_API.Data;
using MARN_API.DTOs.Admin;
using MARN_API.Enums.Account;
using MARN_API.Enums.Property;
using MARN_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MARN_API.Repositories.Implementations
{
    public class AdminDashboardRepo : IAdminDashboardRepo
    {
        private readonly AppDbContext _context;

        public AdminDashboardRepo(AppDbContext context)
        {
            _context = context;
        }

        public Task<long> GetTotalNonAdminUsersAsync()
        {
            return NonAdminUsers().LongCountAsync();
        }

        public Task<long> GetNonAdminUsersCreatedBetweenAsync(DateTime fromUtc, DateTime toUtc)
        {
            return NonAdminUsers()
                .Where(u => u.CreatedAt >= fromUtc && u.CreatedAt < toUtc)
                .LongCountAsync();
        }

        public Task<long> GetTotalPropertiesAsync()
        {
            return _context.Properties.LongCountAsync();
        }

        public Task<long> GetPropertiesCreatedBetweenAsync(DateTime fromUtc, DateTime toUtc)
        {
            return _context.Properties
                .Where(p => p.CreatedAt >= fromUtc && p.CreatedAt < toUtc)
                .LongCountAsync();
        }

        public Task<long> GetPendingUserVerificationsAsync()
        {
            return NonAdminUsers()
                .Where(u => u.AccountStatus == AccountStatus.Pending)
                .LongCountAsync();
        }

        public Task<long> GetPendingPropertyVerificationsAsync()
        {
            return _context.Properties
                .Where(p => p.Status == PropertyStatus.Pending)
                .LongCountAsync();
        }

        public Task<long> GetPendingUserVerificationsCreatedBetweenAsync(DateTime fromUtc, DateTime toUtc)
        {
            return NonAdminUsers()
                .Where(u => u.AccountStatus == AccountStatus.Pending &&
                            u.CreatedAt >= fromUtc &&
                            u.CreatedAt < toUtc)
                .LongCountAsync();
        }

        public Task<long> GetPendingPropertyVerificationsCreatedBetweenAsync(DateTime fromUtc, DateTime toUtc)
        {
            return _context.Properties
                .Where(p => p.Status == PropertyStatus.Pending &&
                            p.CreatedAt >= fromUtc &&
                            p.CreatedAt < toUtc)
                .LongCountAsync();
        }

        public Task<long> GetTotalContractsAsync()
        {
            return _context.Contracts.LongCountAsync();
        }

        public Task<long> GetContractsCreatedBetweenAsync(DateTime fromUtc, DateTime toUtc)
        {
            return _context.Contracts
                .Where(c => c.CreatedAt >= fromUtc && c.CreatedAt < toUtc)
                .LongCountAsync();
        }

        public Task<List<AdminContractStatusCountDto>> GetContractStatusCountsAsync()
        {
            return _context.Contracts
                .GroupBy(c => c.Status)
                .Select(g => new AdminContractStatusCountDto
                {
                    Status = g.Key,
                    Count = g.LongCount()
                })
                .ToListAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _context.Payments
                .SumAsync(p => (decimal?)p.PlatformFee) ?? 0m;
        }

        public async Task<decimal> GetTotalSalesAsync()
        {
            return await _context.Payments
                .SumAsync(p => (decimal?)p.AmountTotal) ?? 0m;
        }

        public async Task<decimal> GetRevenueBetweenAsync(DateTime fromUtc, DateTime toUtc)
        {
            return await _context.Payments
                .Where(p => p.PaidAt >= fromUtc && p.PaidAt < toUtc)
                .SumAsync(p => (decimal?)p.PlatformFee) ?? 0m;
        }

        public Task<List<AdminMonthlyRevenuePointDto>> GetMonthlyRevenueAsync(DateTime fromUtc, DateTime toUtc)
        {
            return _context.Payments
                .Where(p => p.PaidAt >= fromUtc && p.PaidAt < toUtc)
                .GroupBy(p => new { p.PaidAt.Year, p.PaidAt.Month })
                .Select(g => new AdminMonthlyRevenuePointDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Revenue = g.Sum(p => p.PlatformFee),
                    Sales = g.Sum(p => p.AmountTotal)
                })
                .OrderBy(p => p.Year)
                .ThenBy(p => p.Month)
                .ToListAsync();
        }

        private IQueryable<Models.ApplicationUser> NonAdminUsers()
        {
            var adminRoleIds = _context.Roles
                .Where(r => r.NormalizedName == "ADMIN")
                .Select(r => r.Id);

            return _context.Users
                .Where(u => !_context.UserRoles
                    .Any(ur => ur.UserId == u.Id && adminRoleIds.Contains(ur.RoleId)));
        }
    }
}
