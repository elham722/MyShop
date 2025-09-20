using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Identity.Models;

namespace MyShop.Identity.Configurations;

/// <summary>
/// Entity Framework configuration for UserClaim entity
/// </summary>
public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
{
    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        // Primary Key (inherited from IdentityUserClaim<string>)
        builder.HasKey(uc => uc.Id);

        // Properties Configuration
        builder.Property(uc => uc.Id)
            .IsRequired()
            .HasComment("Unique identifier for the user claim");

        builder.Property(uc => uc.UserId)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Foreign key to the user");

        builder.Property(uc => uc.ClaimType)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("The type of the claim");

        builder.Property(uc => uc.ClaimValue)
            .IsRequired()
            .HasMaxLength(500)
            .HasComment("The value of the claim");

        builder.Property(uc => uc.Category)
            .HasMaxLength(50)
            .HasComment("Category for grouping claims (Business, System, etc.)");

        builder.Property(uc => uc.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasComment("Whether this claim is currently active");

        builder.Property(uc => uc.ExpiresAt)
            .HasComment("When this claim expires (null = never expires)");

        builder.Property(uc => uc.CreatedAt)
            .IsRequired()
            .HasComment("When this claim was created");

        builder.Property(uc => uc.UpdatedAt)
            .HasComment("When this claim was last updated");

        builder.Property(uc => uc.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Who created this claim");

        builder.Property(uc => uc.UpdatedBy)
            .HasMaxLength(100)
            .HasComment("Who last updated this claim");

        // Indexes
        builder.HasIndex(uc => uc.UserId)
            .HasDatabaseName("IX_UserClaim_UserId");

        builder.HasIndex(uc => uc.ClaimType)
            .HasDatabaseName("IX_UserClaim_ClaimType");

        builder.HasIndex(uc => uc.Category)
            .HasDatabaseName("IX_UserClaim_Category");

        builder.HasIndex(uc => uc.IsActive)
            .HasDatabaseName("IX_UserClaim_IsActive");

        builder.HasIndex(uc => uc.ExpiresAt)
            .HasDatabaseName("IX_UserClaim_ExpiresAt");

        builder.HasIndex(uc => uc.CreatedAt)
            .HasDatabaseName("IX_UserClaim_CreatedAt");

        builder.HasIndex(uc => new { uc.UserId, uc.ClaimType })
            .HasDatabaseName("IX_UserClaim_UserId_ClaimType")
            .HasFilter("[IsActive] = 1"); // Only among active claims;;

        // Table Configuration
        builder.ToTable("UserClaims", "Identity")
            .HasComment("Custom user claims for dynamic business claims that need management and expiration");

        // Seed Data - Commented out for now to avoid design-time issues
        // builder.HasData(GetSeedData());
    }

    private static IEnumerable<UserClaim> GetSeedData()
    {
        var userClaims = new List<UserClaim>();

        // Example business claims for different users
        // These are dynamic claims that might change over time

        // SuperAdmin business claims
        userClaims.Add(UserClaim.Create(
            "superadmin-user-id",
            "Department",
            "IT",
            "Business",
            null, // Never expires
            "System"));

        userClaims.Add(UserClaim.Create(
            "superadmin-user-id",
            "Region",
            "Global",
            "Business",
            null, // Never expires
            "System"));

        userClaims.Add(UserClaim.Create(
            "superadmin-user-id",
            "AccessLevel",
            "Full",
            "Business",
            null, // Never expires
            "System"));

        // Admin business claims
        userClaims.Add(UserClaim.Create(
            "admin-user-id",
            "Department",
            "Management",
            "Business",
            null, // Never expires
            "System"));

        userClaims.Add(UserClaim.Create(
            "admin-user-id",
            "Region",
            "North America",
            "Business",
            null, // Never expires
            "System"));

        userClaims.Add(UserClaim.Create(
            "admin-user-id",
            "AccessLevel",
            "High",
            "Business",
            null, // Never expires
            "System"));

        // Manager business claims
        userClaims.Add(UserClaim.Create(
            "manager-user-id",
            "Department",
            "Sales",
            "Business",
            null, // Never expires
            "System"));

        userClaims.Add(UserClaim.Create(
            "manager-user-id",
            "Region",
            "Europe",
            "Business",
            null, // Never expires
            "System"));

        userClaims.Add(UserClaim.Create(
            "manager-user-id",
            "AccessLevel",
            "Medium",
            "Business",
            null, // Never expires
            "System"));

        // CustomerService business claims
        userClaims.Add(UserClaim.Create(
            "customerservice-user-id",
            "Department",
            "Customer Service",
            "Business",
            null, // Never expires
            "System"));

        userClaims.Add(UserClaim.Create(
            "customerservice-user-id",
            "Region",
            "Asia",
            "Business",
            null, // Never expires
            "System"));

        userClaims.Add(UserClaim.Create(
            "customerservice-user-id",
            "AccessLevel",
            "Standard",
            "Business",
            null, // Never expires
            "System"));

        // Temporary claims example
        userClaims.Add(UserClaim.Create(
            "salesrep-user-id",
            "TemporaryAccess",
            "PremiumFeatures",
            "Temporary",
            DateTime.UtcNow.AddDays(30), // Expires in 30 days
            "System"));

        userClaims.Add(UserClaim.Create(
            "supportagent-user-id",
            "TemporaryAccess",
            "AdminPanel",
            "Temporary",
            DateTime.UtcNow.AddDays(7), // Expires in 7 days
            "System"));

        // Customer business claims
        userClaims.Add(UserClaim.Create(
            "customer-user-id",
            "CustomerType",
            "Premium",
            "Business",
            null, // Never expires
            "System"));

        userClaims.Add(UserClaim.Create(
            "customer-user-id",
            "Region",
            "North America",
            "Business",
            null, // Never expires
            "System"));

        userClaims.Add(UserClaim.Create(
            "customer-user-id",
            "AccessLevel",
            "Basic",
            "Business",
            null, // Never expires
            "System"));

        return userClaims;
    }
}