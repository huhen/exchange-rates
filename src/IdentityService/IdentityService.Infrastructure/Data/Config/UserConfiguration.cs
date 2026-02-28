using IdentityService.Core.UserAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Infrastructure.Data.Config;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(entity => entity.Id);
        
        builder.Property(entity => entity.Id)
            .HasDefaultValueSql("uuidv7()")
            .ValueGeneratedOnAdd();

        builder.Property(entity => entity.Name)
            .HasMaxLength(UserName.MaxLength)
            .IsRequired();

        builder.Property(entity => entity.PasswordHash)
            .HasMaxLength(UserPasswordHash.MaxLength)
            .IsRequired();
        
        builder.HasIndex(u => u.Name).IsUnique();
    }
}
