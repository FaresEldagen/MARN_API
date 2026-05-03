using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Enums.Payment;
using MARN_API.Models;
using System;

namespace MARN_API.Data.Seed
{
    public class PaymentScheduleSeed : IEntityTypeConfiguration<PaymentSchedule>
    {
        public void Configure(EntityTypeBuilder<PaymentSchedule> builder)
        {
            builder.HasData(
                new PaymentSchedule
                {
                    Id = 1,
                    ContractId = 950001,
                    Amount = 5000,
                    Currency = "egp",
                    DueDate = DateTime.UtcNow.AddDays(5),
                    Status = PaymentScheduleStatus.Pending
                },
                new PaymentSchedule
                {
                    Id = 2,
                    ContractId = 950001,
                    Amount = 5000,
                    Currency = "egp",
                    DueDate = DateTime.UtcNow.AddMonths(1),
                    Status = PaymentScheduleStatus.Pending
                },
                new PaymentSchedule
                {
                    Id = 3,
                    ContractId = 950001,
                    Amount = 5000,
                    Currency = "egp",
                    DueDate = DateTime.UtcNow.AddMonths(-1),
                    Status = PaymentScheduleStatus.PaidOnTime
                }
            );
        }
    }
}
