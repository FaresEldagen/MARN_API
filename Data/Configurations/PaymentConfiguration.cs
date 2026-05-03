using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Enums.Payment;

using MARN_API.Models;

namespace MARN_API.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(p => p.AmountTotal).HasColumnType("decimal(18,2)");
            builder.Property(p => p.OwnerAmount).HasColumnType("decimal(18,2)");
            builder.Property(p => p.PlatformFee).HasColumnType("decimal(18,2)");
            builder.Property(p => p.Status).HasConversion<int>();

            builder.HasIndex(p => new { p.Status, p.AvailableAt });

            builder.HasOne(p => p.PaymentSchedule)
               .WithOne(ps => ps.Payment)
               .HasForeignKey<Payment>(p => p.PaymentScheduleId);

        }
    }
}
