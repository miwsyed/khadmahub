using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Domain;

namespace MyApp.Infrastructure.Persistence.Configurations;

public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(token => token.Id);

        builder.Property(token => token.UserId)
            .IsRequired();

        builder.Property(token => token.TokenHash)
            .IsRequired()
            .HasMaxLength(128);

        builder.HasIndex(token => token.TokenHash)
            .IsUnique();

        builder.Property(token => token.ExpiresAtUtc)
            .IsRequired()
            .HasColumnType("datetimeoffset");

        builder.Property(token => token.CreatedAtUtc)
            .IsRequired()
            .HasColumnType("datetimeoffset");

        builder.Property(token => token.CreatedByIp)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(token => token.RevokedAtUtc)
            .HasColumnType("datetimeoffset");

        builder.Property(token => token.ReplacedByTokenId)
            .HasColumnType("uniqueidentifier");

        builder.HasOne(token => token.User)
            .WithMany()
            .HasForeignKey(token => token.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(token => token.UserId);
    }
}
