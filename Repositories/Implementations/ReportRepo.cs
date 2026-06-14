using MARN_API.Data;
using MARN_API.DTOs.Common;
using MARN_API.DTOs.Moderation;
using MARN_API.Enums.Report;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MARN_API.Repositories.Implementations
{
    public class ReportRepo : IReportRepo
    {
        private readonly AppDbContext Context;

        public ReportRepo(AppDbContext context)
        {
            Context = context;
        }

        public async Task<AdminModerationQueueDto> GetAdminQueueAsync(AdminReportQueryDto query)
        {
            var reportsQuery = BuildReportsQuery(query.Status, query.ReportableType, query.Search);
            var totalCount = await reportsQuery.CountAsync();

            var items = await reportsQuery
                .OrderByDescending(r => r.CreatedAt)
                .ThenByDescending(r => r.Id)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(r => new AdminModerationReportListItemDto
                {
                    ReportId = r.Id,
                    ReportableType = r.ReportableType,
                    Status = r.Status,
                    Reason = r.Reason,
                    CreatedAt = r.CreatedAt,
                    ReviewedAt = r.ReviewedAt,
                    ActionTaken = r.ActionTaken,
                    ReporterId = r.ReporterId,
                    ReporterName = r.Reporter == null
                        ? "[Deleted user]"
                        : (r.Reporter.FirstName + " " + r.Reporter.LastName).Trim(),
                    ReviewerId = r.ReviewerId,
                    ReviewerName = r.Reviewer == null
                        ? null
                        : (r.Reviewer.FirstName + " " + r.Reviewer.LastName).Trim(),
                    TargetLabel = r.ReportableType == ReportableType.User
                        ? Context.Users
                            .IgnoreQueryFilters()
                            .Where(u => u.Id == r.ReportableGuidId)
                            .Select(u => (u.FirstName + " " + u.LastName).Trim())
                            .FirstOrDefault() ?? "[Deleted user]"
                        : r.ReportableType == ReportableType.Property
                        ? Context.Properties
                            .Where(p => p.Id == r.ReportableId)
                            .Select(p => p.Title)
                            .FirstOrDefault() ?? "[Deleted property]"
                        : r.ReportableType == ReportableType.Message
                        ? Context.Messages
                            .Where(m => m.Id == r.ReportableGuidId)
                            .Select(m => "Message between " + m.Sender.FirstName + " " + m.Sender.LastName + " and " + m.Receiver.FirstName + " " + m.Receiver.LastName)
                            .FirstOrDefault() ?? "[Deleted message]"
                        : Context.PropertyFeedbacks
                            .Where(f => f.Id == r.ReportableId)
                            .Select(f => "Comment on " + f.Property.Title)
                            .FirstOrDefault() ?? "[Deleted comment]"
                })
                .ToListAsync();

            var reportIds = items.Select(x => x.ReportId).ToList();
            if (reportIds.Count > 0)
            {
                var actionLogRows = await Context.AdminActionLogs
                    .AsNoTracking()
                    .Where(log => log.ReportId.HasValue && reportIds.Contains(log.ReportId.Value))
                    .Select(log => new
                    {
                        ReportId = log.ReportId!.Value,
                        log.ActionType
                    })
                    .ToListAsync();

                var actionsByReportId = actionLogRows
                    .GroupBy(x => x.ReportId)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => ParseActionType(x.ActionType))
                            .Where(x => x.HasValue)
                            .Select(x => x!.Value)
                            .Distinct()
                            .ToList());

                foreach (var item in items)
                {
                    if (actionsByReportId.TryGetValue(item.ReportId, out var actions))
                    {
                        item.ActionsTaken = actions;
                    }
                    else if (item.ActionTaken.HasValue)
                    {
                        item.ActionsTaken = [item.ActionTaken.Value];
                    }
                }
            }

            var statusBreakdownQuery = BuildReportsQuery(null, query.ReportableType, query.Search);
            var statusBreakdown = await statusBreakdownQuery
                .GroupBy(r => r.Status)
                .Select(g => new AdminReportStatusCountDto
                {
                    Status = g.Key,
                    Count = g.LongCount()
                })
                .OrderBy(x => x.Status)
                .ToListAsync();

            var typeBreakdownQuery = BuildReportsQuery(query.Status, null, query.Search);
            var typeBreakdown = await typeBreakdownQuery
                .GroupBy(r => r.ReportableType)
                .Select(g => new AdminReportTypeCountDto
                {
                    ReportableType = g.Key,
                    Count = g.LongCount()
                })
                .OrderBy(x => x.ReportableType)
                .ToListAsync();

            return new AdminModerationQueueDto
            {
                Reports = new PagedResult<AdminModerationReportListItemDto>
                {
                    Items = items,
                    PageNumber = query.PageNumber,
                    PageSize = query.PageSize,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize)
                },
                StatusBreakdown = statusBreakdown,
                TypeBreakdown = typeBreakdown
            };
        }

        public Task<Report?> GetReportDetailsAsync(long reportId)
        {
            return Context.Reports
                .IgnoreQueryFilters()
                .Include(r => r.Reporter)
                .Include(r => r.Reviewer)
                .Include(r => r.ActionLogs)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == reportId);
        }

        public Task<Report?> GetTrackedReportAsync(long reportId)
        {
            return Context.Reports
                .FirstOrDefaultAsync(r => r.Id == reportId);
        }

        public Task<ApplicationUser?> GetUserTargetAsync(Guid userId)
        {
            return Context.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public Task<Property?> GetPropertyTargetAsync(long propertyId)
        {
            return Context.Properties
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.Id == propertyId);
        }

        public Task<Message?> GetMessageTargetAsync(Guid messageId)
        {
            return Context.Messages
                .IgnoreQueryFilters()
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .FirstOrDefaultAsync(m => m.Id == messageId);
        }

        public Task<PropertyFeedback?> GetPropertyCommentTargetAsync(long commentId)
        {
            return Context.PropertyFeedbacks
                .Include(f => f.Property)
                .ThenInclude(p => p.Owner)
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.Id == commentId);
        }

        public async Task AddAsync(Report report)
        {
            await Context.Reports.AddAsync(report);
        }

        public async Task AddActionLogAsync(AdminActionLog actionLog)
        {
            await Context.AdminActionLogs.AddAsync(actionLog);
        }

        public Task SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
        }

        public async Task DeleteByReporterIdAsync(Guid userId)
        {
            await Context.Reports
                .Where(r => r.ReporterId == userId)
                .ExecuteDeleteAsync();
        }

        private IQueryable<Report> BuildReportsQuery(
            ReportStatus? status,
            ReportableType? reportableType,
            string? search)
        {
            var query = Context.Reports
                .IgnoreQueryFilters()
                .Include(r => r.Reporter)
                .Include(r => r.Reviewer)
                .AsNoTracking()
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(r => r.Status == status.Value);
            }

            if (reportableType.HasValue)
            {
                query = query.Where(r => r.ReportableType == reportableType.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var trimmedSearch = search.Trim().ToLower();
                var parsedReportableId = long.TryParse(search.Trim(), out var reportableIdValue)
                    ? reportableIdValue
                    : (long?)null;
                var parsedReportableGuidId = Guid.TryParse(search.Trim(), out var reportableGuidIdValue)
                    ? reportableGuidIdValue
                    : (Guid?)null;

                query = query.Where(r =>
                    r.Reason.ToLower().Contains(trimmedSearch) ||
                    (r.Reporter != null && ((r.Reporter.FirstName + " " + r.Reporter.LastName).ToLower().Contains(trimmedSearch) ||
                                            (r.Reporter.Email != null && r.Reporter.Email.ToLower().Contains(trimmedSearch)))) ||
                    (r.Reviewer != null && ((r.Reviewer.FirstName + " " + r.Reviewer.LastName).ToLower().Contains(trimmedSearch) ||
                                            (r.Reviewer.Email != null && r.Reviewer.Email.ToLower().Contains(trimmedSearch)))) ||
                    (parsedReportableId.HasValue && r.ReportableId == parsedReportableId.Value) ||
                    (parsedReportableGuidId.HasValue && r.ReportableGuidId == parsedReportableGuidId.Value) ||
                    (r.ReportableType == ReportableType.User &&
                     Context.Users.IgnoreQueryFilters().Any(u =>
                         u.Id == r.ReportableGuidId &&
                         ((u.FirstName + " " + u.LastName).ToLower().Contains(trimmedSearch) ||
                          (u.Email != null && u.Email.ToLower().Contains(trimmedSearch))))) ||
                    (r.ReportableType == ReportableType.Property &&
                     Context.Properties.Any(p =>
                         p.Id == r.ReportableId &&
                         (p.Title.ToLower().Contains(trimmedSearch) || p.Address.ToLower().Contains(trimmedSearch)))) ||
                    (r.ReportableType == ReportableType.Message &&
                     Context.Messages.Any(m =>
                         m.Id == r.ReportableGuidId &&
                         ((m.Sender.FirstName + " " + m.Sender.LastName).ToLower().Contains(trimmedSearch) ||
                          (m.Receiver.FirstName + " " + m.Receiver.LastName).ToLower().Contains(trimmedSearch)))) ||
                    (r.ReportableType == ReportableType.PropertyComment &&
                     Context.PropertyFeedbacks.Any(f =>
                         f.Id == r.ReportableId &&
                         ((f.Content != null && f.Content.ToLower().Contains(trimmedSearch)) ||
                          f.Property.Title.ToLower().Contains(trimmedSearch)))));
            }

            return query;
        }

        private static ReportModerationActionType? ParseActionType(string? actionType)
        {
            return Enum.TryParse<ReportModerationActionType>(actionType, ignoreCase: true, out var parsedAction)
                ? parsedAction
                : null;
        }
    }
}
