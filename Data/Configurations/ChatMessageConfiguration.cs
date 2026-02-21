using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Models;

namespace MARN_API.Data.Configurations
{
    public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.Property(cm => cm.Message).IsRequired();
            builder.Property(cm => cm.ChatRoomId).IsRequired();
            builder.Property(cm => cm.SenderId).IsRequired();
            builder.Property(cm => cm.Message).IsRequired().HasMaxLength(2000);


            builder.Property(cm => cm.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(cm => cm.ChatRoom)
                   .WithMany(cr => cr.Messages)
                   .HasForeignKey(cm => cm.ChatRoomId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cm => cm.Sender)
                   .WithMany(u => u.ChatMessages)
                   .HasForeignKey(cm => cm.SenderId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(cm => cm.IsEdited)
                   .HasDefaultValue(false);

            builder.HasIndex(cm => cm.SenderId);
        }
    }
}



