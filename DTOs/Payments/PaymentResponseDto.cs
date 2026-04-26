using MARN_API.Enums;

namespace MARN_API.DTOs.Payments
{
    public class PaymentResponseDto
    {
        public long Id { get; set; }
        public long? ContractId { get; set; }
        public string StripeSessionId { get; set; } = string.Empty;
        public decimal AmountTotal { get; set; }
        public decimal PlatformFee { get; set; }
        public decimal OwnerAmount { get; set; }
        public Guid RenterId { get; set; }
        public string? RenterEmail { get; set; }
        public Guid OwnerId { get; set; }
        public long PropertyId { get; set; }
        public string OwnerStripeAccountId { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? AvailableAt { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ReceiptUrl { get; set; }
        public string Currency { get; set; } = "EGP";
        public PaymentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? PropertyTitle { get; set; }
        public string? PropertyAddress { get; set; }
        public string? RenterFirstName { get; set; }
        public string? RenterLastName { get; set; }
    }
}
