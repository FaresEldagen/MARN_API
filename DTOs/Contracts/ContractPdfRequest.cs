using MARN_API.Enums;

namespace MARN_API.DTOs.Contracts
{
    public class ContractPdfRequest
    {
        public string? ContractNumber { get; set; }
        public DateTime? IssuedAtUtc { get; set; }
        public PartyInfo? Landlord { get; set; }
        public PartyInfo? Tenant { get; set; }
        public PropertyInfo? Property { get; set; }
        public RentalTermsInfo? RentalTerms { get; set; }
        public ElectronicSignatureInfo? ElectronicSignature { get; set; }
        public List<string>? AdditionalTerms { get; set; }
        public string? GoverningLawNote { get; set; }
    }

    public class PartyInfo
    {
        public string? FullName { get; set; }
        public string? NationalId { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }

    public class PropertyInfo
    {
        public string? ListingTitle { get; set; }
        public string? AddressLine { get; set; }
        public string? UnitNumber { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Description { get; set; }
    }

    public class RentalTermsInfo
    {
        public decimal RentAmount { get; set; }
        public decimal TotalContractAmount { get; set; }
        public PaymentFrequency PaymentFrequency { get; set; } = PaymentFrequency.OneTime;
        public string? Currency { get; set; }
        public DateOnly? LeaseStartDate { get; set; }
        public DateOnly? LeaseEndDate { get; set; }
    }

    public class ElectronicSignatureInfo
    {
        public string? SignerName { get; set; }
        public string? SignerNationalId { get; set; }
        public DateTime? SignedAtUtc { get; set; }
        public string? ConsentStatement { get; set; }
    }

    public class GeneratedContractPdfResult
    {
        public required string FileName { get; init; }
        public required byte[] Content { get; init; }
        public required string ContractNumber { get; init; }
        public required DateTime GeneratedAtUtc { get; init; }
    }
}
