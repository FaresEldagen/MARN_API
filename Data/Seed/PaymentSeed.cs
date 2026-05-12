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
    ///  30020      | 20020       | 2025-06-01           | PaidOnTime (yearly, Renter C)
    ///  30030      | 20030       | 2025-02-26           | PaidEarly (Owner Z, 3 days early)
    ///  30031      | 20031       | 2025-04-01           | PaidOnTime (Owner Z)
    ///  30032      | 20032       | 2025-05-08           | PaidLate (Owner Z, 7 days late)
    ///  30033      | 20033       | 2025-08-01           | PaidOnTime (Owner Z)
    ///  30040      | 20040       | 2025-04-11           | PaidLate (one-time, 10 days late)
    ///  30050      | 20050       | 2024-03-31           | PaidOnTime (expired contract Q1)
    ///  30051      | 20051       | 2024-06-30           | PaidOnTime (expired contract Q2)
    ///  30052      | 20052       | 2024-10-07           | PaidLate  (expired contract Q3)
    ///  30053      | 20053       | 2024-12-25           | PaidEarly (expired contract Q4)
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
                    AmountTotal = 42000m,
                    PlatformFee = 4200m,
                    OwnerAmount = 37800m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20020",
                    PaidAt = new DateTime(2025, 6, 1, 8, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2025, 6, 11, 8, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },

                // ── Renter A / Contract 1000004 / Monthly / Owner Z ────────────────────────

                // Schedule 20030 – PaidEarly
                new Payment
                {
                    Id = 30030,
                    PaymentScheduleId = 20030,
                    AmountTotal = 15000m,
                    PlatformFee = 1500m,
                    OwnerAmount = 13500m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20030",
                    PaidAt = new DateTime(2025, 2, 26, 11, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2025, 3, 8, 11, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },

                // Schedule 20031 – PaidOnTime
                new Payment
                {
                    Id = 30031,
                    PaymentScheduleId = 20031,
                    AmountTotal = 15000m,
                    PlatformFee = 1500m,
                    OwnerAmount = 13500m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20031",
                    PaidAt = new DateTime(2025, 4, 1, 9, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2025, 4, 11, 9, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },

                // Schedule 20032 – PaidLate
                new Payment
                {
                    Id = 30032,
                    PaymentScheduleId = 20032,
                    AmountTotal = 15000m,
                    PlatformFee = 1500m,
                    OwnerAmount = 13500m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20032",
                    PaidAt = new DateTime(2025, 5, 8, 16, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2025, 5, 18, 16, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },

                // Schedule 20033 – PaidOnTime
                new Payment
                {
                    Id = 30033,
                    PaymentScheduleId = 20033,
                    AmountTotal = 15000m,
                    PlatformFee = 1500m,
                    OwnerAmount = 13500m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20033",
                    PaidAt = new DateTime(2025, 8, 1, 10, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2025, 8, 11, 10, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },

                // ── Renter B / Contract 1000005 / OneTime ──────────────────────────────────

                // Schedule 20040 – PaidLate
                new Payment
                {
                    Id = 30040,
                    PaymentScheduleId = 20040,
                    AmountTotal = 30000m,
                    PlatformFee = 3000m,
                    OwnerAmount = 27000m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20040",
                    PaidAt = new DateTime(2025, 4, 11, 13, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2025, 4, 21, 13, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },

                // ── Renter A / Contract 1000006 / Quarterly / Expired ─────────────────────

                // Schedule 20050 – PaidOnTime Q1
                new Payment
                {
                    Id = 30050,
                    PaymentScheduleId = 20050,
                    AmountTotal = 9000m,
                    PlatformFee = 900m,
                    OwnerAmount = 8100m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20050",
                    PaidAt = new DateTime(2024, 3, 31, 10, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2024, 4, 10, 10, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },

                // Schedule 20051 – PaidOnTime Q2
                new Payment
                {
                    Id = 30051,
                    PaymentScheduleId = 20051,
                    AmountTotal = 9000m,
                    PlatformFee = 900m,
                    OwnerAmount = 8100m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20051",
                    PaidAt = new DateTime(2024, 6, 30, 10, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2024, 7, 10, 10, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },

                // Schedule 20052 – PaidLate Q3
                new Payment
                {
                    Id = 30052,
                    PaymentScheduleId = 20052,
                    AmountTotal = 9000m,
                    PlatformFee = 900m,
                    OwnerAmount = 8100m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20052",
                    PaidAt = new DateTime(2024, 10, 7, 10, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2024, 10, 17, 10, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },

                // Schedule 20053 – PaidEarly Q4
                new Payment
                {
                    Id = 30053,
                    PaymentScheduleId = 20053,
                    AmountTotal = 9000m,
                    PlatformFee = 900m,
                    OwnerAmount = 8100m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20053",
                    PaidAt = new DateTime(2024, 12, 25, 10, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2025, 1, 4, 10, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                }
            );
        }
    }
}
