using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;
using MARN_API.Enums.Payment;

namespace MARN_API.Data.Seed
{
    public class BookingRequestSeed : IEntityTypeConfiguration<BookingRequest>
    {
        public void Configure(EntityTypeBuilder<BookingRequest> builder)
        {
            var renterAId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var renterBId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var ownerZId = Guid.Parse("66666666-6666-6666-6666-666666666666");

            builder.HasData(
                // Pending booking for Renter A on Property 1002
                new BookingRequest
                {
                    Id = 5001,
                    PropertyId = 1002,
                    RenterId = renterAId,
                    StartDate = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                    PaymentFrequency = PaymentFrequency.Monthly,
                    CreatedAt = new DateTime(2025, 4, 1, 0, 0, 0, DateTimeKind.Utc)
                },

                // Accepted booking for Renter A on Property 1002 (should not show in pending list)
                new BookingRequest
                {
                    Id = 5002,
                    PropertyId = 1002,
                    RenterId = renterAId,
                    StartDate = new DateTime(2025, 9, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc),
                    PaymentFrequency = PaymentFrequency.OneTime,
                    CreatedAt = new DateTime(2025, 3, 10, 0, 0, 0, DateTimeKind.Utc)
                },

                // Pending booking for Renter B on Property 1003
                new BookingRequest
                {
                    Id = 5003,
                    PropertyId = 1003,
                    RenterId = renterBId,
                    StartDate = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    PaymentFrequency = PaymentFrequency.Monthly,
                    CreatedAt = new DateTime(2025, 4, 2, 0, 0, 0, DateTimeKind.Utc)
                },

                // Pending booking for Owner Z (as renter) on Property 1003
                new BookingRequest
                {
                    Id = 5004,
                    PropertyId = 1003,
                    RenterId = ownerZId,
                    StartDate = new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc),
                    PaymentFrequency = PaymentFrequency.OneTime,
                    CreatedAt = new DateTime(2025, 4, 10, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}

