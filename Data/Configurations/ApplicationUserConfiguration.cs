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
                     // TPH for User / Owner
                     builder
                         .HasDiscriminator<string>("Discriminator")
                         .HasValue<ApplicationUser>("Renter")
                         .HasValue<Owner>("Owner")
                         .HasValue<Admin>("Admin");

                     // Explicitly set discriminator column length
                     builder.Property("Discriminator")
                         .HasMaxLength(21);

                     builder.Property(u => u.Language).HasConversion<int>();
                     builder.Property(u => u.Gender).HasConversion<int>();
                     builder.Property(u => u.Country).HasConversion<int>();
                     builder.Property(u => u.AccountStatus).HasConversion<int>();

                     builder.Property(u => u.CreatedAt)
                            .HasDefaultValueSql("GETUTCDATE()");

                     builder.HasMany(u => u.Reviews)
                            .WithOne(r => r.User)
                            .HasForeignKey(r => r.UserId)
                            .OnDelete(DeleteBehavior.Restrict);

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
              }
       }
}


