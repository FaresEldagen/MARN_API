using MARN_API.Enums.Contract;

namespace MARN_API.DTOs.Admin
{
    public class AdminDashboardOverviewDto
    {
        public AdminMetricCardDto TotalUsers { get; set; } = new();
        public AdminMetricCardDto TotalProperties { get; set; } = new();
        public AdminMetricCardDto PendingVerifications { get; set; } = new();
        public AdminMetricCardDto TotalContracts { get; set; } = new();

        public AdminRevenueSummaryDto RevenueSummary { get; set; } = new();
        public AdminContractStatusSummaryDto ContractSummary { get; set; } = new();
        public List<AdminMonthlyRevenuePointDto> MonthlyRevenue { get; set; } = new();
    }

    public class AdminMetricCardDto
    {
        public long Value { get; set; }
        public decimal? TrendPercentage { get; set; }
    }

    public class AdminRevenueSummaryDto
    {
        public decimal TotalRevenue { get; set; }
        public decimal TotalSales { get; set; }
        public long NewUsersThisMonth { get; set; }
        public long ActiveContracts { get; set; }
        public decimal? RevenueTrendPercentage { get; set; }
    }

    public class AdminContractStatusSummaryDto
    {
        public long All { get; set; }
        public long Active { get; set; }
        public long Pending { get; set; }
        public long Expired { get; set; }
        public long Cancelled { get; set; }
    }

    public class AdminMonthlyRevenuePointDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string Label { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public decimal Sales { get; set; }
    }

    public class AdminContractStatusCountDto
    {
        public ContractStatus Status { get; set; }
        public string StatusDisplayName { get; set; } = string.Empty;
        public long Count { get; set; }
    }
}
