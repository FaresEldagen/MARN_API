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
            builder.Property(p => p.StripePaymentIntentId).IsRequired();
            builder.Property(p => p.StripeCustomerId).IsRequired();
            builder.Property(p => p.PaymentMethod).IsRequired();
            builder.Property(p => p.Currency).IsRequired();
            builder.Property(p => p.Amount).IsRequired();
            builder.Property(p => p.Type).IsRequired();
            builder.Property(p => p.UserId).IsRequired();
            builder.Property(p => p.ContractId).IsRequired();


            builder.Property(p => p.Amount).HasColumnType("decimal(18,2)");
            builder.Property(p => p.Type).HasConversion<int>();
            builder.Property(p => p.Status).HasConversion<int>();

            builder.Property(p => p.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.HasIndex(p => p.UserId);
            builder.HasIndex(p => p.ContractId);
        }
    }
}



