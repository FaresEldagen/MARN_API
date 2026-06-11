using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;
using MARN_API.Enums.Payment;


namespace MARN_API.Data.Seed
{
    /// <summary>
    /// Payment seed – one row per successful Stripe payment_intent.succeeded webhook.
    /// Reference Mock Today: 2026-06-23
    ///
    /// Business rules enforced:
    ///  • AmountTotal = PaymentSchedule.Amount (full rent)
    ///  • PlatformFee = AmountTotal * 0.10   (10%)
    ///  • OwnerAmount = AmountTotal * 0.90   (90%)
    ///  • AvailableAt = PaidAt + 10 days     (fund hold period)
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
                    PaidAt = new DateTime(2026, 1, 29, 12, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2026, 2, 8, 12, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Withdrawn
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
                    PaidAt = new DateTime(2026, 2, 28, 10, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2026, 3, 10, 10, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Withdrawn
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
                    PaidAt = new DateTime(2026, 4, 5, 9, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2026, 4, 15, 9, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Withdrawn
                },

                // Schedule 20004 – PaidOnTime
                new Payment
                {
                    Id = 30004,
                    PaymentScheduleId = 20004,
                    AmountTotal = 5000m,
                    PlatformFee = 500m,
                    OwnerAmount = 4500m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20004",
                    PaidAt = new DateTime(2026, 5, 5, 10, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2026, 5, 15, 10, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },

                // Schedule 20005 – PaidOnTime
                new Payment
                {
                    Id = 30005,
                    PaymentScheduleId = 20005,
                    AmountTotal = 5000m,
                    PlatformFee = 500m,
                    OwnerAmount = 4500m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20005",
                    PaidAt = new DateTime(2026, 6, 5, 9, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2026, 6, 15, 9, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },

                // ── Renter B / Contract 1000004 / Monthly ──────────────────────────────────

                // Schedule 20014 – PaidEarly
                new Payment
                {
                    Id = 30006,
                    PaymentScheduleId = 20014,
                    AmountTotal = 4000m,
                    PlatformFee = 400m,
                    OwnerAmount = 3600m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20014",
                    PaidAt = new DateTime(2026, 2, 22, 10, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2026, 3, 4, 10, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Withdrawn
                },

                // Schedule 20015 – PaidOnTime
                new Payment
                {
                    Id = 30007,
                    PaymentScheduleId = 20015,
                    AmountTotal = 4000m,
                    PlatformFee = 400m,
                    OwnerAmount = 3600m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20015",
                    PaidAt = new DateTime(2026, 3, 31, 11, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2026, 4, 10, 11, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Withdrawn
                },

                // Schedule 20016 – PaidLate
                new Payment
                {
                    Id = 30008,
                    PaymentScheduleId = 20016,
                    AmountTotal = 4000m,
                    PlatformFee = 400m,
                    OwnerAmount = 3600m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20016",
                    PaidAt = new DateTime(2026, 5, 8, 9, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2026, 5, 18, 9, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Withdrawn
                },

                // Schedule 20017 – PaidOnTime
                new Payment
                {
                    Id = 30009,
                    PaymentScheduleId = 20017,
                    AmountTotal = 4000m,
                    PlatformFee = 400m,
                    OwnerAmount = 3600m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20017",
                    PaidAt = new DateTime(2026, 5, 31, 10, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2026, 6, 10, 10, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },

                // ── Renter A / Contract 1000005 / Quarterly (Expired) ──────────────────────

                // Schedule 20025 – PaidLate
                new Payment
                {
                    Id = 30010,
                    PaymentScheduleId = 20025,
                    AmountTotal = 22500m,
                    PlatformFee = 2250m,
                    OwnerAmount = 20250m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20025",
                    PaidAt = new DateTime(2024, 4, 5, 14, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2024, 4, 15, 14, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Withdrawn
                },

                // Schedule 20026 – PaidOnTime
                new Payment
                {
                    Id = 30011,
                    PaymentScheduleId = 20026,
                    AmountTotal = 22500m,
                    PlatformFee = 2250m,
                    OwnerAmount = 20250m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20026",
                    PaidAt = new DateTime(2024, 6, 30, 11, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2024, 7, 10, 11, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Withdrawn
                },

                // Schedule 20027 – PaidLate
                new Payment
                {
                    Id = 30012,
                    PaymentScheduleId = 20027,
                    AmountTotal = 22500m,
                    PlatformFee = 2250m,
                    OwnerAmount = 20250m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20027",
                    PaidAt = new DateTime(2024, 10, 3, 10, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2024, 10, 13, 10, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Withdrawn
                },

                // Schedule 20028 – PaidOnTime
                new Payment
                {
                    Id = 30013,
                    PaymentScheduleId = 20028,
                    AmountTotal = 22500m,
                    PlatformFee = 2250m,
                    OwnerAmount = 20250m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20028",
                    PaidAt = new DateTime(2024, 12, 31, 9, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2025, 1, 10, 9, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Withdrawn
                },

                // ── Renter B / Contract 1000006 / Monthly (Cancelled) ──────────────────────

                // Schedule 20029 – PaidOnTime
                new Payment
                {
                    Id = 30014,
                    PaymentScheduleId = 20029,
                    AmountTotal = 15000m,
                    PlatformFee = 1500m,
                    OwnerAmount = 13500m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20029",
                    PaidAt = new DateTime(2025, 5, 31, 10, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2025, 6, 10, 10, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Withdrawn
                },

                // Schedule 20030 – PaidOnTime
                new Payment
                {
                    Id = 30015,
                    PaymentScheduleId = 20030,
                    AmountTotal = 15000m,
                    PlatformFee = 1500m,
                    OwnerAmount = 13500m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20030",
                    PaidAt = new DateTime(2025, 6, 30, 11, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2025, 7, 10, 11, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Withdrawn
                },

                // Schedule 20031 – PaidLate
                new Payment
                {
                    Id = 30016,
                    PaymentScheduleId = 20031,
                    AmountTotal = 15000m,
                    PlatformFee = 1500m,
                    OwnerAmount = 13500m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20031",
                    PaidAt = new DateTime(2025, 8, 4, 9, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2025, 8, 14, 9, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Withdrawn
                },

                // Schedule 20032 – PaidEarly
                new Payment
                {
                    Id = 30017,
                    PaymentScheduleId = 20032,
                    AmountTotal = 15000m,
                    PlatformFee = 1500m,
                    OwnerAmount = 13500m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20032",
                    PaidAt = new DateTime(2025, 8, 29, 10, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2025, 9, 8, 10, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Withdrawn
                },

                // ── Revenue Graph Payments (merged from AdminDashboardScenarioPaymentSeed) ──

                new Payment
                {
                    Id = 30101,
                    PaymentScheduleId = 20101,
                    AmountTotal = 6000m,
                    PlatformFee = 600m,
                    OwnerAmount = 5400m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20101",
                    PaidAt = new DateTime(2025, 12, 1, 10, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2025, 12, 11, 10, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },
                new Payment
                {
                    Id = 30102,
                    PaymentScheduleId = 20102,
                    AmountTotal = 6000m,
                    PlatformFee = 600m,
                    OwnerAmount = 5400m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20102",
                    PaidAt = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2026, 1, 11, 10, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },
                new Payment
                {
                    Id = 30103,
                    PaymentScheduleId = 20103,
                    AmountTotal = 6000m,
                    PlatformFee = 600m,
                    OwnerAmount = 5400m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20103",
                    PaidAt = new DateTime(2026, 2, 1, 10, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2026, 2, 11, 10, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },
                new Payment
                {
                    Id = 30104,
                    PaymentScheduleId = 20104,
                    AmountTotal = 6000m,
                    PlatformFee = 600m,
                    OwnerAmount = 5400m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20104",
                    PaidAt = new DateTime(2026, 3, 1, 10, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2026, 3, 11, 10, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },
                new Payment
                {
                    Id = 30105,
                    PaymentScheduleId = 20105,
                    AmountTotal = 6000m,
                    PlatformFee = 600m,
                    OwnerAmount = 5400m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20105",
                    PaidAt = new DateTime(2026, 4, 1, 10, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2026, 4, 11, 10, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                },
                new Payment
                {
                    Id = 30106,
                    PaymentScheduleId = 20106,
                    AmountTotal = 6000m,
                    PlatformFee = 600m,
                    OwnerAmount = 5400m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20106",
                    PaidAt = new DateTime(2026, 5, 1, 10, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2026, 5, 11, 10, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                }

            );
        }
    }
}
