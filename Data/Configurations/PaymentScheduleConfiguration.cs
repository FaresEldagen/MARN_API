using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Enums.Payment;

using MARN_API.Models;

namespace MARN_API.Data.Configurations
{
    public class PaymentScheduleConfiguration : IEntityTypeConfiguration<PaymentSchedule>
    {
        public void Configure(EntityTypeBuilder<PaymentSchedule> builder)
        {
            builder.Property(p => p.Amount).HasColumnType("decimal(18,2)");
            builder.Property(p => p.Status).HasConversion<int>();

            builder.HasOne(p => p.Contract)
               .WithMany(c => c.PaymentSchedules)
               .HasForeignKey(p => p.ContractId);

            builder.HasIndex(p => new { p.ContractId, p.DueDate });
        }
    }
}
