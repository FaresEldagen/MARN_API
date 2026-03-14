using MARN_API.Enums;
using MARN_API.Models;

namespace MARN_API.DTOs.Dashboard
{
    public class ActiveRentalCardDto
    {
        public long ContractId { get; set; }
        public ContractStatus ContractStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string PropertyTitle { get; set; } = string.Empty;
        public string PropertyAddress { get; set; } = string.Empty;
        public string PropertyImageUrl { get; set; } = string.Empty;

        public long PaymentId { get; set; }
        public decimal NextPaymentAmount { get; set; }
        public PaymentFrequency PaymentFrequency { get; set; }
        public bool IsPaymentMade { get; set; }
    }
}
