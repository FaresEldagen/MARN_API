using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Enums.Payment;
using MARN_API.Models;
using System;

namespace MARN_API.Data.Seed
{
    public class PaymentSeed : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasData(
                new Payment
                {
                    Id = 1,
                    PaymentScheduleId = 3, // Linked to the PaidOnTime schedule
                    AmountTotal = 5000,
                    PlatformFee = 500,
                    OwnerAmount = 4500,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_1",
                    PaidAt = DateTime.UtcNow.AddMonths(-1),
                    AvailableAt = DateTime.UtcNow.AddMonths(-1).AddDays(7),
                    Status = PaymentStatus.Succeeded
                }
            );
        }
    }
}
