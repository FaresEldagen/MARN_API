using System;
using MARN_API.Enums;

namespace MARN_API.Models
{
    public class Payment
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public long ContractId { get; set; }

        public string StripePaymentIntentId { get; set; } = string.Empty;
        public string StripeCustomerId { get; set; } = string.Empty;
        public string? StripeSubscriptionId { get; set; }

        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EGP";
        public PaymentType Type { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime? PaidAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ApplicationUser User { get; set; } = null!;
        public virtual Contract Contract { get; set; } = null!;
    }
}



