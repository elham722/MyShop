using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Identity.Models;

namespace MyShop.Identity.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.UserId)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("User ID who owns the refresh token");

        builder.Property(r => r.Token)
            .IsRequired()
            .HasMaxLength(500)
            .HasComment("Refresh token value");

        builder.Property(r => r.ExpiresAt)
            .IsRequired()
            .HasComment("When the token expires");

        builder.Property(r => r.IsRevoked)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Whether the token is revoked");

        builder.Property(r => r.RevokedAt)
            .HasComment("When the token was revoked");

        builder.Property(r => r.DeviceInfo)
            .HasMaxLength(200)
            .HasComment("Device information");

        builder.Property(r => r.IpAddress)
            .HasMaxLength(45)
            .HasComment("IP address when token was created");

        // Indexes
        builder.HasIndex(r => r.UserId)
            .HasDatabaseName("IX_RefreshToken_UserId");

        builder.HasIndex(r => r.Token)
            .IsUnique()
            .HasDatabaseName("IX_RefreshToken_Token_Unique");

        builder.HasIndex(r => r.ExpiresAt)
            .HasDatabaseName("IX_RefreshToken_ExpiresAt");

        builder.HasIndex(r => r.IsRevoked)
            .HasDatabaseName("IX_RefreshToken_IsRevoked");

        builder.HasIndex(r => new { r.UserId, r.IsRevoked, r.ExpiresAt })
            .HasDatabaseName("IX_RefreshToken_UserId_IsRevoked_ExpiresAt");

        // Table Configuration
        builder.ToTable("RefreshTokens", "Identity")
            .HasComment("Refresh tokens for user sessions");

        // Relationships
        builder.HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}