using MARN_API.DTOs.Admin;
using MARN_API.Enums;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;

namespace MARN_API.Services.Implementations
{
    public class AdminDetailedStatsService : IAdminDetailedStatsService
    {
        private const int MaxPageSize = 100;
        private readonly IAdminDetailedStatsRepo _detailedStatsRepo;
        private readonly ILogger<AdminDetailedStatsService> _logger;

        public AdminDetailedStatsService(
            IAdminDetailedStatsRepo detailedStatsRepo,
            ILogger<AdminDetailedStatsService> logger)
        {
            _detailedStatsRepo = detailedStatsRepo;
            _logger = logger;
        }

        public async Task<ServiceResult<AdminDetailedUsersResponseDto>> GetUsersAsync(AdminDetailedUsersQueryDto query)
        {
            var period = ResolvePeriod(query);
            if (!period.Success)
                return ServiceResult<AdminDetailedUsersResponseDto>.Fail(period.Message!, resultType: period.ResultType);

            var result = await _detailedStatsRepo.GetUsersAsync(query, period.Data!.FromUtc, period.Data.ToUtc, period.Data.GroupByDay);
            result.AppliedPeriod = period.Data.ToDto();
            return ServiceResult<AdminDetailedUsersResponseDto>.Ok(result);
        }

        public async Task<ServiceResult<AdminDetailedPropertiesResponseDto>> GetPropertiesAsync(AdminDetailedPropertiesQueryDto query)
        {
            var period = ResolvePeriod(query);
            if (!period.Success)
                return ServiceResult<AdminDetailedPropertiesResponseDto>.Fail(period.Message!, resultType: period.ResultType);

            var result = await _detailedStatsRepo.GetPropertiesAsync(query, period.Data!.FromUtc, period.Data.ToUtc);
            result.AppliedPeriod = period.Data.ToDto();
            return ServiceResult<AdminDetailedPropertiesResponseDto>.Ok(result);
        }

        public async Task<ServiceResult<AdminDetailedContractsResponseDto>> GetContractsAsync(AdminDetailedContractsQueryDto query)
        {
            var period = ResolvePeriod(query);
            if (!period.Success)
                return ServiceResult<AdminDetailedContractsResponseDto>.Fail(period.Message!, resultType: period.ResultType);

            var result = await _detailedStatsRepo.GetContractsAsync(query, period.Data!.FromUtc, period.Data.ToUtc);
            result.AppliedPeriod = period.Data.ToDto();
            return ServiceResult<AdminDetailedContractsResponseDto>.Ok(result);
        }

        public async Task<ServiceResult<AdminDetailedRevenueResponseDto>> GetRevenueAsync(AdminDetailedRevenueQueryDto query)
        {
            var period = ResolvePeriod(query);
            if (!period.Success)
                return ServiceResult<AdminDetailedRevenueResponseDto>.Fail(period.Message!, resultType: period.ResultType);

            var result = await _detailedStatsRepo.GetRevenueAsync(query, period.Data!.FromUtc, period.Data.ToUtc, period.Data.GroupByDay);
            result.AppliedPeriod = period.Data.ToDto();
            return ServiceResult<AdminDetailedRevenueResponseDto>.Ok(result);
        }

        private ServiceResult<ResolvedPeriod> ResolvePeriod(AdminDetailedStatsPeriodQueryDto query)
        {
            NormalizePaging(query);

            var nowUtc = DateTime.UtcNow;
            var period = (query.Period ?? "allTime").Trim();

            if (period.Equals("allTime", StringComparison.OrdinalIgnoreCase))
            {
                return ServiceResult<ResolvedPeriod>.Ok(new ResolvedPeriod(period, null, null, false));
            }

            if (period.Equals("thisMonth", StringComparison.OrdinalIgnoreCase))
            {
                var fromUtc = new DateTime(nowUtc.Year, nowUtc.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                return ServiceResult<ResolvedPeriod>.Ok(new ResolvedPeriod(period, fromUtc, nowUtc, true));
            }

            if (period.Equals("thisYear", StringComparison.OrdinalIgnoreCase))
            {
                var fromUtc = new DateTime(nowUtc.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return ServiceResult<ResolvedPeriod>.Ok(new ResolvedPeriod(period, fromUtc, nowUtc, false));
            }

            if (period.Equals("custom", StringComparison.OrdinalIgnoreCase))
            {
                if (!query.FromUtc.HasValue || !query.ToUtc.HasValue)
                    return ServiceResult<ResolvedPeriod>.Fail("Custom period requires fromUtc and toUtc.", resultType: ServiceResultType.BadRequest);

                if (query.FromUtc.Value >= query.ToUtc.Value)
                    return ServiceResult<ResolvedPeriod>.Fail("fromUtc must be earlier than toUtc.", resultType: ServiceResultType.BadRequest);

                var duration = query.ToUtc.Value - query.FromUtc.Value;
                var useDayGrouping = duration.TotalDays <= 31;
                return ServiceResult<ResolvedPeriod>.Ok(new ResolvedPeriod(period, query.FromUtc.Value, query.ToUtc.Value, useDayGrouping));
            }

            return ServiceResult<ResolvedPeriod>.Fail(
                "Invalid period. Supported values are allTime, thisMonth, thisYear, and custom.",
                resultType: ServiceResultType.BadRequest);
        }

        private static void NormalizePaging(AdminDetailedStatsPeriodQueryDto query)
        {
            if (query.PageNumber < 1)
                query.PageNumber = 1;

            if (query.PageSize < 1)
                query.PageSize = 20;

            if (query.PageSize > MaxPageSize)
                query.PageSize = MaxPageSize;
        }

        private sealed class ResolvedPeriod
        {
            public ResolvedPeriod(string period, DateTime? fromUtc, DateTime? toUtc, bool groupByDay)
            {
                Period = period;
                FromUtc = fromUtc;
                ToUtc = toUtc;
                GroupByDay = groupByDay;
            }
            public string Period { get; }
            public DateTime? FromUtc { get; }
            public DateTime? ToUtc { get; }
            public bool GroupByDay { get; }
            public string Grouping => GroupByDay ? "day" : "month";

            public AdminAppliedPeriodDto ToDto()
            {
                return new AdminAppliedPeriodDto
                {
                    Period = Period,
                    FromUtc = FromUtc,
                    ToUtc = ToUtc,
                    Grouping = Grouping
                };
            }
        }
    }
}
