using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;

namespace MARN_API.Data.Configurations
{
    public class ChatRoomParticipantConfiguration : IEntityTypeConfiguration<ChatRoomParticipant>
    {
        public void Configure(EntityTypeBuilder<ChatRoomParticipant> builder)
        {
            builder.HasKey(cp => new { cp.ChatRoomId, cp.UserId });

            builder.Property(cp => cp.JoinedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(cp => cp.ChatRoomId).IsRequired();
            builder.Property(cp => cp.UserId).IsRequired();

            builder.HasOne(cp => cp.ChatRoom)
                   .WithMany(cr => cr.Participants)
                   .HasForeignKey(cp => cp.ChatRoomId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cp => cp.User)
                   .WithMany(u => u.ChatRoomParticipants)
                   .HasForeignKey(cp => cp.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}



