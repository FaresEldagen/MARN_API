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
            builder.Property(c => c.DocumentPath).IsRequired();
            builder.Property(c => c.DocumentHash).IsRequired();
            builder.Property(c => c.StartDate).IsRequired();
            builder.Property(c => c.EndDate).IsRequired();
            builder.Property(c => c.PropertyId).IsRequired();
            builder.Property(c => c.RenterId).IsRequired();
            builder.Property(c => c.OwnerId).IsRequired();


            builder.Property(c => c.Status).HasConversion<int>();
            builder.Property(c => c.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");
            builder.ToTable(t => t.HasCheckConstraint("CK_Contract_Dates", "[EndDate] > [StartDate]"));

            builder.HasOne(c => c.Property)
                   .WithMany(p => p.Contracts)
                   .HasForeignKey(c => c.PropertyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Renter)
                   .WithMany(u => u.ContractsAsRenter)
                   .HasForeignKey(c => c.RenterId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Payments)
                   .WithOne(p => p.Contract)
                   .HasForeignKey(p => p.ContractId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(c => new { c.PropertyId, c.RenterId, c.OwnerId });

            builder.Property(c => c.Snapshot)
                   .HasConversion(
                       v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null!),
                       v => System.Text.Json.JsonSerializer.Deserialize<ContractSnapshot>(v, (System.Text.Json.JsonSerializerOptions)null!))
                   .HasColumnType("nvarchar(max)");
        }
    }
}



