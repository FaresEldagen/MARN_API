using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Enums;
using MARN_API.Models;

namespace MARN_API.Data.Configurations
{
       public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
       {
              public void Configure(EntityTypeBuilder<ApplicationUser> builder)
              {
                     // Global Query Filter: exclude soft-deleted users from all queries
                     builder.HasQueryFilter(u => u.DeletedAt == null);

                     builder.Property(u => u.Language).HasConversion<int>();
                     builder.Property(u => u.Gender).HasConversion<int>();
                     builder.Property(u => u.Country).HasConversion<int>();
                     builder.Property(u => u.AccountStatus).HasConversion<int>();
                     builder.Property(u => u.StatusBeforeBan).HasConversion<int?>();

                     builder.Property(u => u.CreatedAt)
                            .HasDefaultValueSql("GETUTCDATE()");

                     builder.HasMany(u => u.Notifications)
                            .WithOne(n => n.User)
                            .HasForeignKey(n => n.UserId)
                            .OnDelete(DeleteBehavior.Cascade);

                     builder.HasMany(u => u.ReportsFiled)
                            .WithOne(r => r.Reporter)
                            .HasForeignKey(r => r.ReporterId)
                            .OnDelete(DeleteBehavior.Restrict);

                     builder.HasMany(u => u.Activities)
                            .WithOne(a => a.User)
                            .HasForeignKey(a => a.UserId)
                            .OnDelete(DeleteBehavior.Cascade);

                     // Owner relationships (previously in OwnerConfiguration)
                     builder.HasMany(o => o.Properties)
                            .WithOne(p => p.Owner)
                            .HasForeignKey(p => p.OwnerId)
                            .OnDelete(DeleteBehavior.Restrict);

                     // Admin relationships (previously in AdminConfiguration)
                     builder.HasMany(a => a.ReportsReviewed)
                            .WithOne(r => r.Reviewer)
                            .HasForeignKey(r => r.ReviewerId)
                            .OnDelete(DeleteBehavior.Restrict);

                     builder.HasMany(a => a.ActionLogs)
                            .WithOne(l => l.Admin)
                            .HasForeignKey(l => l.AdminId)
                            .OnDelete(DeleteBehavior.Restrict);
              }
       }
}
