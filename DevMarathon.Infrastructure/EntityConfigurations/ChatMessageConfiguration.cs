
using DevMarathon.Domain.Entities.SQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevMarathon.Infrastructure.EntityConfigurations;

public class ChatMessageConfiguration: IEntityTypeConfiguration<ChatMessageEntity>
{
    public void Configure(EntityTypeBuilder<ChatMessageEntity> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(x => x.Message)
            .HasColumnName("message");
        builder.Property(x => x.FromSystem)
            .HasDefaultValue(false)
            .HasColumnName("from_system");
    }
}