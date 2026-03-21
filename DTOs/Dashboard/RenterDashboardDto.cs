using MARN_API.DTOs.Property;
using MARN_API.Enums;
using MARN_API.Models;

namespace MARN_API.DTOs.Dashboard
{
    public class RenterDashboardDto
    {
        public int ActiveRentalsCount { get; set; }
        public RenterNextPaymentDto? NextPayment { get; set; }
        public int SavedPropertiesCount { get; set; }
        public int UnreadNotificationsCount { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public ICollection<ActiveRentalCardDto>? ActiveRentals { get; set; }
        public ICollection<RenterPendingBookingRequestDto>? PendingBookingRequests { get; set; }
        public ICollection<PropertyCardDto>? SavedProperties { get; set; }
        public ICollection<NotificationCardDto>? Notifications { get; set; }
        public ICollection<PropertyCardDto>? Recommendations { get; set; }
    }
}
