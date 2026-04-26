using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;

namespace MARN_API.Data.Configurations
{
    public class RentalTransactionConfiguration : IEntityTypeConfiguration<RentalTransaction>
    {
        public void Configure(EntityTypeBuilder<RentalTransaction> builder)
        {
            builder.Property(r => r.RenterId).IsRequired();
            builder.Property(r => r.OwnerId).IsRequired();
            builder.Property(r => r.PropertyId).IsRequired();
            builder.Property(r => r.Status).HasConversion<int>();
            builder.Property(r => r.PaymentStatus).HasConversion<int>();
            builder.Property(r => r.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(r => r.Property)
                   .WithMany(p => p.RentalTransactions)
                   .HasForeignKey(r => r.PropertyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(r => r.StripeSessionId);
            builder.HasIndex(r => r.PaymentId);
            builder.HasIndex(r => r.ContractId);
            builder.HasIndex(r => new { r.RenterId, r.PropertyId, r.CreatedAt });
        }
    }
}
