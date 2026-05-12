using MARN_API.Enums.Payment;

namespace MARN_API.DTOs.Dashboard
{
    public class OwnerPendingBookingRequestDto
    {
        public long BookingRequestId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public PaymentFrequency PaymentFrequency { get; set; }

        public long PropertyId { get; set; }
        public string PropertyTitle { get; set; } = string.Empty;

        public Guid RenterId { get; set; }
        public string RenterName { get; set; } = string.Empty;
        public string? RenterProfileImage { get; set; }
    }
}
