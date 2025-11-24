using DevMarathon.Domain.Entities.SQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevMarathon.Infrastructure.EntityConfigurations;

public class UserConfiguration: IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(x => x.PhoneNumber)
            .IsRequired()
            .HasColumnName("phone_number")
            .IsRequired()
            .HasMaxLength(11);
    }
}