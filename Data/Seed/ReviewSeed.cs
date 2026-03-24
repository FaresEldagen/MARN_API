using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;

namespace MARN_API.Data.Seed
{
    public class ReviewSeed : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasData(
                new Review
                {
                    Id = 1,
                    PropertyId = 1001,
                    UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Rating = 5,
                    Comment = "Great place! Very clean and quiet.",
                    CreatedAt = new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new Review
                {
                    Id = 2,
                    PropertyId = 1001,
                    UserId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Rating = 4,
                    Comment = "Awesome location, but the neighbors were a bit noisy.",
                    CreatedAt = new DateTime(2025, 3, 5, 0, 0, 0, DateTimeKind.Utc)
                },
                new Review
                {
                    Id = 3,
                    PropertyId = 1004,
                    UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Rating = 5,
                    Comment = "Superb luxury villa. Highly recommend!",
                    CreatedAt = new DateTime(2025, 3, 10, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
