using MARN_API.DTOs.Property;
using MARN_API.Enums;

namespace MARN_API.DTOs.Dashboard
{
    public class OwnerDashboardDto
    {
        public int PropertiesCount { get; set; }
        public ICollection<OwnerPropertyCardDto>? Properties { get; set; }

        public int OccupiedPlaces { get; set; }
        public int VacantPlaces { get; set; }
        public long TotalViews { get; set; }

        public decimal MonthlyRevenue { get; set; }
        public ICollection<MonthlyEarningDto>? MonthlyEarning { get; set; }
        public ICollection<YearlyEarningDto>? YearlyEarning { get; set; }
        public decimal WithdrawableEarnings { get; set; }
        public decimal OnHoldEarnings { get; set; }

        public ICollection<OwnerContractCardDto>? AllContracts { get; set; }

        public int UnreadNotificationsCount { get; set; }
        public ICollection<NotificationMiniCardDto>? Notifications { get; set; }

        public int PendingBookingRequestsCount { get; set; }
        public ICollection<OwnerPendingBookingRequestDto>? PendingBookingRequests { get; set; }

        public AccountStatus AccountStatus { get; set; }
    }
}