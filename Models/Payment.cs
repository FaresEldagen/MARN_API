using System;
using MARN_API.Enums;

namespace MARN_API.Models
{
    public class Payment
    {
        public long Id { get; set; }
        public long ContractId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PlatformFee { get; set; }
        public decimal OwnerAmount { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? AvailableAt { get; set; }
        public string? StripePaymentIntentId { get; set; }
        public string Currency { get; set; } = "EGP";
        public PaymentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public virtual Contract Contract { get; set; } = null!;
    }
}



