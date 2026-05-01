using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Enums;
using MARN_API.Models;

namespace MARN_API.Data.Seed
{
    public class ContractSeed : IEntityTypeConfiguration<Contract>
    {
        public void Configure(EntityTypeBuilder<Contract> builder)
        {
            var renterAId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var renterBId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var ownerXId = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var ownerZId = Guid.Parse("66666666-6666-6666-6666-666666666666");

            builder.HasData(
                new Contract
                {
                    Id = 950001,
                    PropertyId = 1001,
                    RenterId = renterAId,
                    OwnerId = ownerXId,
                    ContractNumber = "SEED-CONTRACT-1001-ACTIVE-A",
                    LeaseStartDate = new DateOnly(2025, 3, 1),
                    LeaseEndDate = new DateOnly(2026, 3, 1),
                    FileName = "seed-contract-1001-active-a.pdf",
                    Hash = "SEEDHASH1001ACTIVEA",
                    SubmittedAt = new DateTime(2025, 2, 25, 0, 0, 0, DateTimeKind.Utc),
                    SignedByRenterAt = new DateTime(2025, 2, 26, 0, 0, 0, DateTimeKind.Utc),
                    SignedByOwnerAt = new DateTime(2025, 2, 27, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2025, 2, 25, 0, 0, 0, DateTimeKind.Utc),
                    Status = ContractStatus.Active,
                    AnchoringStatus = ContractAnchoringStatus.Pending,
                    PaymentFrequency = PaymentFrequency.Monthly,
                    Version = 1,
                    IsLocked = false
                },
                new Contract
                {
                    Id = 950002,
                    PropertyId = 1001,
                    RenterId = renterBId,
                    OwnerId = ownerXId,
                    ContractNumber = "SEED-CONTRACT-1001-EXPIRED-B",
                    LeaseStartDate = new DateOnly(2024, 1, 1),
                    LeaseEndDate = new DateOnly(2024, 12, 31),
                    FileName = "seed-contract-1001-expired-b.pdf",
                    Hash = "SEEDHASH1001EXPIREDB",
                    SubmittedAt = new DateTime(2023, 12, 20, 0, 0, 0, DateTimeKind.Utc),
                    SignedByRenterAt = new DateTime(2023, 12, 21, 0, 0, 0, DateTimeKind.Utc),
                    SignedByOwnerAt = new DateTime(2023, 12, 22, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2023, 12, 20, 0, 0, 0, DateTimeKind.Utc),
                    Status = ContractStatus.Expired,
                    AnchoringStatus = ContractAnchoringStatus.Pending,
                    PaymentFrequency = PaymentFrequency.Monthly,
                    Version = 1,
                    IsLocked = false
                },
                new Contract
                {
                    Id = 950003,
                    PropertyId = 1004,
                    RenterId = renterAId,
                    OwnerId = ownerZId,
                    ContractNumber = "SEED-CONTRACT-1004-EXPIRED-A",
                    LeaseStartDate = new DateOnly(2024, 6, 1),
                    LeaseEndDate = new DateOnly(2025, 1, 31),
                    FileName = "seed-contract-1004-expired-a.pdf",
                    Hash = "SEEDHASH1004EXPIREDA",
                    SubmittedAt = new DateTime(2024, 5, 20, 0, 0, 0, DateTimeKind.Utc),
                    SignedByRenterAt = new DateTime(2024, 5, 21, 0, 0, 0, DateTimeKind.Utc),
                    SignedByOwnerAt = new DateTime(2024, 5, 22, 0, 0, 0, DateTimeKind.Utc),
                    CreatedAt = new DateTime(2024, 5, 20, 0, 0, 0, DateTimeKind.Utc),
                    Status = ContractStatus.Expired,
                    AnchoringStatus = ContractAnchoringStatus.Pending,
                    PaymentFrequency = PaymentFrequency.Monthly,
                    Version = 1,
                    IsLocked = false
                });
        }
    }
}
