using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Enums;
using MARN_API.Models;
using MARN_API.Enums.Payment;

namespace MARN_API.Data.Seed
{
    /// <summary>
    /// Seed contracts that cover every meaningful business scenario:
    ///
    ///  ID         | Property | Renter  | Frequency   | Status   | Scenario
    /// ------------|----------|---------|-------------|----------|-----------------------------------------
    ///  1000001    | 1001     | A       | Monthly     | Active   | Healthy active monthly rental (main test)
    ///  1000002    | 1002     | B       | Quarterly   | Active   | Active quarterly rental with overdue schedule
    ///  1000003    | 1003     | C       | Yearly      | Active   | Active yearly rental – NotAvailableYet future
    ///  1000004    | 1004     | A       | Monthly     | Active   | Owner Z property – for owner dashboard earnings
    ///  1000005    | 1001     | B       | OneTime     | Active   | One-time payment already paid
    ///  1000006    | 1002     | A       | Monthly     | Expired  | Fully paid expired contract (history)
    ///  1000007    | 1004     | B       | Monthly     | Cancelled| Cancelled – no schedules should be payable
    ///
    /// Using IDs starting from 1,000,001 to avoid conflicts with existing manual or old seed data.
    /// </summary>
    public class ContractSeed : IEntityTypeConfiguration<Contract>
    {
        public void Configure(EntityTypeBuilder<Contract> builder)
        {
            var renterAId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var renterBId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var renterCId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            builder.HasData(

                // ── CONTRACT 1000001 ──────────────────────────────────────────────────────
                // Active monthly rental: Renter A in Property 1001 (Owner X)
                new Contract
                {
                    Id = 1000001,
                    PropertyId = 1001,
                    RenterId = renterAId,
                    LeaseStartDate = new DateOnly(2025, 1, 1),
                    LeaseEndDate = new DateOnly(2026, 1, 1),
                    TotalContractAmount = 60000m,
                    FileName = "seed-contract-1000001.pdf",
                    Hash = "SEEDHASH1000001ACTIVEMONTHLY",
                    SignedByRenterAt = new DateTime(2024, 12, 28, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2024, 12, 27, 0, 0, 0, DateTimeKind.Utc),
                    Status = ContractStatus.Active,
                    AnchoringStatus = ContractAnchoringStatus.Anchored,
                    AnchoredAt = new DateTime(2024, 12, 29, 0, 0, 0, DateTimeKind.Utc),
                    PaymentFrequency = PaymentFrequency.Monthly
                },

                // ── CONTRACT 1000002 ──────────────────────────────────────────────────────
                // Active quarterly rental: Renter B in Property 1002 (Owner X)
                new Contract
                {
                    Id = 1000002,
                    PropertyId = 1002,
                    RenterId = renterBId,
                    LeaseStartDate = new DateOnly(2025, 1, 1),
                    LeaseEndDate = new DateOnly(2026, 1, 1),
                    TotalContractAmount = 90000m,
                    FileName = "seed-contract-1000002.pdf",
                    Hash = "SEEDHASH1000002ACTIVEQUARTERLY",
                    SignedByRenterAt = new DateTime(2024, 12, 29, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2024, 12, 28, 0, 0, 0, DateTimeKind.Utc),
                    Status = ContractStatus.Active,
                    AnchoringStatus = ContractAnchoringStatus.Anchored,
                    AnchoredAt = new DateTime(2024, 12, 30, 0, 0, 0, DateTimeKind.Utc),
                    PaymentFrequency = PaymentFrequency.Quarterly
                },

                // ── CONTRACT 1000003 ──────────────────────────────────────────────────────
                // Active yearly rental: Renter C in Property 1003 (Owner X)
                new Contract
                {
                    Id = 1000003,
                    PropertyId = 1003,
                    RenterId = renterCId,
                    LeaseStartDate = new DateOnly(2024, 6, 1),
                    LeaseEndDate = new DateOnly(2026, 6, 1),
                    TotalContractAmount = 84000m,
                    FileName = "seed-contract-1000003.pdf",
                    Hash = "SEEDHASH1000003ACTIVEYEARLY",
                    SignedByRenterAt = new DateTime(2024, 5, 25, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2024, 5, 24, 0, 0, 0, DateTimeKind.Utc),
                    Status = ContractStatus.Active,
                    AnchoringStatus = ContractAnchoringStatus.Anchored,
                    AnchoredAt = new DateTime(2024, 5, 26, 0, 0, 0, DateTimeKind.Utc),
                    PaymentFrequency = PaymentFrequency.Yearly
                },

                // ── CONTRACT 1000004 ──────────────────────────────────────────────────────
                // Active monthly rental: Renter A in Property 1004 (Owner Z)
                new Contract
                {
                    Id = 1000004,
                    PropertyId = 1004,
                    RenterId = renterAId,
                    LeaseStartDate = new DateOnly(2025, 2, 1),
                    LeaseEndDate = new DateOnly(2026, 2, 1),
                    TotalContractAmount = 180000m,
                    FileName = "seed-contract-1000004.pdf",
                    Hash = "SEEDHASH1000004OWNERZMONTHLY",
                    SignedByRenterAt = new DateTime(2025, 1, 28, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2025, 1, 27, 0, 0, 0, DateTimeKind.Utc),
                    Status = ContractStatus.Active,
                    AnchoringStatus = ContractAnchoringStatus.Anchored,
                    AnchoredAt = new DateTime(2025, 1, 29, 0, 0, 0, DateTimeKind.Utc),
                    PaymentFrequency = PaymentFrequency.Monthly
                },

                // ── CONTRACT 1000005 ──────────────────────────────────────────────────────
                // Active one-time rental: Renter B in Property 1001 (Owner X)
                new Contract
                {
                    Id = 1000005,
                    PropertyId = 1001,
                    RenterId = renterBId,
                    LeaseStartDate = new DateOnly(2025, 3, 1),
                    LeaseEndDate = new DateOnly(2025, 9, 1),
                    TotalContractAmount = 30000m,
                    FileName = "seed-contract-1000005.pdf",
                    Hash = "SEEDHASH1000005ONETIME",
                    SignedByRenterAt = new DateTime(2025, 2, 25, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2025, 2, 24, 0, 0, 0, DateTimeKind.Utc),
                    Status = ContractStatus.Active,
                    AnchoringStatus = ContractAnchoringStatus.Pending,
                    PaymentFrequency = PaymentFrequency.OneTime
                },

                // ── CONTRACT 1000006 ──────────────────────────────────────────────────────
                // Expired contract: Renter A in Property 1002 (Owner X)
                new Contract
                {
                    Id = 1000006,
                    PropertyId = 1002,
                    RenterId = renterAId,
                    LeaseStartDate = new DateOnly(2024, 1, 1),
                    LeaseEndDate = new DateOnly(2024, 12, 31),
                    TotalContractAmount = 36000m,
                    FileName = "seed-contract-1000006.pdf",
                    Hash = "SEEDHASH1000006EXPIRED",
                    SignedByRenterAt = new DateTime(2023, 12, 20, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2023, 12, 19, 0, 0, 0, DateTimeKind.Utc),
                    Status = ContractStatus.Expired,
                    AnchoringStatus = ContractAnchoringStatus.Anchored,
                    AnchoredAt = new DateTime(2023, 12, 21, 0, 0, 0, DateTimeKind.Utc),
                    PaymentFrequency = PaymentFrequency.Quarterly
                },

                // ── CONTRACT 1000007 ──────────────────────────────────────────────────────
                // Cancelled contract: Renter B in Property 1004 (Owner Z)
                new Contract
                {
                    Id = 1000007,
                    PropertyId = 1004,
                    RenterId = renterBId,
                    LeaseStartDate = new DateOnly(2025, 5, 1),
                    LeaseEndDate = new DateOnly(2026, 5, 1),
                    TotalContractAmount = 180000m,
                    FileName = "seed-contract-1000007.pdf",
                    Hash = "SEEDHASH1000007CANCELLED",
                    SignedByRenterAt = null,
                    CreatedAt = new DateTime(2025, 4, 20, 0, 0, 0, DateTimeKind.Utc),
                    Status = ContractStatus.Cancelled,
                    AnchoringStatus = ContractAnchoringStatus.Pending,
                    PaymentFrequency = PaymentFrequency.Monthly
                }
            );
        }
    }
}
