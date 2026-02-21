using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Enums;
using MARN_API.Models;

namespace MARN_API.Data.Configurations
{
    public class RoommatePreferenceConfiguration : IEntityTypeConfiguration<RoommatePreference>
    {
        public void Configure(EntityTypeBuilder<RoommatePreference> builder)
        {
            builder.Property(rp => rp.UserId).IsRequired();

            builder.Property(rp => rp.BudgetRangeMin).HasColumnType("decimal(18,2)");
            builder.Property(rp => rp.BudgetRangeMax).HasColumnType("decimal(18,2)");
            builder.ToTable(t => t.HasCheckConstraint("CK_RoommatePreference_Budget",
                    "[BudgetRangeMax] IS NULL OR [BudgetRangeMin] IS NULL OR [BudgetRangeMax] >= [BudgetRangeMin]"));

            builder.Property(rp => rp.EducationLevel).HasConversion<int>();
            builder.Property(rp => rp.FieldOfStudy).HasConversion<int>();
            builder.Property(rp => rp.GuestsFrequency).HasConversion<int>();
            builder.Property(rp => rp.WorkSchedule).HasConversion<int>();
            builder.Property(rp => rp.SharingLevel).HasConversion<int>();

            builder.HasOne(rp => rp.User)
                   .WithOne(u => u.RoommatePreference)
                   .HasForeignKey<RoommatePreference>(rp => rp.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(rp => rp.UserId).IsUnique();
        }
    }
}



