using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;

namespace MARN_API.Data.Seed
{
    public class UserDeviceSeed : IEntityTypeConfiguration<UserDevice>
    {
        public void Configure(EntityTypeBuilder<UserDevice> builder)
        {
            builder.HasData(
                new UserDevice
                {
                    Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddd01"),
                    UserId = "11111111-1111-1111-1111-111111111111",
                    FcmToken = "fcm-token-renter-a-device-1",
                    LastUpdated = new DateTime(2025, 3, 24, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
