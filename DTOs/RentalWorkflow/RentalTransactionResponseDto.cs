using MARN_API.Enums;

namespace MARN_API.DTOs.RentalWorkflow
{
    public class RentalTransactionResponseDto
    {
        public Guid Id { get; set; }
        public Guid RenterId { get; set; }
        public Guid OwnerId { get; set; }
        public long PropertyId { get; set; }
        public string? StripeSessionId { get; set; }
        public long? PaymentId { get; set; }
        public long? ContractId { get; set; }
        public RentalTransactionStatus Status { get; set; }
        public RentalPaymentStatus PaymentStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
