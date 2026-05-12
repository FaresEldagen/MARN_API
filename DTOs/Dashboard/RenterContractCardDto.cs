using MARN_API.Enums.Contract;

namespace MARN_API.DTOs.Dashboard
{
    public class RenterContractCardDto
    {
        public long ContractId { get; set; }
        public ContractStatus ContractStatus { get; set; }
        public DateTime ExpiryDate { get; set; }

        public Guid OwnerId { get; set; }
        public string OwnerName { get; set; } = string.Empty;

        public long PropertyId { get; set; }
        public string PropertyTitle { get; set; } = string.Empty;
    }
}
