using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;
using MARN_API.Enums.Account;

namespace MARN_API.Data.Seed
{
    public class AdminSeed : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            var adminId = Guid.Parse("99999999-9999-9999-9999-999999999999");
            
            // Password: Password123!
            var demoPasswordHash = "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==";

            builder.HasData(
                new Admin
                {
                    Id = adminId,
                    UserName = "admin@marn.com",
                    NormalizedUserName = "ADMIN@MARN.COM",
                    Email = "admin@marn.com",
                    NormalizedEmail = "ADMIN@MARN.COM",
                    PasswordHash = demoPasswordHash,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    SecurityStamp = "SEED-ADMIN-SECURITY-STAMP",
                    ConcurrencyStamp = "SEED-ADMIN-CONCURRENCY-STAMP",

                    FirstName = "System",
                    LastName = "Admin",
                    Language = Language.English,
                    Gender = Gender.Unknown,
                    Country = Country.Egypt,
                    AccountStatus = AccountStatus.Active,
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
