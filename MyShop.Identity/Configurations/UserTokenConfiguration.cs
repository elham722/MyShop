using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Identity.Models;

namespace MyShop.Identity.Configurations;

/// <summary>
/// Entity Framework configuration for UserToken entity
/// </summary>
public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        // Primary Key (inherited from IdentityUserToken<string>)
        builder.HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });

        // Properties Configuration
        builder.Property(ut => ut.UserId)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Foreign key to the user");

        builder.Property(ut => ut.LoginProvider)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("The login provider (e.g., Local, Google, Facebook)");

        builder.Property(ut => ut.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("The name of the token (e.g., AccessToken, RefreshToken)");

        builder.Property(ut => ut.Value)
            .IsRequired()
            .HasMaxLength(2000)
            .HasComment("The token value");

        builder.Property(ut => ut.TokenType)
            .HasMaxLength(50)
            .HasComment("The type of token (e.g., Bearer, JWT, OAuth)");

        builder.Property(ut => ut.TokenPurpose)
            .HasMaxLength(50)
            .HasComment("The purpose of the token (e.g., Access, Refresh, Authentication)");

        builder.Property(ut => ut.DeviceInfo)
            .HasMaxLength(200)
            .HasComment("Device information (OS, model, etc.)");

        builder.Property(ut => ut.IpAddress)
            .HasMaxLength(45) // IPv6 max length
            .HasComment("IP address of the token creation");

        builder.Property(ut => ut.UserAgent)
            .HasMaxLength(500)
            .HasComment("User agent string from the browser");

        builder.Property(ut => ut.ParentTokenId)
            .HasMaxLength(100)
            .HasComment("ID of the parent token (for token rotation)");

        builder.Property(ut => ut.RevocationReason)
            .HasMaxLength(500)
            .HasComment("Reason for revoking the token");

        builder.Property(ut => ut.UsageCount)
            .IsRequired()
            .HasDefaultValue(0)
            .HasComment("Number of times this token has been used");

        builder.Property(ut => ut.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasComment("Whether this token is currently active");

        builder.Property(ut => ut.IsRevoked)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Whether this token has been revoked");

        builder.Property(ut => ut.IsRotated)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Whether this token has been rotated");

        builder.Property(ut => ut.ExpiresAt)
            .HasComment("When this token expires (null = never expires)");

        builder.Property(ut => ut.LastUsedAt)
            .HasComment("When this token was last used");

        builder.Property(ut => ut.RevokedAt)
            .HasComment("When this token was revoked");

        builder.Property(ut => ut.RotatedAt)
            .HasComment("When this token was rotated");

        builder.Property(ut => ut.CreatedAt)
            .IsRequired()
            .HasComment("When this token was created");

        builder.Property(ut => ut.UpdatedAt)
            .HasComment("When this token was last updated");

        builder.Property(ut => ut.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Who created this token");

        builder.Property(ut => ut.UpdatedBy)
            .HasMaxLength(100)
            .HasComment("Who last updated this token");

        builder.Property(ut => ut.RevokedBy)
            .HasMaxLength(100)
            .HasComment("Who revoked this token");

        builder.Property(ut => ut.RotatedBy)
            .HasMaxLength(100)
            .HasComment("Who rotated this token");

        // Indexes
        builder.HasIndex(ut => ut.UserId)
            .HasDatabaseName("IX_UserToken_UserId");

        builder.HasIndex(ut => ut.LoginProvider)
            .HasDatabaseName("IX_UserToken_LoginProvider");

        builder.HasIndex(ut => ut.Name)
            .HasDatabaseName("IX_UserToken_Name");

        builder.HasIndex(ut => ut.TokenType)
            .HasDatabaseName("IX_UserToken_TokenType");

        builder.HasIndex(ut => ut.TokenPurpose)
            .HasDatabaseName("IX_UserToken_TokenPurpose");

        builder.HasIndex(ut => ut.IsActive)
            .HasDatabaseName("IX_UserToken_IsActive");

        builder.HasIndex(ut => ut.IsRevoked)
            .HasDatabaseName("IX_UserToken_IsRevoked");

        builder.HasIndex(ut => ut.IsRotated)
            .HasDatabaseName("IX_UserToken_IsRotated");

        builder.HasIndex(ut => ut.ExpiresAt)
            .HasDatabaseName("IX_UserToken_ExpiresAt");

        builder.HasIndex(ut => ut.LastUsedAt)
            .HasDatabaseName("IX_UserToken_LastUsedAt");

        builder.HasIndex(ut => ut.CreatedAt)
            .HasDatabaseName("IX_UserToken_CreatedAt");

        builder.HasIndex(ut => ut.IpAddress)
            .HasDatabaseName("IX_UserToken_IpAddress");

        builder.HasIndex(ut => ut.ParentTokenId)
            .HasDatabaseName("IX_UserToken_ParentTokenId");

        builder.HasIndex(ut => new { ut.UserId, ut.IsActive })
            .HasDatabaseName("IX_UserToken_UserId_IsActive");

        builder.HasIndex(ut => new { ut.UserId, ut.TokenPurpose })
            .HasDatabaseName("IX_UserToken_UserId_TokenPurpose");

        builder.HasIndex(ut => new { ut.UserId, ut.IsRevoked })
            .HasDatabaseName("IX_UserToken_UserId_IsRevoked");

        builder.HasIndex(ut => new { ut.UserId, ut.ExpiresAt })
            .HasDatabaseName("IX_UserToken_UserId_ExpiresAt");

        builder.HasIndex(ut => new { ut.UserId, ut.LastUsedAt })
            .HasDatabaseName("IX_UserToken_UserId_LastUsedAt");

        // Table Configuration
        builder.ToTable("UserTokens", "Identity")
            .HasComment("User tokens for refresh token management, multi-token scenarios, and token revocation");

        // Seed Data - Commented out for now to avoid design-time issues
        // builder.HasData(GetSeedData());
    }

    private static IEnumerable<UserToken> GetSeedData()
    {
        var userTokens = new List<UserToken>();

        // SuperAdmin tokens
        userTokens.Add(UserToken.Create(
            "superadmin-user-id",
            "Local",
            "AccessToken",
            "superadmin-access-token-value",
            DateTime.UtcNow.AddHours(1), // Expires in 1 hour
            "Windows 11 Desktop",
            "192.168.1.100",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            "Bearer",
            "Access",
            null,
            "System"));

        userTokens.Add(UserToken.Create(
            "superadmin-user-id",
            "Local",
            "RefreshToken",
            "superadmin-refresh-token-value",
            DateTime.UtcNow.AddDays(30), // Expires in 30 days
            "Windows 11 Desktop",
            "192.168.1.100",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            "Bearer",
            "Refresh",
            null,
            "System"));

        // Admin tokens
        userTokens.Add(UserToken.Create(
            "admin-user-id",
            "Local",
            "AccessToken",
            "admin-access-token-value",
            DateTime.UtcNow.AddHours(1), // Expires in 1 hour
            "MacBook Pro",
            "192.168.1.102",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36",
            "Bearer",
            "Access",
            null,
            "System"));

        userTokens.Add(UserToken.Create(
            "admin-user-id",
            "Local",
            "RefreshToken",
            "admin-refresh-token-value",
            DateTime.UtcNow.AddDays(30), // Expires in 30 days
            "MacBook Pro",
            "192.168.1.102",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36",
            "Bearer",
            "Refresh",
            null,
            "System"));

        // Manager tokens
        userTokens.Add(UserToken.Create(
            "manager-user-id",
            "Local",
            "AccessToken",
            "manager-access-token-value",
            DateTime.UtcNow.AddHours(1), // Expires in 1 hour
            "Dell Laptop",
            "192.168.1.104",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            "Bearer",
            "Access",
            null,
            "System"));

        userTokens.Add(UserToken.Create(
            "manager-user-id",
            "Local",
            "RefreshToken",
            "manager-refresh-token-value",
            DateTime.UtcNow.AddDays(30), // Expires in 30 days
            "Dell Laptop",
            "192.168.1.104",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            "Bearer",
            "Refresh",
            null,
            "System"));

        // CustomerService tokens
        userTokens.Add(UserToken.Create(
            "customerservice-user-id",
            "Local",
            "AccessToken",
            "customerservice-access-token-value",
            DateTime.UtcNow.AddHours(1), // Expires in 1 hour
            "HP Desktop",
            "192.168.1.106",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            "Bearer",
            "Access",
            null,
            "System"));

        userTokens.Add(UserToken.Create(
            "customerservice-user-id",
            "Local",
            "RefreshToken",
            "customerservice-refresh-token-value",
            DateTime.UtcNow.AddDays(30), // Expires in 30 days
            "HP Desktop",
            "192.168.1.106",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            "Bearer",
            "Refresh",
            null,
            "System"));

        // SalesRep tokens
        userTokens.Add(UserToken.Create(
            "salesrep-user-id",
            "Local",
            "AccessToken",
            "salesrep-access-token-value",
            DateTime.UtcNow.AddHours(1), // Expires in 1 hour
            "iPad Pro",
            "192.168.1.107",
            "Mozilla/5.0 (iPad; CPU OS 16_0 like Mac OS X) AppleWebKit/537.36",
            "Bearer",
            "Access",
            null,
            "System"));

        userTokens.Add(UserToken.Create(
            "salesrep-user-id",
            "Local",
            "RefreshToken",
            "salesrep-refresh-token-value",
            DateTime.UtcNow.AddDays(30), // Expires in 30 days
            "iPad Pro",
            "192.168.1.107",
            "Mozilla/5.0 (iPad; CPU OS 16_0 like Mac OS X) AppleWebKit/537.36",
            "Bearer",
            "Refresh",
            null,
            "System"));

        // SupportAgent tokens
        userTokens.Add(UserToken.Create(
            "supportagent-user-id",
            "Local",
            "AccessToken",
            "supportagent-access-token-value",
            DateTime.UtcNow.AddHours(1), // Expires in 1 hour
            "Lenovo ThinkPad",
            "192.168.1.108",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            "Bearer",
            "Access",
            null,
            "System"));

        userTokens.Add(UserToken.Create(
            "supportagent-user-id",
            "Local",
            "RefreshToken",
            "supportagent-refresh-token-value",
            DateTime.UtcNow.AddDays(30), // Expires in 30 days
            "Lenovo ThinkPad",
            "192.168.1.108",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            "Bearer",
            "Refresh",
            null,
            "System"));

        // Customer tokens
        userTokens.Add(UserToken.Create(
            "customer-user-id",
            "Local",
            "AccessToken",
            "customer-access-token-value",
            DateTime.UtcNow.AddHours(1), // Expires in 1 hour
            "iPhone 13",
            "192.168.1.109",
            "Mozilla/5.0 (iPhone; CPU iPhone OS 15_0 like Mac OS X)",
            "Bearer",
            "Access",
            null,
            "System"));

        userTokens.Add(UserToken.Create(
            "customer-user-id",
            "Local",
            "RefreshToken",
            "customer-refresh-token-value",
            DateTime.UtcNow.AddDays(30), // Expires in 30 days
            "iPhone 13",
            "192.168.1.109",
            "Mozilla/5.0 (iPhone; CPU iPhone OS 15_0 like Mac OS X)",
            "Bearer",
            "Refresh",
            null,
            "System"));

        // Google OAuth tokens
        userTokens.Add(UserToken.Create(
            "customer-user-id",
            "Google",
            "AccessToken",
            "google-access-token-value",
            DateTime.UtcNow.AddHours(1), // Expires in 1 hour
            "Chrome Browser",
            "192.168.1.110",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            "Bearer",
            "Access",
            null,
            "System"));

        userTokens.Add(UserToken.Create(
            "customer-user-id",
            "Google",
            "RefreshToken",
            "google-refresh-token-value",
            DateTime.UtcNow.AddDays(30), // Expires in 30 days
            "Chrome Browser",
            "192.168.1.110",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            "Bearer",
            "Refresh",
            null,
            "System"));

        // Expired token example
        userTokens.Add(UserToken.Create(
            "auditor-user-id",
            "Local",
            "AccessToken",
            "expired-access-token-value",
            DateTime.UtcNow.AddDays(-1), // Expired 1 day ago
            "HP Desktop",
            "192.168.1.111",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            "Bearer",
            "Access",
            null,
            "System"));

        // Revoked token example
        var revokedToken = UserToken.Create(
            "reportviewer-user-id",
            "Local",
            "AccessToken",
            "revoked-access-token-value",
            DateTime.UtcNow.AddHours(1), // Expires in 1 hour
            "HP Desktop",
            "192.168.1.112",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            "Bearer",
            "Access",
            null,
            "System");
        
        revokedToken.Revoke("Admin", "Security breach detected");
        userTokens.Add(revokedToken);

        return userTokens;
    }
}