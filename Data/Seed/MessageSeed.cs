using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;

namespace MARN_API.Data.Seed
{
    public class MessageSeed : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            var renterAId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var ownerXId = Guid.Parse("44444444-4444-4444-4444-444444444444");
            var bannedRenterId = Guid.Parse("10000000-0000-0000-0000-000000000002");

            var encryptedSeedMessage = "XB+UQj6hKk23omCXxH8uwFxZpOCQjhe1tRbMbKMHUIKitggz1H61tTuCsIyQwnDRBEWtEIP3n24n1DyxJMAPTuWIvOprIjOmfp48oVxQa6M=";

            builder.HasData(
                new Message
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    SenderId = renterAId,
                    ReceiverId = ownerXId,
                    // Hi, is the apartment still available for next month?
                    Content = "XB+UQj6hKk23omCXxH8uwFxZpOCQjhe1tRbMbKMHUIKitggz1H61tTuCsIyQwnDRBEWtEIP3n24n1DyxJMAPTuWIvOprIjOmfp48oVxQa6M=",
                    SentAt = new DateTime(2025, 3, 20, 10, 0, 0, DateTimeKind.Utc),
                    ReadAt = new DateTime(2025, 3, 20, 10, 30, 0, DateTimeKind.Utc)
                },
                new Message
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    SenderId = ownerXId,
                    ReceiverId = renterAId,
                    // Yes, it is! Would you like to schedule a visit?
                    Content = "E8jOydWqRhQPRv/E1P+cXgNPhEczTZ62c8OsZm62YoKZnffb6X6KXosOMw92CvheYLt5FO58PHhnweOYeJRQ6A==",
                    SentAt = new DateTime(2025, 3, 20, 11, 0, 0, DateTimeKind.Utc),
                },

                // ── Moderated message (merged from AdminDashboardScenarioMessageSeed) ──
                new Message
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000101"),
                    SenderId = bannedRenterId,
                    ReceiverId = ownerXId,
                    Content = encryptedSeedMessage,
                    SentAt = new DateTime(2026, 4, 12, 19, 30, 0, DateTimeKind.Utc),
                    IsHiddenByModeration = true,
                    HiddenAt = new DateTime(2026, 4, 13, 9, 0, 0, DateTimeKind.Utc),
                    HiddenByAdminId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                    HiddenReason = "Seeded moderation example for admin dashboard testing."
                }
            );
        }
    }
}
