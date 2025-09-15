
using DocFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocFlow.Infrastructure.Data.EntityFramework.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username).IsRequired().HasMaxLength(50);
            builder.HasIndex(u => u.Username).IsUnique();

            builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.Password).IsRequired();

            builder.Property(u => u.Role).IsRequired();

            builder.Property(u => u.Code).IsRequired(false);

            builder.Property(u => u.EmailVerified).IsRequired();
        }
    }
}
