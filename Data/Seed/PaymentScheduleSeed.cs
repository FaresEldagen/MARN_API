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
    ///  20004        | 1000001 | Overdue           | Missed – 15 days past due date
    ///  20005        | 1000001 | DueToday          | Due exactly today (2026-05-06)
    ///  20006        | 1000001 | Available         | Due in 4 days (within 7-day window)
    ///  20007        | 1000001 | NotAvailableYet   | Due in 30 days (future, not yet payable)
    ///  20008        | 1000001 | NotAvailableYet   | Due in 60 days (far future)
    ///  20009        | 1000001 | NotAvailableYet   | Due in 90 days (far future, last instalment)
    ///
    ///  ── CONTRACT 1000003 (Active One-Time, Renter A, Property 1100) ─────────────────────────
    ///  20020        | 1000003 | PaidOnTime        | Full amount paid on time
    ///
    ///  ── CONTRACT 1000004 (Active Monthly, Renter B, Property 1100) ──────────────
    ///  20030        | 1000004 | PaidEarly         | Feb – paid 3 days early
    ///  20031        | 1000004 | PaidOnTime        | Mar – paid on time
    ///  20032        | 1000004 | PaidLate          | Apr – paid 7 days late
    ///  20033        | 1000004 | PaidOnTime        | May – paid on time
    ///  20034        | 1000004 | Overdue           | Jun – overdue (missed)
    ///  20035        | 1000004 | Available         | Jul – available
    ///  20036        | 1000004 | NotAvailableYet   | Aug – not yet available
    ///  20037        | 1000004 | NotAvailableYet   | Sep – not yet available
    ///
    ///  ── CONTRACT 1000005 (Expired Quarterly, Renter A, Property 1002) ─────────────────────
    ///  20040        | 1000005 | PaidLate          | Q1 2024
    ///
    ///  ── CONTRACT 1000006 (Cancelled Monthly, Renter B, Property 1004) ─────────────────────
    ///  20050        | 1000006 | PaidOnTime        | Month 1
    ///  20051        | 1000006 | PaidOnTime        | Month 2
    ///  20052        | 1000006 | PaidLate          | Month 3
    ///  20053        | 1000006 | PaidEarly         | Month 4
    ///  20060        | 1000006 | NotAvailableYet   | Future schedule (cancelled)
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
                }
            );
        }
    }
}
