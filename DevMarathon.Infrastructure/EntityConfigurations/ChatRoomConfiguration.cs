using DevMarathon.Domain.Entities.SQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevMarathon.Infrastructure.EntityConfigurations;

public class ChatRoomConfiguration: IEntityTypeConfiguration<ChatRoomEntity>
{
    public void Configure(EntityTypeBuilder<ChatRoomEntity> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(x => x.Title)
            .HasColumnName("title")
            .IsRequired(false)
            .HasMaxLength(100);
        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .HasMaxLength(200);
    }
}