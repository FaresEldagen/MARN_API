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
    ///  ── CONTRACT 1000002 (Active Quarterly, Renter B, Property 1002) ──────────────────────
    ///  20010        | 1000002 | PaidEarly         | Paid 10 days before due
    ///  20011        | 1000002 | Overdue           | Missed – 45 days past due
    ///  20012        | 1000002 | Available         | Due in 6 days (within 7-day window)
    ///  20013        | 1000002 | NotAvailableYet   | Due in 90 days
    ///
    ///  ── CONTRACT 1000003 (Active Yearly, Renter C, Property 1003) ─────────────────────────
    ///  20020        | 1000003 | PaidOnTime        | First year paid on time
    ///  20021        | 1000003 | NotAvailableYet   | Second year – due 2027-06-01 (far future)
    ///
    ///  ── CONTRACT 1000004 (Active Monthly, Renter A, Property 1004 / Owner Z) ──────────────
    ///  20030        | 1000004 | PaidEarly         | Jan – paid 3 days early
    ///  20031        | 1000004 | PaidOnTime        | Feb – paid on time
    ///  20032        | 1000004 | PaidLate          | Mar – paid 7 days late
    ///  20033        | 1000004 | PaidOnTime        | Apr – paid on time
    ///  20034        | 1000004 | Overdue           | May – overdue (missed)
    ///  20035        | 1000004 | Available         | Jun – available (due in 3 days)
    ///  20036        | 1000004 | NotAvailableYet   | Jul – not yet available
    ///  20037        | 1000004 | NotAvailableYet   | Aug – not yet available
    ///
    ///  ── CONTRACT 1000005 (Active One-Time, Renter B, Property 1001) ───────────────────────
    ///  20040        | 1000005 | PaidLate          | Single one-time instalment paid late
    ///
    ///  ── CONTRACT 1000006 (Expired Quarterly, Renter A, Property 1002) ─────────────────────
    ///  20050        | 1000006 | PaidOnTime        | Q1 2024
    ///  20051        | 1000006 | PaidOnTime        | Q2 2024
    ///  20052        | 1000006 | PaidLate          | Q3 2024
    ///  20053        | 1000006 | PaidEarly         | Q4 2024
    ///
    ///  ── CONTRACT 1000007 (Cancelled, Renter B, Property 1004) ────────────────────────────
    ///  20060        | 1000007 | NotAvailableYet   | Should never be payable (contract cancelled)
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
                    DueDate = new DateTime(2025, 1, 31, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidEarly,
                    PaymentIntentId = "pi_seed_20001"
                },
                new PaymentSchedule
                {
                    Id = 20002,
                    ContractId = 1000001,
                    Amount = 5000m,
                    Currency = "egp",
                    DueDate = new DateTime(2025, 2, 28, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20002"
                },
                new PaymentSchedule
                {
                    Id = 20003,
                    ContractId = 1000001,
                    Amount = 5000m,
                    Currency = "egp",
                    DueDate = new DateTime(2025, 3, 31, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidLate,
                    PaymentIntentId = "pi_seed_20003"
                },
                new PaymentSchedule
                {
                    Id = 20004,
                    ContractId = 1000001,
                    Amount = 5000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 4, 21, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.Overdue
                },
                new PaymentSchedule
                {
                    Id = 20005,
                    ContractId = 1000001,
                    Amount = 5000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 5, 6, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.DueToday
                },
                new PaymentSchedule
                {
                    Id = 20006,
                    ContractId = 1000001,
                    Amount = 5000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 5, 10, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.Available
                },
                new PaymentSchedule
                {
                    Id = 20007,
                    ContractId = 1000001,
                    Amount = 5000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 6, 5, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.NotAvailableYet
                },
                new PaymentSchedule
                {
                    Id = 20008,
                    ContractId = 1000001,
                    Amount = 5000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 7, 5, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.NotAvailableYet
                },
                new PaymentSchedule
                {
                    Id = 20009,
                    ContractId = 1000001,
                    Amount = 5000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 8, 5, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.NotAvailableYet
                },

                // ── CONTRACT 1000002 ────────────────────────────────────────────────────────────
                new PaymentSchedule
                {
                    Id = 20010,
                    ContractId = 1000002,
                    Amount = 22500m,
                    Currency = "egp",
                    DueDate = new DateTime(2025, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidEarly,
                    PaymentIntentId = "pi_seed_20010"
                },
                new PaymentSchedule
                {
                    Id = 20011,
                    ContractId = 1000002,
                    Amount = 22500m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 3, 22, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.Overdue
                },
                new PaymentSchedule
                {
                    Id = 20012,
                    ContractId = 1000002,
                    Amount = 22500m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 5, 12, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.Available
                },
                new PaymentSchedule
                {
                    Id = 20013,
                    ContractId = 1000002,
                    Amount = 22500m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 8, 12, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.NotAvailableYet
                },

                // ── CONTRACT 1000003 ────────────────────────────────────────────────────────────
                new PaymentSchedule
                {
                    Id = 20020,
                    ContractId = 1000003,
                    Amount = 42000m,
                    Currency = "egp",
                    DueDate = new DateTime(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20020"
                },
                new PaymentSchedule
                {
                    Id = 20021,
                    ContractId = 1000003,
                    Amount = 42000m,
                    Currency = "egp",
                    DueDate = new DateTime(2027, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.NotAvailableYet
                },

                // ── CONTRACT 1000004 ───────────────────────────────────
                new PaymentSchedule
                {
                    Id = 20030,
                    ContractId = 1000004,
                    Amount = 15000m,
                    Currency = "egp",
                    DueDate = new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidEarly,
                    PaymentIntentId = "pi_seed_20030"
                },
                new PaymentSchedule
                {
                    Id = 20031,
                    ContractId = 1000004,
                    Amount = 15000m,
                    Currency = "egp",
                    DueDate = new DateTime(2025, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20031"
                },
                new PaymentSchedule
                {
                    Id = 20032,
                    ContractId = 1000004,
                    Amount = 15000m,
                    Currency = "egp",
                    DueDate = new DateTime(2025, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidLate,
                    PaymentIntentId = "pi_seed_20032"
                },
                new PaymentSchedule
                {
                    Id = 20033,
                    ContractId = 1000004,
                    Amount = 15000m,
                    Currency = "egp",
                    DueDate = new DateTime(2025, 8, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20033"
                },
                new PaymentSchedule
                {
                    Id = 20034,
                    ContractId = 1000004,
                    Amount = 15000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 4, 11, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.Overdue
                },
                new PaymentSchedule
                {
                    Id = 20035,
                    ContractId = 1000004,
                    Amount = 15000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 5, 9, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.Available
                },
                new PaymentSchedule
                {
                    Id = 20036,
                    ContractId = 1000004,
                    Amount = 15000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 6, 9, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.NotAvailableYet
                },
                new PaymentSchedule
                {
                    Id = 20037,
                    ContractId = 1000004,
                    Amount = 15000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 7, 9, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.NotAvailableYet
                },

                // ── CONTRACT 1000005 ────────────────────────────
                new PaymentSchedule
                {
                    Id = 20040,
                    ContractId = 1000005,
                    Amount = 30000m,
                    Currency = "egp",
                    DueDate = new DateTime(2025, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidLate,
                    PaymentIntentId = "pi_seed_20040"
                },

                // ── CONTRACT 1000006 ───────────────────
                new PaymentSchedule
                {
                    Id = 20050,
                    ContractId = 1000006,
                    Amount = 9000m,
                    Currency = "egp",
                    DueDate = new DateTime(2024, 3, 31, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20050"
                },
                new PaymentSchedule
                {
                    Id = 20051,
                    ContractId = 1000006,
                    Amount = 9000m,
                    Currency = "egp",
                    DueDate = new DateTime(2024, 6, 30, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20051"
                },
                new PaymentSchedule
                {
                    Id = 20052,
                    ContractId = 1000006,
                    Amount = 9000m,
                    Currency = "egp",
                    DueDate = new DateTime(2024, 9, 30, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidLate,
                    PaymentIntentId = "pi_seed_20052"
                },
                new PaymentSchedule
                {
                    Id = 20053,
                    ContractId = 1000006,
                    Amount = 9000m,
                    Currency = "egp",
                    DueDate = new DateTime(2024, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidEarly,
                    PaymentIntentId = "pi_seed_20053"
                },

                // ── CONTRACT 1000007 ───────────────
                new PaymentSchedule
                {
                    Id = 20060,
                    ContractId = 1000007,
                    Amount = 15000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.NotAvailableYet
                }
            );
        }
    }
}
