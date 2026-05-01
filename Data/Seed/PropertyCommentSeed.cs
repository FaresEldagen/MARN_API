using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;

namespace MARN_API.Data.Seed
{
    public class PropertyCommentSeed : IEntityTypeConfiguration<PropertyComment>
    {
        public void Configure(EntityTypeBuilder<PropertyComment> builder)
        {
            builder.HasData(
                new PropertyComment
                {
                    Id = 900001,
                    PropertyId = 1001,
                    UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Content = "Great place! Very clean and quiet.",
                    CreatedAt = new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new PropertyComment
                {
                    Id = 900002,
                    PropertyId = 1001,
                    UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Content = "Awesome location, but the neighbors were a bit noisy.",
                    CreatedAt = new DateTime(2025, 3, 5, 0, 0, 0, DateTimeKind.Utc)
                },
                new PropertyComment
                {
                    Id = 900003,
                    PropertyId = 1004,
                    UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Content = "Superb luxury villa. Highly recommend!",
                    CreatedAt = new DateTime(2025, 3, 10, 0, 0, 0, DateTimeKind.Utc)
                });
        }
    }
}
