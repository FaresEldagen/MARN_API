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
            var renterCId = Guid.Parse("33333333-3333-3333-3333-333333333333");
            var ownerXId = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var ownerZId = Guid.Parse("66666666-6666-6666-6666-666666666666");

            builder.HasData(
                new Contract
                {
                    Id = 3001,
                    PropertyId = 1001,
                    RenterId = renterAId,
                    OwnerId = ownerXId,
                    DocumentPath = "/contracts/seed/contract1.pdf",
                    DocumentHash = "SEED-CONTRACT-1-HASH",
                    RenterSignature = "RenterA-Signature",
                    OwnerSignature = "OwnerX-Signature",
                    Status = ContractStatus.Active,
                    StartDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2027, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    PaymentFrequency = PaymentFrequency.Monthly,
                    IsLocked = false,
                    Version = 1,
                    CreatedAt = new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    Snapshot = new ContractSnapshot
                    {
                        RenterSnapshot = new UserSnapshot
                        {
                            Id = renterAId,
                            FullName = "Renter Alpha",
                            Email = "renter.a@example.com",
                            ArabicFullName = "رينتر ألفا",
                            ArabicAddress = "123 شارع النيل، القاهرة",
                            NationalIDNumber = "12345678901234",
                            ProfileImage = "/images/profiles/78e645e4-7c92-4cdc-b3bc-11a8f4ef796c.png"
                        },
                        OwnerSnapshot = new UserSnapshot
                        {
                            Id = ownerXId,
                            FullName = "Owner X",
                            Email = "owner.x@example.com"
                        },
                        PropertySnapshot = new PropertySnapshot
                        {
                            Id = 1001,
                            Title = "Cozy Seed Apartment",
                            Description = "A cozy seeded apartment suitable for testing active rentals.",
                            Address = "123 Seed Street, Cairo",
                            Price = 5000m,
                            RentalUnit = "Monthly",
                            PropertyType = "Apartment",
                            Bedrooms = 2,
                            Bathrooms = 1,
                            Beds = 3
                        }
                    }
                },
                new Contract
                {
                    Id = 3002,
                    PropertyId = 1002,
                    RenterId = renterCId,
                    OwnerId = ownerXId,
                    DocumentPath = "/contracts/seed/contract2.pdf",
                    DocumentHash = "SEED-CONTRACT-2-HASH",
                    RenterSignature = "RenterC-Signature",
                    OwnerSignature = "OwnerX-Signature",
                    Status = ContractStatus.Active,
                    StartDate = new DateTime(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                    PaymentFrequency = PaymentFrequency.Monthly,
                    IsLocked = false,
                    Version = 1,
                    CreatedAt = new DateTime(2025, 3, 2, 0, 0, 0, DateTimeKind.Utc),
                    Snapshot = new ContractSnapshot
                    {
                        RenterSnapshot = new UserSnapshot
                        {
                            Id = renterCId,
                            FullName = "Renter Gamma",
                            Email = "renter.c@example.com"
                        },
                        OwnerSnapshot = new UserSnapshot
                        {
                            Id = ownerXId,
                            FullName = "Owner X",
                            Email = "owner.x@example.com"
                        },
                        PropertySnapshot = new PropertySnapshot
                        {
                            Id = 1002,
                            Title = "Modern Seed Loft",
                            Description = "A modern loft used for pending booking and payments tests.",
                            Address = "456 Integration Avenue, Cairo",
                            Price = 7500m,
                            RentalUnit = "Monthly",
                            PropertyType = "Apartment",
                            Bedrooms = 1,
                            Bathrooms = 1,
                            Beds = 1
                        }
                    }
                },
                new Contract
                {
                    Id = 3003,
                    PropertyId = 1003,
                    RenterId = renterAId,
                    OwnerId = ownerXId,
                    DocumentPath = "/contracts/seed/contract3.pdf",
                    DocumentHash = "SEED-CONTRACT-3-HASH",
                    Status = ContractStatus.Expired,
                    StartDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    PaymentFrequency = PaymentFrequency.Monthly,
                    IsLocked = false,
                    Version = 1,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    Snapshot = new ContractSnapshot
                    {
                        RenterSnapshot = new UserSnapshot
                        {
                            Id = renterAId,
                            FullName = "Renter Alpha",
                            Email = "renter.a@example.com",
                            ArabicFullName = "رينتر ألفا",
                            ArabicAddress = "123 شارع النيل، القاهرة",
                            NationalIDNumber = "12345678901234",
                            ProfileImage = "/images/profiles/78e645e4-7c92-4cdc-b3bc-11a8f4ef796c.png"
                        },
                        OwnerSnapshot = new UserSnapshot
                        {
                            Id = ownerXId,
                            FullName = "Owner X",
                            Email = "owner.x@example.com"
                        },
                        PropertySnapshot = new PropertySnapshot
                        {
                            Id = 1003,
                            Title = "Seed Studio Flat",
                            Description = "A small studio property used for saved properties and pending bookings.",
                            Address = "789 Scenario Road, Cairo",
                            Price = 3500m,
                            RentalUnit = "Monthly",
                            PropertyType = "Studio",
                            Bedrooms = 1,
                            Bathrooms = 1,
                            Beds = 1
                        }
                    }
                },
                // Active contract where Owner Z is the renter (for renter dashboard)
                new Contract
                {
                    Id = 3004,
                    PropertyId = 1001,
                    RenterId = ownerZId,
                    OwnerId = ownerXId,
                    DocumentPath = "/contracts/seed/contract4.pdf",
                    DocumentHash = "SEED-CONTRACT-4-HASH",
                    RenterSignature = "OwnerZ-AsRenter-Signature",
                    OwnerSignature = "OwnerX-Signature",
                    Status = ContractStatus.Active,
                    StartDate = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2027, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    PaymentFrequency = PaymentFrequency.Monthly,
                    IsLocked = false,
                    Version = 1,
                    CreatedAt = new DateTime(2025, 3, 5, 0, 0, 0, DateTimeKind.Utc),
                    Snapshot = new ContractSnapshot
                    {
                        RenterSnapshot = new UserSnapshot
                        {
                            Id = ownerZId,
                            FullName = "Owner Z",
                            Email = "owner.z@example.com"
                        },
                        OwnerSnapshot = new UserSnapshot
                        {
                            Id = ownerXId,
                            FullName = "Owner X",
                            Email = "owner.x@example.com"
                        },
                        PropertySnapshot = new PropertySnapshot
                        {
                            Id = 1001,
                            Title = "Cozy Seed Apartment",
                            Description = "A cozy seeded apartment suitable for testing active rentals.",
                            Address = "123 Seed Street, Cairo",
                            Price = 5000m,
                            RentalUnit = "Monthly",
                            PropertyType = "Apartment",
                            Bedrooms = 2,
                            Bathrooms = 1,
                            Beds = 3
                        }
                    }
                }
            );
        }
    }
}

