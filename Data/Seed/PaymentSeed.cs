using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Enums;
using MARN_API.Models;

namespace MARN_API.Data.Seed
{
    public class PaymentSeed : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasData(
                // Payments for Contract 3001 (Renter A) - one pending, one succeeded
                new Payment
                {
                    Id = 4001,
                    ContractId = 3001,
                    TotalAmount = 5000m,
                    OwnerAmount = 4500m,
                    PlatformFee = 500m,
                    DueDate = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                    PaidAt = null,
                    Currency = "EGP",
                    Status = PaymentStatus.Pending,
                    CreatedAt = new DateTime(2025, 3, 5, 0, 0, 0, DateTimeKind.Utc)
                },
                new Payment
                {
                    Id = 4002,
                    ContractId = 3001,
                    TotalAmount = 5000m,
                    OwnerAmount = 4500m,
                    PlatformFee = 500m,
                    DueDate = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    PaidAt = new DateTime(2026, 3, 2, 0, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2020, 1, 15, 0, 0, 0, DateTimeKind.Utc),
                    Currency = "EGP",
                    Status = PaymentStatus.Succeeded,
                    CreatedAt = new DateTime(2025, 3, 4, 0, 0, 0, DateTimeKind.Utc)
                },

                // Payments for Contract 3002 (Renter C) - all succeeded
                new Payment
                {
                    Id = 4003,
                    ContractId = 3002,
                    TotalAmount = 7500m,
                    OwnerAmount = 6750m,
                    PlatformFee = 750m,
                    DueDate = new DateTime(2025, 7, 1, 0, 0, 0, DateTimeKind.Utc),
                    PaidAt = new DateTime(2025, 7, 2, 0, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2020, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    Currency = "EGP",
                    Status = PaymentStatus.Succeeded,
                    CreatedAt = new DateTime(2025, 6, 30, 0, 0, 0, DateTimeKind.Utc)
                },
                new Payment
                {
                    Id = 4004,
                    ContractId = 3002,
                    TotalAmount = 7500m,
                    OwnerAmount = 6750m,
                    PlatformFee = 750m,
                    DueDate = new DateTime(2025, 8, 1, 0, 0, 0, DateTimeKind.Utc),
                    PaidAt = new DateTime(2025, 8, 2, 0, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2035, 12, 1, 0, 0, 0, DateTimeKind.Utc),
                    Currency = "EGP",
                    Status = PaymentStatus.Succeeded,
                    CreatedAt = new DateTime(2025, 7, 30, 0, 0, 0, DateTimeKind.Utc)
                },

                // Payments for Contract 3004 (Owner Z as renter)
                new Payment
                {
                    Id = 4005,
                    ContractId = 3004,
                    TotalAmount = 5000m,
                    OwnerAmount = 4500m,
                    PlatformFee = 500m,
                    DueDate = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    PaidAt = new DateTime(2026, 3, 3, 0, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                    Currency = "EGP",
                    Status = PaymentStatus.Succeeded,
                    CreatedAt = new DateTime(2025, 3, 6, 0, 0, 0, DateTimeKind.Utc)
                },
                new Payment
                {
                    Id = 4006,
                    ContractId = 3004,
                    TotalAmount = 5000m,
                    OwnerAmount = 4500m,
                    PlatformFee = 500m,
                    DueDate = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                    PaidAt = null,
                    Currency = "EGP",
                    Status = PaymentStatus.Pending,
                    CreatedAt = new DateTime(2025, 3, 7, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}

