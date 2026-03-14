namespace MARN_API.DTOs.Dashboard
{
    public class RenterPendingBookingRequestDto
    {
        public long BookingRequestId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public long PropertyId { get; set; }
        public string PropertyTitle { get; set; } = string.Empty;

        public Guid OwnerId { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public string? OwnerProfileImage { get; set; }
    }
}
