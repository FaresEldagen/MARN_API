using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Enums;
using MARN_API.Models;

namespace MARN_API.Data.Seed
{
    public class OwnerSeed : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            var ownerXId = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var ownerYId = Guid.Parse("55555555-5555-5555-5555-555555555555");

            // Demo password for owners as well:
            // Password: Password123!
            var demoPasswordHash = "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==";

            builder.HasData(
                new Owner
                {
                    Id = ownerXId,
                    UserName = "owner.x@example.com",
                    NormalizedUserName = "OWNER.X@EXAMPLE.COM",
                    Email = "owner.x@example.com",
                    NormalizedEmail = "OWNER.X@EXAMPLE.COM",
                    PasswordHash = demoPasswordHash,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    SecurityStamp = "SEED-OWNER-X-SECURITY-STAMP",
                    ConcurrencyStamp = "SEED-OWNER-X-CONCURRENCY-STAMP",

                    FirstName = "Owner",
                    LastName = "X",
                    Language = Language.English,
                    Gender = Gender.Male,
                    Country = Country.Egypt,
                    AccountStatus = AccountStatus.Active,
                    CreatedAt = new DateTime(2025, 1, 4, 0, 0, 0, DateTimeKind.Utc),

                    WithdrawableEarnings = 10000m
                },
                new Owner
                {
                    Id = ownerYId,
                    UserName = "owner.y@example.com",
                    NormalizedUserName = "OWNER.Y@EXAMPLE.COM",
                    Email = "owner.y@example.com",
                    NormalizedEmail = "OWNER.Y@EXAMPLE.COM",
                    PasswordHash = demoPasswordHash,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    SecurityStamp = "SEED-OWNER-Y-SECURITY-STAMP",
                    ConcurrencyStamp = "SEED-OWNER-Y-CONCURRENCY-STAMP",

                    FirstName = "Owner",
                    LastName = "Y",
                    Language = Language.English,
                    Gender = Gender.Female,
                    Country = Country.Egypt,
                    AccountStatus = AccountStatus.Active,
                    CreatedAt = new DateTime(2025, 1, 5, 0, 0, 0, DateTimeKind.Utc),

                    WithdrawableEarnings = 0m
                }
            );
        }
    }
}

