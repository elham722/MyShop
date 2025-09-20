using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Identity.Models;

namespace MyShop.Identity.Configurations;

/// <summary>
/// Entity Framework configuration for UserRole entity
/// </summary>
public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        // Primary Key (inherited from IdentityUserRole<string>)
        builder.HasKey(ur => new { ur.UserId, ur.RoleId });

        // Properties Configuration
        builder.Property(ur => ur.UserId)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Foreign key to the user");

        builder.Property(ur => ur.RoleId)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Foreign key to the role");

        builder.Property(ur => ur.AssignmentReason)
            .HasMaxLength(500)
            .HasComment("Reason for assigning this role to the user");

        builder.Property(ur => ur.AssignmentCategory)
            .HasMaxLength(50)
            .HasComment("Category of the assignment (Standard, Temporary, Emergency, etc.)");

        builder.Property(ur => ur.Priority)
            .IsRequired()
            .HasDefaultValue(5)
            .HasComment("Priority level for sorting (1=highest, 10=lowest)");

        builder.Property(ur => ur.IsTemporary)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Whether this is a temporary role assignment");

        builder.Property(ur => ur.Notes)
            .HasMaxLength(1000)
            .HasComment("Additional notes about this role assignment");

        builder.Property(ur => ur.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasComment("Whether this role assignment is currently active");

        builder.Property(ur => ur.AssignedAt)
            .IsRequired()
            .HasComment("When this role was assigned to the user");

        builder.Property(ur => ur.ExpiresAt)
            .HasComment("When this role assignment expires (null = never expires)");

        builder.Property(ur => ur.AssignedBy)
            .HasMaxLength(100)
            .HasComment("Who assigned this role to the user");

        builder.Property(ur => ur.CreatedAt)
            .IsRequired()
            .HasComment("When this role assignment was created");

        builder.Property(ur => ur.UpdatedAt)
            .HasComment("When this role assignment was last updated");

        builder.Property(ur => ur.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Who created this role assignment");

        builder.Property(ur => ur.UpdatedBy)
            .HasMaxLength(100)
            .HasComment("Who last updated this role assignment");

        // Indexes
        builder.HasIndex(ur => ur.UserId)
            .HasDatabaseName("IX_UserRole_UserId");

        builder.HasIndex(ur => ur.RoleId)
            .HasDatabaseName("IX_UserRole_RoleId");

        builder.HasIndex(ur => ur.IsActive)
            .HasDatabaseName("IX_UserRole_IsActive");

        builder.HasIndex(ur => ur.IsTemporary)
            .HasDatabaseName("IX_UserRole_IsTemporary");

        builder.HasIndex(ur => ur.Priority)
            .HasDatabaseName("IX_UserRole_Priority");

        builder.HasIndex(ur => ur.AssignmentCategory)
            .HasDatabaseName("IX_UserRole_AssignmentCategory");

        builder.HasIndex(ur => ur.AssignedAt)
            .HasDatabaseName("IX_UserRole_AssignedAt");

        builder.HasIndex(ur => ur.ExpiresAt)
            .HasDatabaseName("IX_UserRole_ExpiresAt");

        builder.HasIndex(ur => ur.AssignedBy)
            .HasDatabaseName("IX_UserRole_AssignedBy");

        builder.HasIndex(ur => ur.CreatedAt)
            .HasDatabaseName("IX_UserRole_CreatedAt");

        builder.HasIndex(ur => new { ur.UserId, ur.IsActive })
            .HasDatabaseName("IX_UserRole_UserId_IsActive");

        builder.HasIndex(ur => new { ur.RoleId, ur.IsActive })
            .HasDatabaseName("IX_UserRole_RoleId_IsActive");

        builder.HasIndex(ur => new { ur.UserId, ur.IsTemporary })
            .HasDatabaseName("IX_UserRole_UserId_IsTemporary");

        builder.HasIndex(ur => new { ur.UserId, ur.Priority })
            .HasDatabaseName("IX_UserRole_UserId_Priority");

        builder.HasIndex(ur => new { ur.UserId, ur.ExpiresAt })
            .HasDatabaseName("IX_UserRole_UserId_ExpiresAt");

        // Table Configuration
        builder.ToTable("UserRoles", "Identity")
            .HasComment("User role assignments with expiration, priority, and audit tracking");

        // Seed Data - Commented out for now to avoid design-time issues
        // builder.HasData(GetSeedData());
    }

    private static IEnumerable<UserRole> GetSeedData()
    {
        var userRoles = new List<UserRole>();

        // SuperAdmin role assignments
        userRoles.Add(UserRole.Create(
            "superadmin-user-id",
            "superadmin-role-id",
            "System",
            "Initial system setup",
            null, // Never expires
            "System",
            1, // Highest priority
            false, // Not temporary
            "Super administrator with full system access",
            "System"));

        // Admin role assignments
        userRoles.Add(UserRole.Create(
            "admin-user-id",
            "admin-role-id",
            "SuperAdmin",
            "Promoted to administrator",
            null, // Never expires
            "Administrative",
            2, // High priority
            false, // Not temporary
            "Administrator with management privileges",
            "System"));

        // Manager role assignments
        userRoles.Add(UserRole.Create(
            "manager-user-id",
            "manager-role-id",
            "Admin",
            "Promoted to manager",
            null, // Never expires
            "Administrative",
            3, // Medium-high priority
            false, // Not temporary
            "Manager with supervisory privileges",
            "System"));

        // CustomerService role assignments
        userRoles.Add(UserRole.Create(
            "customerservice-user-id",
            "customerservice-role-id",
            "Manager",
            "Hired for customer service",
            null, // Never expires
            "Business",
            4, // Medium priority
            false, // Not temporary
            "Customer service representative",
            "System"));

        // SalesRep role assignments
        userRoles.Add(UserRole.Create(
            "salesrep-user-id",
            "salesrep-role-id",
            "Manager",
            "Hired for sales",
            null, // Never expires
            "Business",
            5, // Medium priority
            false, // Not temporary
            "Sales representative",
            "System"));

        // SupportAgent role assignments
        userRoles.Add(UserRole.Create(
            "supportagent-user-id",
            "supportagent-role-id",
            "Manager",
            "Hired for technical support",
            null, // Never expires
            "Business",
            5, // Medium priority
            false, // Not temporary
            "Technical support agent",
            "System"));

        // Customer role assignments
        userRoles.Add(UserRole.Create(
            "customer-user-id",
            "customer-role-id",
            "System",
            "Customer registration",
            null, // Never expires
            "User",
            8, // Low priority
            false, // Not temporary
            "Regular customer account",
            "System"));

        // Temporary role assignments examples
        userRoles.Add(UserRole.Create(
            "salesrep-user-id",
            "manager-role-id",
            "Admin",
            "Temporary manager coverage",
            DateTime.UtcNow.AddDays(30), // Expires in 30 days
            "Temporary",
            2, // High priority
            true, // Temporary
            "Temporary manager role for coverage",
            "System"));

        userRoles.Add(UserRole.Create(
            "supportagent-user-id",
            "admin-role-id",
            "SuperAdmin",
            "Emergency admin access",
            DateTime.UtcNow.AddDays(7), // Expires in 7 days
            "Emergency",
            1, // Highest priority
            true, // Temporary
            "Emergency admin access for system maintenance",
            "System"));

        // Audit role assignments
        userRoles.Add(UserRole.Create(
            "auditor-user-id",
            "auditor-role-id",
            "SuperAdmin",
            "Audit team member",
            null, // Never expires
            "Specialized",
            3, // Medium-high priority
            false, // Not temporary
            "Internal auditor with read-only access",
            "System"));

        // ReportViewer role assignments
        userRoles.Add(UserRole.Create(
            "reportviewer-user-id",
            "reportviewer-role-id",
            "Manager",
            "Report viewing access",
            null, // Never expires
            "Specialized",
            6, // Medium-low priority
            false, // Not temporary
            "Report viewer with limited access",
            "System"));

        return userRoles;
    }
}