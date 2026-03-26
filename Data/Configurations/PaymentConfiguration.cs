using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Enums;
using MARN_API.Models;

namespace MARN_API.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(p => p.Currency).IsRequired();
            builder.Property(p => p.TotalAmount).IsRequired();
            builder.Property(p => p.OwnerAmount).IsRequired();
            builder.Property(p => p.PlatformFee).IsRequired();
            builder.Property(p => p.ContractId).IsRequired();

            builder.Property(p => p.TotalAmount).HasColumnType("decimal(18,2)");
            builder.Property(p => p.OwnerAmount).HasColumnType("decimal(18,2)");
            builder.Property(p => p.PlatformFee).HasColumnType("decimal(18,2)");
            builder.Property(p => p.Status).HasConversion<int>();

            builder.Property(p => p.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.HasIndex(p => p.DueDate);
            builder.HasIndex(p => p.ContractId);
            builder.HasIndex(p => new { p.Status, p.AvailableAt });
        }
    }
}



