using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;

namespace MARN_API.Data.Configurations
{
    public class ConnectedAccountConfiguration : IEntityTypeConfiguration<ConnectedAccount>
    {
        public void Configure(EntityTypeBuilder<ConnectedAccount> builder)
        {
            builder.Property(c => c.ApplicationUserId).IsRequired();
            builder.Property(c => c.StripeAccountId).HasMaxLength(64);
            builder.Property(c => c.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(c => c.ApplicationUser)
                   .WithOne(u => u.ConnectedAccount)
                   .HasForeignKey<ConnectedAccount>(c => c.ApplicationUserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(c => c.ApplicationUserId).IsUnique();
            builder.HasIndex(c => c.StripeAccountId);
        }
    }
}
