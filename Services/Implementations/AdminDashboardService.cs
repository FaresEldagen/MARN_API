using MARN_API.DTOs.Admin;
using MARN_API.Enums.Contract;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using System.Globalization;

namespace MARN_API.Services.Implementations
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IAdminDashboardRepo _dashboardRepo;
        private readonly ILogger<AdminDashboardService> _logger;

        public AdminDashboardService(
            IAdminDashboardRepo dashboardRepo,
            ILogger<AdminDashboardService> logger)
        {
            _dashboardRepo = dashboardRepo;
            _logger = logger;
        }

        public async Task<ServiceResult<AdminDashboardOverviewDto>> GetOverviewAsync()
        {
            _logger.LogInformation("Admin dashboard overview requested.");

            var nowUtc = DateTime.UtcNow;
            var currentMonthStart = new DateTime(nowUtc.Year, nowUtc.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var nextMonthStart = currentMonthStart.AddMonths(1);
            var previousMonthStart = currentMonthStart.AddMonths(-1);
            var previousComparisonEnd = previousMonthStart.Add(nowUtc - currentMonthStart);
            if (previousComparisonEnd > currentMonthStart)
                previousComparisonEnd = currentMonthStart;

            var graphStart = currentMonthStart.AddMonths(-5);

            var totalUsers = await _dashboardRepo.GetTotalNonAdminUsersAsync();
            var totalProperties = await _dashboardRepo.GetTotalPropertiesAsync();
            var pendingUsers = await _dashboardRepo.GetPendingUserVerificationsAsync();
            var pendingProperties = await _dashboardRepo.GetPendingPropertyVerificationsAsync();
            var totalContracts = await _dashboardRepo.GetTotalContractsAsync();
            var totalRevenue = await _dashboardRepo.GetTotalRevenueAsync();
            var totalSales = await _dashboardRepo.GetTotalSalesAsync();
            var statusCounts = await _dashboardRepo.GetContractStatusCountsAsync();

            var newUsersThisMonth = await _dashboardRepo.GetNonAdminUsersCreatedBetweenAsync(currentMonthStart, nextMonthStart);
            var currentMonthUsers = await _dashboardRepo.GetNonAdminUsersCreatedBetweenAsync(currentMonthStart, nowUtc);
            var previousMonthUsers = await _dashboardRepo.GetNonAdminUsersCreatedBetweenAsync(previousMonthStart, previousComparisonEnd);

            var currentMonthProperties = await _dashboardRepo.GetPropertiesCreatedBetweenAsync(currentMonthStart, nowUtc);
            var previousMonthProperties = await _dashboardRepo.GetPropertiesCreatedBetweenAsync(previousMonthStart, previousComparisonEnd);

            var currentMonthPendingVerifications =
                await _dashboardRepo.GetPendingUserVerificationsCreatedBetweenAsync(currentMonthStart, nowUtc) +
                await _dashboardRepo.GetPendingPropertyVerificationsCreatedBetweenAsync(currentMonthStart, nowUtc);

            var previousMonthPendingVerifications =
                await _dashboardRepo.GetPendingUserVerificationsCreatedBetweenAsync(previousMonthStart, previousComparisonEnd) +
                await _dashboardRepo.GetPendingPropertyVerificationsCreatedBetweenAsync(previousMonthStart, previousComparisonEnd);

            var currentMonthContracts = await _dashboardRepo.GetContractsCreatedBetweenAsync(currentMonthStart, nowUtc);
            var previousMonthContracts = await _dashboardRepo.GetContractsCreatedBetweenAsync(previousMonthStart, previousComparisonEnd);

            var currentMonthRevenue = await _dashboardRepo.GetRevenueBetweenAsync(currentMonthStart, nowUtc);
            var previousMonthRevenue = await _dashboardRepo.GetRevenueBetweenAsync(previousMonthStart, previousComparisonEnd);

            var monthlyRevenue = await _dashboardRepo.GetMonthlyRevenueAsync(graphStart, nextMonthStart);
            var filledMonthlyRevenue = FillMonthlyRevenue(graphStart, monthlyRevenue);

            var contractSummary = BuildContractSummary(statusCounts, totalContracts);
            var pendingVerifications = pendingUsers + pendingProperties;

            var overview = new AdminDashboardOverviewDto
            {
                TotalUsers = new AdminMetricCardDto
                {
                    Value = totalUsers,
                    TrendPercentage = CalculateTrendPercentage(currentMonthUsers, previousMonthUsers)
                },
                TotalProperties = new AdminMetricCardDto
                {
                    Value = totalProperties,
                    TrendPercentage = CalculateTrendPercentage(currentMonthProperties, previousMonthProperties)
                },
                PendingVerifications = new AdminMetricCardDto
                {
                    Value = pendingVerifications,
                    TrendPercentage = CalculateTrendPercentage(currentMonthPendingVerifications, previousMonthPendingVerifications)
                },
                TotalContracts = new AdminMetricCardDto
                {
                    Value = totalContracts,
                    TrendPercentage = CalculateTrendPercentage(currentMonthContracts, previousMonthContracts)
                },
                RevenueSummary = new AdminRevenueSummaryDto
                {
                    TotalRevenue = totalRevenue,
                    TotalSales = totalSales,
                    NewUsersThisMonth = newUsersThisMonth,
                    ActiveContracts = contractSummary.Active,
                    RevenueTrendPercentage = CalculateTrendPercentage(currentMonthRevenue, previousMonthRevenue)
                },
                ContractSummary = contractSummary,
                MonthlyRevenue = filledMonthlyRevenue
            };

            return ServiceResult<AdminDashboardOverviewDto>.Ok(overview);
        }

        private static AdminContractStatusSummaryDto BuildContractSummary(
            List<AdminContractStatusCountDto> statusCounts,
            long totalContracts)
        {
            long CountFor(ContractStatus status) =>
                statusCounts.FirstOrDefault(c => c.Status == status)?.Count ?? 0;

            return new AdminContractStatusSummaryDto
            {
                All = totalContracts,
                Active = CountFor(ContractStatus.Active),
                Pending = CountFor(ContractStatus.Pending),
                Expired = CountFor(ContractStatus.Expired),
                Cancelled = CountFor(ContractStatus.Cancelled)
            };
        }

        private static List<AdminMonthlyRevenuePointDto> FillMonthlyRevenue(
            DateTime graphStart,
            List<AdminMonthlyRevenuePointDto> source)
        {
            var sourceByMonth = source.ToDictionary(
                p => new DateTime(p.Year, p.Month, 1),
                p => p);

            var points = new List<AdminMonthlyRevenuePointDto>();
            for (var month = graphStart; points.Count < 6; month = month.AddMonths(1))
            {
                var key = new DateTime(month.Year, month.Month, 1);
                sourceByMonth.TryGetValue(key, out var point);

                points.Add(new AdminMonthlyRevenuePointDto
                {
                    Year = month.Year,
                    Month = month.Month,
                    Label = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month.Month),
                    Revenue = point?.Revenue ?? 0m,
                    Sales = point?.Sales ?? 0m
                });
            }

            return points;
        }

        private static decimal? CalculateTrendPercentage(decimal current, decimal previous)
        {
            if (previous == 0m)
                return current == 0m ? 0m : 100m;

            return Math.Round(((current - previous) / previous) * 100m, 2);
        }
    }
}
