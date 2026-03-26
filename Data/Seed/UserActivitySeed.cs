using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;
using MARN_API.Enums;

namespace MARN_API.Data.Seed
{
    public class UserActivitySeed : IEntityTypeConfiguration<UserActivity>
    {
        public void Configure(EntityTypeBuilder<UserActivity> builder)
        {
            builder.HasData(
                new UserActivity
                {
                    Id = 1,
                    UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Type = UserActivityType.Login,
                    Description = "User logged in.",
                    IPAddress = "127.0.0.1",
                    CreatedAt = new DateTime(2025, 3, 24, 10, 0, 0, DateTimeKind.Utc)
                },
                new UserActivity
                {
                    Id = 2,
                    UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Type = UserActivityType.ViewedProperty,
                    Description = "User viewed property 1001.",
                    Metadata = "{\"PropertyId\": 1001}",
                    CreatedAt = new DateTime(2025, 3, 24, 10, 5, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
