using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Enums.Contract;
using MARN_API.Enums.Payment;
using MARN_API.Models;
using System;

namespace MARN_API.Data.Seed
{
    public class AdminDashboardScenarioContractSeed : IEntityTypeConfiguration<Contract>
    {
        public void Configure(EntityTypeBuilder<Contract> builder)
        {
            builder.HasData(
                new Contract
                {
                    Id = AdminDashboardScenarioIds.PendingContractId,
                    PropertyId = AdminDashboardScenarioIds.RecentPropertyId,
                    RenterId = AdminDashboardScenarioIds.RecentRenterId,
                    Status = ContractStatus.Pending,
                    CreatedAt = new DateTime(2026, 5, 8, 13, 0, 0, DateTimeKind.Utc),
                    PaymentFrequency = PaymentFrequency.Monthly,
                    TotalContractAmount = 15600m,
                    LeaseStartDate = new DateOnly(2026, 6, 1),
                    LeaseEndDate = new DateOnly(2026, 7, 31),
                    SignedByRenterAt = null,
                    FileName = "seed-contract-1000101.pdf",
                    Hash = "SEEDHASH1000101PENDINGADMINDASHBOARD",
                    AnchoringStatus = ContractAnchoringStatus.Pending
                },
                new Contract
                {
                    Id = AdminDashboardScenarioIds.RevenueContractId,
                    PropertyId = 1003,
                    RenterId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    Status = ContractStatus.Active,
                    CreatedAt = new DateTime(2025, 11, 28, 12, 0, 0, DateTimeKind.Utc),
                    PaymentFrequency = PaymentFrequency.Monthly,
                    TotalContractAmount = 42000m,
                    LeaseStartDate = new DateOnly(2025, 12, 1),
                    LeaseEndDate = new DateOnly(2026, 6, 30),
                    SignedByRenterAt = new DateTime(2025, 11, 29, 10, 0, 0, DateTimeKind.Utc),
                    FileName = "seed-contract-1000102.pdf",
                    Hash = "SEEDHASH1000102REVENUEGRAPHADMINDASHBOARD",
                    AnchoringStatus = ContractAnchoringStatus.Anchored,
                    AnchoredAt = new DateTime(2025, 11, 30, 9, 0, 0, DateTimeKind.Utc)
                });
        }
    }

    public class AdminDashboardScenarioPaymentScheduleSeed : IEntityTypeConfiguration<PaymentSchedule>
    {
        public void Configure(EntityTypeBuilder<PaymentSchedule> builder)
        {
            builder.HasData(
                new PaymentSchedule
                {
                    Id = AdminDashboardScenarioIds.RevenueScheduleDec2025Id,
                    ContractId = AdminDashboardScenarioIds.RevenueContractId,
                    Amount = 6000m,
                    Currency = "egp",
                    DueDate = new DateTime(2025, 12, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20101"
                },
                new PaymentSchedule
                {
                    Id = AdminDashboardScenarioIds.RevenueScheduleJan2026Id,
                    ContractId = AdminDashboardScenarioIds.RevenueContractId,
                    Amount = 6000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20102"
                },
                new PaymentSchedule
                {
                    Id = AdminDashboardScenarioIds.RevenueScheduleFeb2026Id,
                    ContractId = AdminDashboardScenarioIds.RevenueContractId,
                    Amount = 6000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20103"
                },
                new PaymentSchedule
                {
                    Id = AdminDashboardScenarioIds.RevenueScheduleMar2026Id,
                    ContractId = AdminDashboardScenarioIds.RevenueContractId,
                    Amount = 6000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20104"
                },
                new PaymentSchedule
                {
                    Id = AdminDashboardScenarioIds.RevenueScheduleApr2026Id,
                    ContractId = AdminDashboardScenarioIds.RevenueContractId,
                    Amount = 6000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20105"
                },
                new PaymentSchedule
                {
                    Id = AdminDashboardScenarioIds.RevenueScheduleMay2026Id,
                    ContractId = AdminDashboardScenarioIds.RevenueContractId,
                    Amount = 6000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.PaidOnTime,
                    PaymentIntentId = "pi_seed_20106"
                },
                new PaymentSchedule
                {
                    Id = AdminDashboardScenarioIds.RevenueScheduleJun2026Id,
                    ContractId = AdminDashboardScenarioIds.RevenueContractId,
                    Amount = 6000m,
                    Currency = "egp",
                    DueDate = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                    Status = PaymentScheduleStatus.NotAvailableYet
                });
        }
    }

    public class AdminDashboardScenarioPaymentSeed : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasData(
                new Payment
                {
                    Id = AdminDashboardScenarioIds.RevenuePaymentDec2025Id,
                    PaymentScheduleId = AdminDashboardScenarioIds.RevenueScheduleDec2025Id,
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
                    Id = AdminDashboardScenarioIds.RevenuePaymentJan2026Id,
                    PaymentScheduleId = AdminDashboardScenarioIds.RevenueScheduleJan2026Id,
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
                    Id = AdminDashboardScenarioIds.RevenuePaymentFeb2026Id,
                    PaymentScheduleId = AdminDashboardScenarioIds.RevenueScheduleFeb2026Id,
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
                    Id = AdminDashboardScenarioIds.RevenuePaymentMar2026Id,
                    PaymentScheduleId = AdminDashboardScenarioIds.RevenueScheduleMar2026Id,
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
                    Id = AdminDashboardScenarioIds.RevenuePaymentApr2026Id,
                    PaymentScheduleId = AdminDashboardScenarioIds.RevenueScheduleApr2026Id,
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
                    Id = AdminDashboardScenarioIds.RevenuePaymentMay2026Id,
                    PaymentScheduleId = AdminDashboardScenarioIds.RevenueScheduleMay2026Id,
                    AmountTotal = 6000m,
                    PlatformFee = 600m,
                    OwnerAmount = 5400m,
                    Currency = "egp",
                    PaymentIntentId = "pi_seed_20106",
                    PaidAt = new DateTime(2026, 5, 1, 10, 0, 0, DateTimeKind.Utc),
                    AvailableAt = new DateTime(2026, 5, 11, 10, 0, 0, DateTimeKind.Utc),
                    Status = PaymentStatus.Available
                });
        }
    }
}
