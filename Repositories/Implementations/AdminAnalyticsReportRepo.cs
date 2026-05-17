using MARN_API.Data;
using MARN_API.DTOs.Admin;
using MARN_API.DTOs.Common;
using MARN_API.Repositories.Interfaces;
using MARN_API.Models;
using Microsoft.EntityFrameworkCore;

namespace MARN_API.Repositories.Implementations
{
    public class AdminAnalyticsReportRepo : IAdminAnalyticsReportRepo
    {
        private readonly AppDbContext _context;

        public AdminAnalyticsReportRepo(AppDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(AdminAnalyticsReport report)
        {
            return _context.AdminAnalyticsReports.AddAsync(report).AsTask();
        }

        public Task<AdminAnalyticsReport?> GetByIdAsync(long reportId)
        {
            return _context.AdminAnalyticsReports
                .IgnoreQueryFilters()
                .Include(x => x.GeneratedByAdmin)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == reportId);
        }

        public Task<ApplicationUser?> GetAdminUserAsync(Guid adminId)
        {
            return _context.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x => x.Id == adminId);
        }

        public async Task<PagedResult<AdminAnalyticsReportListItemDto>> GetReportsAsync(AdminAnalyticsReportQueryDto query)
        {
            var reportsQuery = _context.AdminAnalyticsReports
                .IgnoreQueryFilters()
                .Include(x => x.GeneratedByAdmin)
                .AsNoTracking()
                .AsQueryable();

            if (query.Scope.HasValue)
            {
                reportsQuery = reportsQuery.Where(x => x.Scope == query.Scope.Value);
            }

            if (query.Format.HasValue)
            {
                reportsQuery = reportsQuery.Where(x => x.Format == query.Format.Value);
            }

            if (query.Year.HasValue)
            {
                reportsQuery = reportsQuery.Where(x => (x.FromUtc ?? x.GeneratedAt).Year == query.Year.Value);
            }

            if (query.Month.HasValue)
            {
                reportsQuery = reportsQuery.Where(x => (x.FromUtc ?? x.GeneratedAt).Month == query.Month.Value);
            }

            var totalCount = await reportsQuery.CountAsync();
            var items = await reportsQuery
                .OrderByDescending(x => x.GeneratedAt)
                .ThenByDescending(x => x.Id)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(x => new AdminAnalyticsReportListItemDto
                {
                    ReportId = x.Id,
                    Scope = x.Scope,
                    Format = x.Format,
                    RequestedPeriod = x.RequestedPeriod,
                    FromUtc = x.FromUtc,
                    ToUtc = x.ToUtc,
                    Grouping = x.Grouping,
                    FileName = x.FileName,
                    FileSizeBytes = x.FileSizeBytes,
                    GeneratedAt = x.GeneratedAt,
                    GeneratedByAdminId = x.GeneratedByAdminId,
                    GeneratedByAdminName = (x.GeneratedByAdmin.FirstName + " " + x.GeneratedByAdmin.LastName).Trim()
                })
                .ToListAsync();

            return new PagedResult<AdminAnalyticsReportListItemDto>
            {
                Items = items,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)query.PageSize)
            };
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
