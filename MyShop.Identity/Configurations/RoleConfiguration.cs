using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Identity.Models;

namespace MyShop.Identity.Configurations;

/// <summary>
/// Entity Framework configuration for Role entity
/// </summary>
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Primary Key (inherited from IdentityRole<string>)
        builder.HasKey(r => r.Id);

        // Properties Configuration
        builder.Property(r => r.Id)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Unique identifier for the role");

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Human-readable name of the role");

        builder.Property(r => r.NormalizedName)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Normalized name for case-insensitive lookups");

        builder.Property(r => r.Description)
            .IsRequired()
            .HasMaxLength(500)
            .HasComment("Detailed description of what this role allows");

        builder.Property(r => r.Category)
            .HasMaxLength(50)
            .HasComment("Category for grouping roles");

        builder.Property(r => r.Priority)
            .IsRequired()
            .HasDefaultValue(5)
            .HasComment("Priority level for sorting (1=highest, 10=lowest)");

        builder.Property(r => r.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasComment("Whether this role is currently active");

        builder.Property(r => r.IsSystemRole)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Whether this is a system role that cannot be modified");

        builder.Property(r => r.CreatedAt)
            .IsRequired()
            .HasComment("When this role was created");

        builder.Property(r => r.UpdatedAt)
            .HasComment("When this role was last updated");

        builder.Property(r => r.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Who created this role");

        builder.Property(r => r.UpdatedBy)
            .HasMaxLength(100)
            .HasComment("Who last updated this role");

        // Indexes
        builder.HasIndex(r => r.NormalizedName)
            .IsUnique()
            .HasDatabaseName("IX_Role_NormalizedName_Unique")
            .HasFilter("[IsActive] = 1");

        builder.HasIndex(r => r.Category)
            .HasDatabaseName("IX_Role_Category");

        builder.HasIndex(r => r.IsActive)
            .HasDatabaseName("IX_Role_IsActive");

        builder.HasIndex(r => r.IsSystemRole)
            .HasDatabaseName("IX_Role_IsSystemRole");

        builder.HasIndex(r => r.Priority)
            .HasDatabaseName("IX_Role_Priority");

        builder.HasIndex(r => r.CreatedAt)
            .HasDatabaseName("IX_Role_CreatedAt");

        // Table Configuration
        builder.ToTable("Roles", "Identity")
            .HasComment("User roles for role-based access control");

        // Seed Data
        builder.HasData(GetSeedData());
    }

    private static IEnumerable<Role> GetSeedData()
    {
        var roles = new List<Role>();

        // System Roles
        roles.Add(Role.Create(
            "SuperAdmin",
            "Super Administrator with full system access",
            "System",
            1,
            true,
            "System"));

        roles.Add(Role.Create(
            "SystemAdmin",
            "System Administrator with administrative privileges",
            "System",
            2,
            true,
            "System"));

        // Administrative Roles
        roles.Add(Role.Create(
            "Admin",
            "Administrator with management privileges",
            "Administrative",
            3,
            false,
            "System"));

        roles.Add(Role.Create(
            "Manager",
            "Manager with supervisory privileges",
            "Administrative",
            4,
            false,
            "System"));

        // Business Roles
        roles.Add(Role.Create(
            "CustomerService",
            "Customer service representative",
            "Business",
            5,
            false,
            "System"));

        roles.Add(Role.Create(
            "SalesRep",
            "Sales representative",
            "Business",
            6,
            false,
            "System"));

        roles.Add(Role.Create(
            "SupportAgent",
            "Technical support agent",
            "Business",
            7,
            false,
            "System"));

        // User Roles
        roles.Add(Role.Create(
            "Customer",
            "Regular customer with basic access",
            "User",
            8,
            false,
            "System"));

        roles.Add(Role.Create(
            "Guest",
            "Guest user with limited access",
            "User",
            9,
            false,
            "System"));

        // Specialized Roles
        roles.Add(Role.Create(
            "Auditor",
            "Auditor with read-only access to audit logs",
            "Specialized",
            5,
            false,
            "System"));

        roles.Add(Role.Create(
            "ReportViewer",
            "User with access to reports and analytics",
            "Specialized",
            6,
            false,
            "System"));

        return roles;
    }
}