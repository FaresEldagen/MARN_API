using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;

namespace MARN_API.Data.Configurations
{
    public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder.Property(o => o.WithdrawableEarnings).HasColumnType("decimal(18,2)");

            builder.HasMany(o => o.Properties)
                   .WithOne(p => p.Owner)
                   .HasForeignKey(p => p.OwnerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(o => o.BookingRequestsAsOwner)
                   .WithOne(br => br.Owner)
                   .HasForeignKey(br => br.OwnerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(o => o.ContractsAsOwner)
                   .WithOne(c => c.Owner)
                   .HasForeignKey(c => c.OwnerId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}



