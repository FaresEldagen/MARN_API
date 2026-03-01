using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MARN_API.Enums;
using MARN_API.Models;

namespace MARN_API.Data.Configurations
{
    public class ChatRoomConfiguration : IEntityTypeConfiguration<ChatRoom>
    {
        public void Configure(EntityTypeBuilder<ChatRoom> builder)
        {
            builder.Property(cr => cr.Type).HasConversion<int>();
            builder.Property(cr => cr.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(cr => cr.Type).IsRequired();

            builder.HasOne(cr => cr.Request)
                   .WithOne(br => br.ChatRoom)
                   .HasForeignKey<ChatRoom>(cr => cr.RequestId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}



