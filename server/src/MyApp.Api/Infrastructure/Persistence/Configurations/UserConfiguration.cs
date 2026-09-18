using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Domain;

namespace MyApp.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Username)
            .IsRequired()
            .HasMaxLength(128);

        builder.HasIndex(user => user.Username)
            .IsUnique();

        builder.Property(user => user.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(user => user.FullName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(user => user.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasIndex(user => user.Email)
            .IsUnique();

        builder.Property(user => user.NationalId)
            .HasMaxLength(16);

        builder.HasIndex(user => user.NationalId)
            .IsUnique();

        builder.Property(user => user.Role)
            .IsRequired()
            .HasMaxLength(64)
            .HasDefaultValue("User");

        builder.Property(user => user.IsActive)
            .IsRequired();

        builder.Property(user => user.FailedLoginCount)
            .IsRequired();

        builder.Property(user => user.LockoutEndUtc)
            .HasColumnType("datetimeoffset");

        builder.Property(user => user.LastLoginAtUtc)
            .HasColumnType("datetimeoffset");

        builder.Property(user => user.CreatedAtUtc)
            .IsRequired()
            .HasColumnType("datetimeoffset");

        builder.Property(user => user.UpdatedAtUtc)
            .IsRequired()
            .HasColumnType("datetimeoffset");

        builder.HasIndex(user => user.Role);
    }
}
