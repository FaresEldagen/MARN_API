using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Enums.Payment;
using MARN_API.Models;

namespace MARN_API.Data.Seed
{
    /// <summary>
    /// Payment schedule seed – one row per instalment, static dates only (no DateTime.UtcNow).
    /// Reference date: 2026-05-06 (today). All DueDates are expressed relative to this date.
    ///
    /// Coverage matrix:
    ///
    ///  Schedule ID | Contract | Status            | Scenario
    /// -------------|----------|-------------------|-------------------------------------------------
    ///  ── CONTRACT 1000001 (Active Monthly, Renter A, Property 1001) ──────────────────────────
    ///  20001        | 1000001 | PaidEarly         | Paid 2 days before due
    ///  20002        | 1000001 | PaidOnTime        | Paid exactly on due date
    ///  20003        | 1000001 | PaidLate          | Paid 5 days after due
    ///  20004        | 1000001 | PaidOnTime        | Paid on time
    ///  20005        | 1000001 | PaidOnTime        | Paid on time
    ///  20006        | 1000001 | Available         | Due in 4 days (within 7-day window)
    ///  20007-20012  | 1000001 | NotAvailableYet   | Future instalments
    ///
    ///  ── CONTRACT 1000003 (Active One-Time, Renter A, Property 1100) ─────────────────────────
    ///  20013        | 1000003 | NotAvailableYet   | Full amount not yet payable
    ///
    ///  ── CONTRACT 1000004 (Active Monthly, Renter B, Property 1100) ──────────────────────────
    ///  20014-20024  | 1000004 | Mixed             | Various payment statuses
    ///
    ///  ── CONTRACT 1000005 (Expired Quarterly, Renter A, Property 1002) ───────────────────────
    ///  20025-20028  | 1000005 | Mixed             | Historical payments
    ///
    ///  ── CONTRACT 1000006 (Cancelled Monthly, Renter B, Property 1004) ───────────────────────
    ///  20029-20032  | 1000006 | Mixed             | Payments before cancellation
    ///
    ///  ── CONTRACT 1000102 (Active Monthly, Renter E, Property 1003 – Revenue Graph) ─────────
    ///  20101-20107  | 1000102 | Mixed             | Monthly revenue graph data
    /// </summary>
    public class PaymentScheduleSeed : IEntityTypeConfiguration<PaymentSchedule>
    {
        public void Configure(EntityTypeBuilder<PaymentSchedule> builder)
        {
            // ── CONTRACT 1000001 ────────────────────────────────────────────────────────────
            builder.HasData(
                new PaymentSchedule
                {
                    Id = 20001,
                    ContractId = 1000001,
                    Amount = 5000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidEarly,
                    PaymentIntentId = "pi_seed_20001"
                },
                new PaymentSchedule
                {
                    Id = 20002,
                    ContractId = 1000001,
                    Amount = 5000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 2, 28, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20002"
                },
                new PaymentSchedule
                {
                    Id = 20003,
                    ContractId = 1000001,
                    Amount = 5000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 3, 31, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidLate,
                    PaymentIntentId = "pi_seed_20003"
                },
                new PaymentSchedule
                {
                    Id = 20004,
                    ContractId = 1000001,
                    Amount = 5000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 4, 30, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20004"
                },
                new PaymentSchedule
                {
                    Id = 20005,
                    ContractId = 1000001,
                    Amount = 5000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 5, 31, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20005"
                },
                new PaymentSchedule
                {
                    Id = 20006,
                    ContractId = 1000001,
                    Amount = 5000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 6, 30, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.Available
                },
                new PaymentSchedule
                {
                    Id = 20007,
                    ContractId = 1000001,
                    Amount = 5000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 7, 31, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.NotAvailableYet
                },
                new PaymentSchedule
                {
                    Id = 20008,
                    ContractId = 1000001,
                    Amount = 5000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 8, 31, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.NotAvailableYet
                },
                new PaymentSchedule
                {
                    Id = 20009,
                    ContractId = 1000001,
                    Amount = 5000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 9, 30, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.NotAvailableYet
                },
                new PaymentSchedule
                {
                    Id = 20010,
                    ContractId = 1000001,
                    Amount = 5000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 10, 31, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.NotAvailableYet
                },
                new PaymentSchedule
                {
                    Id = 20011,
                    ContractId = 1000001,
                    Amount = 5000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 11, 30, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.NotAvailableYet
                },
                new PaymentSchedule
                {
                    Id = 20012,
                    ContractId = 1000001,
                    Amount = 5000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.NotAvailableYet
                },

                // ── CONTRACT 1000003 ────────────────────────────────────────────────────────────
                new PaymentSchedule
                {
                    Id = 20013,
                    ContractId = 1000003,
                    Amount = 96000m,
                    Currency = "egp",
                    DueDate = new DateTime(2027, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.NotAvailableYet
                },

                // ── CONTRACT 1000004 ───────────────────────────────────
                new PaymentSchedule
                {
                    Id = 20014,
                    ContractId = 1000004,
                    Amount = 4000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 2, 25, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidEarly,
                    PaymentIntentId = "pi_seed_20014"
                },
                new PaymentSchedule
                {
                    Id = 20015,
                    ContractId = 1000004,
                    Amount = 4000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 3, 31, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20015"
                },
                new PaymentSchedule
                {
                    Id = 20016,
                    ContractId = 1000004,
                    Amount = 4000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidLate,
                    PaymentIntentId = "pi_seed_20016"
                },
                new PaymentSchedule
                {
                    Id = 20017,
                    ContractId = 1000004,
                    Amount = 4000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 5, 31, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20017"
                },
                new PaymentSchedule
                {
                    Id = 20018,
                    ContractId = 1000004,
                    Amount = 4000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 6, 30, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.Available
                },
                new PaymentSchedule
                {
                    Id = 20019,
                    ContractId = 1000004,
                    Amount = 4000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 7, 31, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.NotAvailableYet
                },
                new PaymentSchedule
                {
                    Id = 20020,
                    ContractId = 1000004,
                    Amount = 4000m,
                    Currency = "egp",
                    DueDate = new DateTime(2025, 8, 31, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.NotAvailableYet
                },
                new PaymentSchedule
                {
                    Id = 20021,
                    ContractId = 1000004,
                    Amount = 4000m,
                    Currency = "egp",
                    DueDate = new DateTime(2025, 9, 30, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.NotAvailableYet
                },
                new PaymentSchedule
                {
                    Id = 20022,
                    ContractId = 1000004,
                    Amount = 4000m,
                    Currency = "egp",
                    DueDate = new DateTime(2025, 10, 31, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.NotAvailableYet
                },
                new PaymentSchedule
                {
                    Id = 20023,
                    ContractId = 1000004,
                    Amount = 4000m,
                    Currency = "egp",
                    DueDate = new DateTime(2025, 11, 30, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.NotAvailableYet
                },
                new PaymentSchedule
                {
                    Id = 20024,
                    ContractId = 1000004,
                    Amount = 4000m,
                    Currency = "egp",
                    DueDate = new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.NotAvailableYet
                },

                // ── CONTRACT 1000005 ────────────────────────────
                new PaymentSchedule
                {
                    Id = 20025,
                    ContractId = 1000005,
                    Amount = 22500m,
                    Currency = "egp",
                    DueDate = new DateTime(2024, 3, 31, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidLate,
                    PaymentIntentId = "pi_seed_20025"
                },
                new PaymentSchedule
                {
                    Id = 20026,
                    ContractId = 1000005,
                    Amount = 22500m,
                    Currency = "egp",
                    DueDate = new DateTime(2024, 6, 30, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20026"
                }, 
                new PaymentSchedule
                {
                    Id = 20027,
                    ContractId = 1000005,
                    Amount = 22500m,
                    Currency = "egp",
                    DueDate = new DateTime(2024, 9, 30, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidLate,
                    PaymentIntentId = "pi_seed_20027"
                }, 
                new PaymentSchedule
                {
                    Id = 20028,
                    ContractId = 1000005,
                    Amount = 22500m,
                    Currency = "egp",
                    DueDate = new DateTime(2024,12, 31, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20028"
                },

                // ── CONTRACT 1000006 ───────────────────
                new PaymentSchedule
                {
                    Id = 20029,
                    ContractId = 1000006,
                    Amount = 15000m,
                    Currency = "egp",
                    DueDate = new DateTime(2025, 5, 31, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20029"
                },
                new PaymentSchedule
                {
                    Id = 20030,
                    ContractId = 1000006,
                    Amount = 15000m,
                    Currency = "egp",
                    DueDate = new DateTime(2025, 6, 30, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20030"
                },
                new PaymentSchedule
                {
                    Id = 20031,
                    ContractId = 1000006,
                    Amount = 15000m,
                    Currency = "egp",
                    DueDate = new DateTime(2025, 7, 31, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidLate,
                    PaymentIntentId = "pi_seed_20031"
                },
                new PaymentSchedule
                {
                    Id = 20032,
                    ContractId = 1000006,
                    Amount = 15000m,
                    Currency = "egp",
                    DueDate = new DateTime(2025, 8, 31, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidEarly,
                    PaymentIntentId = "pi_seed_20032"
                },

                // ── CONTRACT 1000102 (Revenue Graph – merged from AdminDashboardScenarioPaymentScheduleSeed) ──
                new PaymentSchedule
                {
                    Id = 20101,
                    ContractId = 1000102,
                    Amount = 6000m,
                    Currency = "egp",
                    DueDate = new DateTime(2025, 12, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20101"
                },
                new PaymentSchedule
                {
                    Id = 20102,
                    ContractId = 1000102,
                    Amount = 6000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20102"
                },
                new PaymentSchedule
                {
                    Id = 20103,
                    ContractId = 1000102,
                    Amount = 6000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20103"
                },
                new PaymentSchedule
                {
                    Id = 20104,
                    ContractId = 1000102,
                    Amount = 6000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20104"
                },
                new PaymentSchedule
                {
                    Id = 20105,
                    ContractId = 1000102,
                    Amount = 6000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20105"
                },
                new PaymentSchedule
                {
                    Id = 20106,
                    ContractId = 1000102,
                    Amount = 6000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20106"
                },
                new PaymentSchedule
                {
                    Id = 20107,
                    ContractId = 1000102,
                    Amount = 6000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.NotAvailableYet
                },
                new PaymentSchedule
                {
                    Id = 20108,
                    ContractId = 1000103,
                    Amount = 5000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 6, 23, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.Available
                }
            );
        }
    }
}
