using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagementSystem.DataAccess.Entities;

namespace UserManagementSystem.DataAccess.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);
        builder.Property(x=>x.Id).IsRequired().HasMaxLength(36);
        builder.Property(x=>x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x=>x.Email).HasMaxLength(100).IsRequired();
        builder.HasIndex(x => x.Email).IsUnique();
        builder.Property(x=>x.ActivityStatus).IsRequired();
        builder.Property(x => x.LastActivityTime).IsRequired();
        builder.Property(x => x.RegistrationTime).IsRequired();
    }
}