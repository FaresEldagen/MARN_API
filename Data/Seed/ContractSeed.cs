using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;
using MARN_API.Enums.Payment;
using MARN_API.Enums.Contract;
using MARN_API.Utilities;

namespace MARN_API.Data.Seed
{
    /// <summary>
    /// Seed contracts that cover every meaningful business scenario:
    ///
    ///  ID         | Property | Renter       | Frequency   | Status    | Scenario
    /// ------------|----------|--------------|-------------|-----------|------------------------------------------
    ///  1000001    | 1001     | Renter A     | Monthly     | Active    | Healthy active monthly rental (main test)
    ///  1000002    | 1002     | Renter B     | Quarterly   | Pending   | Active quarterly rental with overdue schedule
    ///  1000003    | 1100     | Renter A     | Onetime     | Active    | Active yearly rental – NotAvailableYet future
    ///  1000004    | 1100     | Renter B     | Monthly     | Active    | Owner Z property – for owner dashboard earnings
    ///  1000005    | 1002     | Renter A     | Monthly     | Expired   | Fully paid expired contract (history)
    ///  1000006    | 1004     | Renter B     | Monthly     | Cancelled | Cancelled – no schedules should be payable
    ///  1000101    | 1204     | Recent       | Monthly     | Pending   | Dashboard pending contract scenario
    ///  1000102    | 1003     | Renter E     | Monthly     | Active    | Dashboard revenue graph scenario
    /// </summary>
    public class ContractSeed : IEntityTypeConfiguration<Contract>
    {
        public void Configure(EntityTypeBuilder<Contract> builder)
        {
            var renterAId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var renterBId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var renterEId = Guid.Parse("88888888-8888-8888-8888-888888888888");
            var recentRenterId = Guid.Parse("10000000-0000-0000-0000-000000000004");
            var bannedRenterId = Guid.Parse("10000000-0000-0000-0000-000000000002");

            builder.HasData(

                // ── CONTRACT 1000001 ──────────────────────────────────────────────────────
                // Active monthly rental: Renter A in Property 1001 (Owner X)
                new Contract
                {
                    Id = 1000001,
                    PropertyId = 1001,
                    RenterId = renterAId,

                    LeaseStartDate = new DateOnly(2026, 1, 1),
                    LeaseEndDate = new DateOnly(2027, 1, 1),
                    PaymentFrequency = PaymentFrequency.Monthly,
                    TotalContractAmount = 60000m,

                    CreatedAt = new DateTime(2025, 12, 27, 0, 0, 0, DateTimeKind.Utc),
                    SignedByRenterAt = new DateTime(2025, 12, 28, 0, 0, 0, DateTimeKind.Utc),
                    Status = ContractStatus.Active,

                    FileName = "rental-contract-1000001.pdf",
                    Hash = "3039d56c00f0d4068ebe0b93a771e151c13954c3a18b5668817c573098f63198",
                    TransactionId = null,
                    MerkleRoot = null,
                    FilePath = ContractDocumentPathBuilder.BuildPdfRelativePath(1000001),
                    OtsFilePath = ContractDocumentPathBuilder.BuildOtsRelativePath(1000001)
                },

                // ── CONTRACT 1000002 ──────────────────────────────────────────────────────
                // Active quarterly rental: Renter B in Property 1002 (Owner X)
                new Contract
                {
                    Id = 1000002,
                    PropertyId = 1002,
                    RenterId = renterBId,

                    LeaseStartDate = new DateOnly(2026, 1, 1),
                    LeaseEndDate = new DateOnly(2027, 1, 1),
                    PaymentFrequency = PaymentFrequency.Quarterly,
                    TotalContractAmount = 90000m,

                    CreatedAt = new DateTime(2025, 12, 28, 0, 0, 0, DateTimeKind.Utc),
                    SignedByRenterAt = new DateTime(2025, 12, 29, 0, 0, 0, DateTimeKind.Utc),
                    Status = ContractStatus.Pending,

                    FileName = "rental-contract-1000002.pdf",
                    Hash = "ff411815aaad5ad467d9b4f65d194bff57438215019590ac11cef7ec788fca39",
                    TransactionId = null,
                    MerkleRoot = null,
                    FilePath = ContractDocumentPathBuilder.BuildPdfRelativePath(1000002),
                    OtsFilePath = ContractDocumentPathBuilder.BuildOtsRelativePath(1000002)
                },

                // ── CONTRACT 1000003 ──────────────────────────────────────────────────────
                // Active monthly rental: Renter A in Property 1100 (Owner X)
                new Contract
                {
                    Id = 1000003,
                    PropertyId = 1100,
                    RenterId = renterAId,

                    LeaseStartDate = new DateOnly(2025, 6, 1),
                    LeaseEndDate = new DateOnly(2027, 6, 1),
                    PaymentFrequency = PaymentFrequency.OneTime,
                    TotalContractAmount = 96000m,

                    CreatedAt = new DateTime(2025, 5, 24, 0, 0, 0, DateTimeKind.Utc),
                    SignedByRenterAt = new DateTime(2025, 5, 25, 0, 0, 0, DateTimeKind.Utc),
                    Status = ContractStatus.Active,

                    FileName = "rental-contract-1000003.pdf",
                    Hash = "d7c850ed73db284d3804dbf6fa4e97d7ebebf30e046484d9a0ea2de8459b414d",
                    TransactionId = null,
                    MerkleRoot = null,
                    FilePath = ContractDocumentPathBuilder.BuildPdfRelativePath(1000003),
                    OtsFilePath = ContractDocumentPathBuilder.BuildOtsRelativePath(1000003)
                },

                // ── CONTRACT 1000004 ──────────────────────────────────────────────────────
                // Active one-time rental: Renter B in Property 1100 (Owner X)
                new Contract
                {
                    Id = 1000004,
                    PropertyId = 1100,
                    RenterId = renterBId,

                    LeaseStartDate = new DateOnly(2026, 2, 1),
                    LeaseEndDate = new DateOnly(2027, 2, 1),
                    PaymentFrequency = PaymentFrequency.Monthly,
                    TotalContractAmount = 48000m,

                    CreatedAt = new DateTime(2026, 1, 27, 0, 0, 0, DateTimeKind.Utc),
                    SignedByRenterAt = new DateTime(2026, 1, 28, 0, 0, 0, DateTimeKind.Utc),
                    Status = ContractStatus.Active,

                    FileName = "rental-contract-1000004.pdf",
                    Hash = "050a52314d17bad942a9552a176b93f3c706366c14792f5570379d511bae24ba",
                    TransactionId = null,
                    MerkleRoot = null,
                    FilePath = ContractDocumentPathBuilder.BuildPdfRelativePath(1000004),
                    OtsFilePath = ContractDocumentPathBuilder.BuildOtsRelativePath(1000004)
                },

                // ── CONTRACT 1000005 ──────────────────────────────────────────────────────
                // Expired contract: Renter A in Property 1002 (Owner X)
                new Contract
                {
                    Id = 1000005,
                    PropertyId = 1002,
                    RenterId = renterAId,

                    LeaseStartDate = new DateOnly(2024, 1, 1),
                    LeaseEndDate = new DateOnly(2024, 12, 31),
                    PaymentFrequency = PaymentFrequency.Quarterly,
                    TotalContractAmount = 90000m,

                    CreatedAt = new DateTime(2023, 12, 19, 0, 0, 0, DateTimeKind.Utc),
                    SignedByRenterAt = new DateTime(2023, 12, 20, 0, 0, 0, DateTimeKind.Utc),
                    Status = ContractStatus.Expired,

                    FileName = "rental-contract-1000005.pdf",
                    Hash = "037a1152d09ce6cecda1cc548dfce20efe010d53749dd5b7fa5409c2f1632139",
                    TransactionId = null,
                    MerkleRoot = null,
                    FilePath = ContractDocumentPathBuilder.BuildPdfRelativePath(1000005),
                    OtsFilePath = ContractDocumentPathBuilder.BuildOtsRelativePath(1000005)
                },

                // ── CONTRACT 1000006 ──────────────────────────────────────────────────────
                // Cancelled contract: Renter B in Property 1004 (Owner Z)
                new Contract
                {
                    Id = 1000006,
                    PropertyId = 1004,
                    RenterId = renterBId,

                    LeaseStartDate = new DateOnly(2026, 5, 1),
                    LeaseEndDate = new DateOnly(2027, 5, 1),
                    PaymentFrequency = PaymentFrequency.Monthly,
                    TotalContractAmount = 180000m,

                    CreatedAt = new DateTime(2026, 4, 20, 0, 0, 0, DateTimeKind.Utc),
                    SignedByRenterAt = new DateTime(2026, 4, 25, 0, 0, 0, DateTimeKind.Utc),
                    Status = ContractStatus.Cancelled,

                    FileName = "rental-contract-1000006.pdf",
                    Hash = "59aa5fa3b0c47d6473f48638de632bd0e9de58332e4e3d77d6cdc3748c03de96",
                    TransactionId = null,
                    MerkleRoot = null,
                    FilePath = ContractDocumentPathBuilder.BuildPdfRelativePath(1000006),
                    OtsFilePath = ContractDocumentPathBuilder.BuildOtsRelativePath(1000006)
                },

                // ── Scenario Contracts (merged from AdminDashboardScenarioContractSeed) ──

                // Pending contract for dashboard testing
                new Contract
                {
                    Id = 1000101,
                    PropertyId = 1204,
                    RenterId = recentRenterId,
                    Status = ContractStatus.Pending,
                    CreatedAt = new DateTime(2026, 5, 8, 13, 0, 0, DateTimeKind.Utc),
                    PaymentFrequency = PaymentFrequency.Monthly,
                    TotalContractAmount = 15600m,
                    LeaseStartDate = new DateOnly(2026, 6, 1),
                    LeaseEndDate = new DateOnly(2026, 7, 31),
                    SignedByRenterAt = null,
                    FileName = "seed-contract-1000101.pdf",
                    Hash = "SEEDHASH1000101PENDINGADMINDASHBOARD",
                    AnchoringStatus = ContractAnchoringStatus.Pending
                },

                // Active contract for revenue graph testing
                new Contract
                {
                    Id = 1000102,
                    PropertyId = 1003,
                    RenterId = renterEId,
                    Status = ContractStatus.Active,
                    CreatedAt = new DateTime(2025, 11, 28, 12, 0, 0, DateTimeKind.Utc),
                    PaymentFrequency = PaymentFrequency.Monthly,
                    TotalContractAmount = 42000m,
                    LeaseStartDate = new DateOnly(2025, 12, 1),
                    LeaseEndDate = new DateOnly(2026, 6, 30),
                    SignedByRenterAt = new DateTime(2025, 11, 29, 10, 0, 0, DateTimeKind.Utc),
                    FileName = "seed-contract-1000102.pdf",
                    Hash = "SEEDHASH1000102REVENUEGRAPHADMINDASHBOARD",
                    AnchoringStatus = ContractAnchoringStatus.Anchored,
                    AnchoredAt = new DateTime(2025, 11, 30, 9, 0, 0, DateTimeKind.Utc)
                },

                // Active contract for banned renter testing
                new Contract
                {
                    Id = 1000103,
                    PropertyId = 1205,
                    RenterId = bannedRenterId,
                    Status = ContractStatus.Active,
                    CreatedAt = new DateTime(2026, 5, 20, 12, 0, 0, DateTimeKind.Utc),
                    PaymentFrequency = PaymentFrequency.Monthly,
                    TotalContractAmount = 30000m,
                    LeaseStartDate = new DateOnly(2026, 6, 1),
                    LeaseEndDate = new DateOnly(2026, 12, 1),
                    SignedByRenterAt = new DateTime(2026, 5, 21, 10, 0, 0, DateTimeKind.Utc),
                    FileName = "seed-contract-1000103.pdf",
                    Hash = "SEEDHASH1000103BANNEDRENTERDASHBOARD",
                    AnchoringStatus = ContractAnchoringStatus.Anchored,
                    AnchoredAt = new DateTime(2026, 5, 22, 9, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
