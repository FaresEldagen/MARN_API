using System;
using System.Collections.Generic;
using MARN_API.Enums;

namespace MARN_API.Models
{
    public class BookingRequest
    {
        public long Id { get; set; }
        public long PropertyId { get; set; }
        public Guid RenterId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public BookingRequestStatus Status { get; set; } = BookingRequestStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual Property Property { get; set; } = null!;
        public virtual ApplicationUser Renter { get; set; } = null!;
        public virtual ChatRoom ChatRoom { get; set; } = null!;
    }
}



