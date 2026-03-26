using MARN_API.Enums;

namespace MARN_API.DTOs.Dashboard
{
    public class OwnerContractCardDto
    {
        public long ContractId { get; set; }
        public ContractStatus ContractStatus { get; set; }
        public DateTime ExpiryDate { get; set; }

        public Guid RenterId { get; set; }
        public string RenterName { get; set; } = string.Empty;

        public long PropertyId { get; set; }
        public string PropertyTitle { get; set; } = string.Empty;
    }
}
