namespace MARN_API.DTOs.Dashboard
{
    public class OwnerPropertyContractDto
    {
        public long ContractId { get; set; }
        public Guid RenterId { get; set; }
        public string RenterName { get; set; } = string.Empty;
        public string? RenterProfileImage { get; set; }
    }
}
