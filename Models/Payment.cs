using System;
using MARN_API.Enums.Payment;

namespace MARN_API.Models
{
    public class Payment
    {
        public long Id { get; set; }
        public long PaymentScheduleId { get; set; }

        public decimal AmountTotal { get; set; }
        public decimal PlatformFee { get; set; }
        public decimal OwnerAmount { get; set; }
        public string Currency { get; set; } = "egp";

        public string PaymentIntentId { get; set; } = string.Empty;

        public DateTime PaidAt { get; set; }
        public DateTime AvailableAt { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.OnHold;

        public virtual PaymentSchedule PaymentSchedule { get; set; } = null!;
    }
}
