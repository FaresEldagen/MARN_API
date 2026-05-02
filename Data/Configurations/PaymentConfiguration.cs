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
            builder.Property(p => p.StripeSessionId).IsRequired();
            builder.Property(p => p.Currency).IsRequired();
            builder.Property(p => p.AmountTotal).IsRequired();
            builder.Property(p => p.OwnerAmount).IsRequired();
            builder.Property(p => p.PlatformFee).IsRequired();
            builder.Property(p => p.RenterId).IsRequired();
            builder.Property(p => p.OwnerId).IsRequired();
            builder.Property(p => p.PropertyId).IsRequired();
            builder.Property(p => p.OwnerStripeAccountId).IsRequired();

            builder.Property(p => p.AmountTotal).HasColumnType("decimal(18,2)");
            builder.Property(p => p.OwnerAmount).HasColumnType("decimal(18,2)");
            builder.Property(p => p.PlatformFee).HasColumnType("decimal(18,2)");
            builder.Property(p => p.Status).HasConversion<int>();

            builder.Property(p => p.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(p => p.Contract)
                   .WithMany()
                   .HasForeignKey(p => p.ContractId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(p => p.Renter)
                   .WithMany(u => u.PaymentsAsRenter)
                   .HasForeignKey(p => p.RenterId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => p.DueDate);
            builder.HasIndex(p => p.ContractId);
            builder.HasIndex(p => p.StripeSessionId).IsUnique();
            builder.HasIndex(p => new { p.Status, p.AvailableAt });
        }
    }
}
