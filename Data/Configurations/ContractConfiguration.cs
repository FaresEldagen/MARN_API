using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Enums;
using MARN_API.Models;

namespace MARN_API.Data.Configurations
{
    public class ContractConfiguration : IEntityTypeConfiguration<Contract>
    {
        public void Configure(EntityTypeBuilder<Contract> builder)
        {
            builder.Property(c => c.ContractNumber).IsRequired().HasMaxLength(64);
            builder.Property(c => c.FileName).IsRequired().HasMaxLength(260);
            builder.Property(c => c.Hash).IsRequired().HasMaxLength(128);
            builder.Property(c => c.LeaseStartDate).HasColumnType("date");
            builder.Property(c => c.LeaseEndDate).HasColumnType("date");
            builder.Property(c => c.PropertyId).IsRequired();
            builder.Property(c => c.RenterId).IsRequired();
            builder.Property(c => c.OwnerId).IsRequired();

            builder.Property(c => c.Status).HasConversion<int>();
            builder.Property(c => c.AnchoringStatus).HasConversion<int>();
            builder.Property(c => c.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");
            builder.Property(c => c.SubmittedAt)
                   .HasDefaultValueSql("GETUTCDATE()");
            builder.ToTable(t => t.HasCheckConstraint("CK_Contract_Dates", "[LeaseEndDate] IS NULL OR [LeaseStartDate] IS NULL OR [LeaseEndDate] > [LeaseStartDate]"));

            builder.HasOne(c => c.Property)
                   .WithMany(p => p.Contracts)
                   .HasForeignKey(c => c.PropertyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Renter)
                   .WithMany(u => u.ContractsAsRenter)
                   .HasForeignKey(c => c.RenterId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(c => new { c.PropertyId, c.RenterId, c.OwnerId });
            builder.HasIndex(c => c.ContractNumber).IsUnique();
        }
    }
}
