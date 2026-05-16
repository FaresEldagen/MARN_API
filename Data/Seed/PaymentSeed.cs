using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;
using MARN_API.Enums.Payment;


namespace MARN_API.Data.Seed
{
    /// <summary>
    /// Payment seed – one row per successful Stripe payment_intent.succeeded webhook.
    ///
    /// Business rules enforced:
    ///  • AmountTotal = PaymentSchedule.Amount (full rent)
    ///  • PlatformFee = AmountTotal * 0.10   (10%)
    ///  • OwnerAmount = AmountTotal * 0.90   (90%)
    ///  • AvailableAt = PaidAt + 10 days     (fund hold period)
    ///
    ///  Payment ID | Schedule ID | PaidAt               | Scenario
    /// ------------|-------------|----------------------|---------------------------------
    ///  30001      | 20001       | 2025-01-29           | PaidEarly (2 days before due)
    ///  30002      | 20002       | 2025-02-28           | PaidOnTime
    ///  30003      | 20003       | 2025-04-05           | PaidLate (5 days after due)
    ///  30010      | 20010       | 2025-03-22           | PaidEarly (10 days before Q2 due)
    ///  30020      | 20020       | 2024-06-01           | PaidOnTime (One-Time, Renter A)
    ///  30030      | 20030       | 2025-01-28           | PaidEarly (Monthly, Renter B)
    ///  30031      | 20031       | 2025-03-01           | PaidOnTime
    ///  30032      | 20032       | 2025-04-08           | PaidLate
    ///  30033      | 20033       | 2025-05-01           | PaidOnTime
    ///  30040      | 20040       | 2024-04-10           | PaidLate (Quarterly, Expired)
    ///  30050      | 20050       | 2025-05-31           | PaidOnTime (Monthly, Cancelled)
    ///  30051      | 20051       | 2025-06-30           | PaidOnTime
    ///  30052      | 20052       | 2025-08-07           | PaidLate
    ///  30053      | 20053       | 2025-08-25           | PaidEarly
    /// </summary>
    public class PaymentSeed : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasData(

                // ── Renter A / Contract 1000001 / Monthly ──────────────────────────────────

                // Schedule 20001 – PaidEarly
                new Payment
                {
                    Id = 30001,
                    PaymentScheduleId = 20001,
                    AmountTotal = 5000m,
                    PlatformFee = 500m,
                    OwnerAmount = 4500m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20001",
                    PaidAt = new DateTime(2025, 1, 29, 12, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2025, 2, 8, 12, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },

                // Schedule 20002 – PaidOnTime
                new Payment
                {
                    Id = 30002,
                    PaymentScheduleId = 20002,
                    AmountTotal = 5000m,
                    PlatformFee = 500m,
                    OwnerAmount = 4500m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20002",
                    PaidAt = new DateTime(2025, 2, 28, 10, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2025, 3, 10, 10, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },

                // Schedule 20003 – PaidLate
                new Payment
                {
                    Id = 30003,
                    PaymentScheduleId = 20003,
                    AmountTotal = 5000m,
                    PlatformFee = 500m,
                    OwnerAmount = 4500m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20003",
                    PaidAt = new DateTime(2025, 4, 5, 9, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2025, 4, 15, 9, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },

                // ── Renter B / Contract 1000002 / Quarterly ────────────────────────────────

                // Schedule 20010 – PaidEarly
                new Payment
                {
                    Id = 30010,
                    PaymentScheduleId = 20010,
                    AmountTotal = 22500m,
                    PlatformFee = 2250m,
                    OwnerAmount = 20250m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20010",
                    PaidAt = new DateTime(2025, 3, 22, 14, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2025, 4, 1, 14, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },

                // ── Renter C / Contract 1000003 / Yearly ───────────────────────────────────

                // Schedule 20020 – PaidOnTime
                new Payment
                {
                    Id = 30020,
                    PaymentScheduleId = 20020,
                    AmountTotal = 96000m,
                    PlatformFee = 9600m,
                    OwnerAmount = 86400m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20020",
                    PaidAt = new DateTime(2024, 6, 1, 8, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2024, 6, 11, 8, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },

                // ── Renter B / Contract 1000004 / Monthly ────────────────────────

                // Schedule 20030 – PaidEarly
                new Payment
                {
                    Id = 30030,
                    PaymentScheduleId = 20030,
                    AmountTotal = 40000m,
                    PlatformFee = 4000m,
                    OwnerAmount = 36000m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20030",
                    PaidAt = new DateTime(2025, 1, 28, 11, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2025, 2, 7, 11, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },

                // Schedule 20031 – PaidOnTime
                new Payment
                {
                    Id = 30031,
                    PaymentScheduleId = 20031,
                    AmountTotal = 40000m,
                    PlatformFee = 4000m,
                    OwnerAmount = 36000m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20031",
                    PaidAt = new DateTime(2025, 3, 1, 9, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2025, 3, 11, 9, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },

                // Schedule 20032 – PaidLate
                new Payment
                {
                    Id = 30032,
                    PaymentScheduleId = 20032,
                    AmountTotal = 40000m,
                    PlatformFee = 4000m,
                    OwnerAmount = 36000m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20032",
                    PaidAt = new DateTime(2025, 4, 8, 16, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2025, 4, 18, 16, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },

                // Schedule 20033 – PaidOnTime
                new Payment
                {
                    Id = 30033,
                    PaymentScheduleId = 20033,
                    AmountTotal = 40000m,
                    PlatformFee = 4000m,
                    OwnerAmount = 36000m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20033",
                    PaidAt = new DateTime(2025, 5, 1, 10, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2025, 5, 11, 10, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },

                // ── Renter A / Contract 1000005 / Quarterly / Expired ─────────────────────

                // Schedule 20040 – PaidLate
                new Payment
                {
                    Id = 30040,
                    PaymentScheduleId = 20040,
                    AmountTotal = 22500m,
                    PlatformFee = 2250m,
                    OwnerAmount = 20250m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20040",
                    PaidAt = new DateTime(2024, 4, 10, 13, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2024, 4, 20, 13, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },

                // ── Renter B / Contract 1000006 / Monthly / Cancelled ─────────────────────

                // Schedule 20050 – PaidOnTime Q1
                new Payment
                {
                    Id = 30050,
                    PaymentScheduleId = 20050,
                    AmountTotal = 15000m,
                    PlatformFee = 1500m,
                    OwnerAmount = 13500m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20050",
                    PaidAt = new DateTime(2025, 5, 31, 10, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2025, 6, 10, 10, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },

                // Schedule 20051 – PaidOnTime Q2
                new Payment
                {
                    Id = 30051,
                    PaymentScheduleId = 20051,
                    AmountTotal = 15000m,
                    PlatformFee = 1500m,
                    OwnerAmount = 13500m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20051",
                    PaidAt = new DateTime(2025, 6, 30, 10, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2025, 7, 10, 10, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },

                // Schedule 20052 – PaidLate Q3
                new Payment
                {
                    Id = 30052,
                    PaymentScheduleId = 20052,
                    AmountTotal = 15000m,
                    PlatformFee = 1500m,
                    OwnerAmount = 13500m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20052",
                    PaidAt = new DateTime(2025, 8, 7, 10, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2025, 8, 17, 10, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },

                // Schedule 20053 – PaidEarly Q4
                new Payment
                {
                    Id = 30053,
                    PaymentScheduleId = 20053,
                    AmountTotal = 15000m,
                    PlatformFee = 1500m,
                    OwnerAmount = 13500m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20053",
                    PaidAt = new DateTime(2025, 8, 25, 10, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2025, 9, 4, 10, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                }
            );
        }
    }
}
