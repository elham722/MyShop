using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Identity.Models;

namespace MyShop.Identity.Configurations;

/// <summary>
/// Entity Framework configuration for RolePermission entity
/// </summary>
public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        // Primary Key
        builder.HasKey(rp => rp.Id);

        // Properties Configuration
        builder.Property(rp => rp.Id)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Unique identifier for the role-permission assignment");

        builder.Property(rp => rp.RoleId)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Foreign key to the role");

        builder.Property(rp => rp.PermissionId)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Foreign key to the permission");

        builder.Property(rp => rp.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasComment("Whether this assignment is currently active");

        builder.Property(rp => rp.IsGranted)
            .IsRequired()
            .HasDefaultValue(true)
            .HasComment("Whether this permission is granted (true) or denied (false)");

        builder.Property(rp => rp.AssignedAt)
            .IsRequired()
            .HasComment("When this permission was assigned to the role");

        builder.Property(rp => rp.ExpiresAt)
            .HasComment("When this assignment expires (null = never expires)");

        builder.Property(rp => rp.AssignedBy)
            .HasMaxLength(100)
            .HasComment("Who assigned this permission to the role");

        builder.Property(rp => rp.CreatedAt)
            .IsRequired()
            .HasComment("When this assignment was created");

        builder.Property(rp => rp.UpdatedAt)
            .HasComment("When this assignment was last updated");

        builder.Property(rp => rp.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Who created this assignment");

        builder.Property(rp => rp.UpdatedBy)
            .HasMaxLength(100)
            .HasComment("Who last updated this assignment");

        // Foreign Key Relationships
        builder.HasOne(rp => rp.Role)
            .WithMany()
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_RolePermission_Role");

        builder.HasOne(rp => rp.Permission)
            .WithMany()
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_RolePermission_Permission");

        // Indexes
        builder.HasIndex(rp => new { rp.RoleId, rp.PermissionId })
            .IsUnique()
            .HasDatabaseName("IX_RolePermission_RoleId_PermissionId_Unique")
            .HasFilter("[IsActive] = 1") // Only unique among active assignments
            .HasComment("Unique index on role-permission combination for active assignments");

        builder.HasIndex(rp => rp.RoleId)
            .HasDatabaseName("IX_RolePermission_RoleId")
            .HasComment("Index for filtering by role");

        builder.HasIndex(rp => rp.PermissionId)
            .HasDatabaseName("IX_RolePermission_PermissionId")
            .HasComment("Index for filtering by permission");

        builder.HasIndex(rp => rp.IsActive)
            .HasDatabaseName("IX_RolePermission_IsActive")
            .HasComment("Index for filtering active/inactive assignments");

        builder.HasIndex(rp => rp.IsGranted)
            .HasDatabaseName("IX_RolePermission_IsGranted")
            .HasComment("Index for filtering granted/denied permissions");

        builder.HasIndex(rp => rp.ExpiresAt)
            .HasDatabaseName("IX_RolePermission_ExpiresAt")
            .HasComment("Index for filtering by expiration date");

        builder.HasIndex(rp => rp.AssignedAt)
            .HasDatabaseName("IX_RolePermission_AssignedAt")
            .HasComment("Index for sorting by assignment date");

        builder.HasIndex(rp => rp.AssignedBy)
            .HasDatabaseName("IX_RolePermission_AssignedBy")
            .HasComment("Index for filtering by assigner");

        // Table Configuration
        builder.ToTable("RolePermissions", "Identity")
            .HasComment("Many-to-many relationship between roles and permissions");

        // Seed Data
        builder.HasData(GetSeedData());
    }

    private static IEnumerable<RolePermission> GetSeedData()
    {
        var rolePermissions = new List<RolePermission>();

        // SuperAdmin gets all permissions
        var superAdminRoleId = "superadmin-role-id"; // This would be the actual SuperAdmin role ID
        var allPermissionIds = new[]
        {
            "system-configure-permission-id",
            "system-monitor-permission-id",
            "system-backup-permission-id",
            "system-migrate-permission-id",
            "user-create-permission-id",
            "user-read-permission-id",
            "user-update-permission-id",
            "user-delete-permission-id",
            "user-list-permission-id",
            "user-activate-permission-id",
            "user-deactivate-permission-id",
            "user-suspend-permission-id",
            "role-create-permission-id",
            "role-read-permission-id",
            "role-update-permission-id",
            "role-delete-permission-id",
            "role-list-permission-id",
            "role-assign-permission-id",
            "role-unassign-permission-id",
            "permission-create-permission-id",
            "permission-read-permission-id",
            "permission-update-permission-id",
            "permission-delete-permission-id",
            "permission-list-permission-id",
            "permission-assign-permission-id",
            "permission-unassign-permission-id",
            "customer-create-permission-id",
            "customer-read-permission-id",
            "customer-update-permission-id",
            "customer-delete-permission-id",
            "customer-list-permission-id",
            "customer-activate-permission-id",
            "customer-deactivate-permission-id",
            "customer-suspend-permission-id",
            "audit-read-permission-id",
            "audit-list-permission-id",
            "audit-execute-permission-id"
        };

        foreach (var permissionId in allPermissionIds)
        {
            rolePermissions.Add(RolePermission.Create(
                superAdminRoleId,
                permissionId,
                "System",
                null,
                true,
                "System"));
        }

        // SystemAdmin gets most permissions except some system-level ones
        var systemAdminRoleId = "systemadmin-role-id";
        var systemAdminPermissionIds = allPermissionIds.Where(p => 
            !p.Contains("system-configure") && 
            !p.Contains("system-migrate")).ToArray();

        foreach (var permissionId in systemAdminPermissionIds)
        {
            rolePermissions.Add(RolePermission.Create(
                systemAdminRoleId,
                permissionId,
                "System",
                null,
                true,
                "System"));
        }

        // Admin gets user management and business permissions
        var adminRoleId = "admin-role-id";
        var adminPermissionIds = allPermissionIds.Where(p => 
            p.Contains("user-") || 
            p.Contains("role-") || 
            p.Contains("permission-") || 
            p.Contains("customer-")).ToArray();

        foreach (var permissionId in adminPermissionIds)
        {
            rolePermissions.Add(RolePermission.Create(
                adminRoleId,
                permissionId,
                "System",
                null,
                true,
                "System"));
        }

        // Manager gets business permissions
        var managerRoleId = "manager-role-id";
        var managerPermissionIds = allPermissionIds.Where(p => 
            p.Contains("customer-") && 
            !p.Contains("customer-delete")).ToArray();

        foreach (var permissionId in managerPermissionIds)
        {
            rolePermissions.Add(RolePermission.Create(
                managerRoleId,
                permissionId,
                "System",
                null,
                true,
                "System"));
        }

        // CustomerService gets customer read and update permissions
        var customerServiceRoleId = "customerservice-role-id";
        var customerServicePermissionIds = new[]
        {
            "customer-read-permission-id",
            "customer-update-permission-id",
            "customer-list-permission-id",
            "customer-activate-permission-id",
            "customer-deactivate-permission-id"
        };

        foreach (var permissionId in customerServicePermissionIds)
        {
            rolePermissions.Add(RolePermission.Create(
                customerServiceRoleId,
                permissionId,
                "System",
                null,
                true,
                "System"));
        }

        // SalesRep gets customer and order permissions
        var salesRepRoleId = "salesrep-role-id";
        var salesRepPermissionIds = new[]
        {
            "customer-read-permission-id",
            "customer-create-permission-id",
            "customer-update-permission-id",
            "customer-list-permission-id"
        };

        foreach (var permissionId in salesRepPermissionIds)
        {
            rolePermissions.Add(RolePermission.Create(
                salesRepRoleId,
                permissionId,
                "System",
                null,
                true,
                "System"));
        }

        // SupportAgent gets customer read permissions
        var supportAgentRoleId = "supportagent-role-id";
        var supportAgentPermissionIds = new[]
        {
            "customer-read-permission-id",
            "customer-list-permission-id"
        };

        foreach (var permissionId in supportAgentPermissionIds)
        {
            rolePermissions.Add(RolePermission.Create(
                supportAgentRoleId,
                permissionId,
                "System",
                null,
                true,
                "System"));
        }

        // Auditor gets audit permissions
        var auditorRoleId = "auditor-role-id";
        var auditorPermissionIds = new[]
        {
            "audit-read-permission-id",
            "audit-list-permission-id"
        };

        foreach (var permissionId in auditorPermissionIds)
        {
            rolePermissions.Add(RolePermission.Create(
                auditorRoleId,
                permissionId,
                "System",
                null,
                true,
                "System"));
        }

        // ReportViewer gets report permissions
        var reportViewerRoleId = "reportviewer-role-id";
        var reportViewerPermissionIds = new[]
        {
            "audit-read-permission-id",
            "audit-list-permission-id"
        };

        foreach (var permissionId in reportViewerPermissionIds)
        {
            rolePermissions.Add(RolePermission.Create(
                reportViewerRoleId,
                permissionId,
                "System",
                null,
                true,
                "System"));
        }

        // Customer gets basic permissions
        var customerRoleId = "customer-role-id";
        var customerPermissionIds = new[]
        {
            "customer-read-permission-id" // Customers can only read their own data
        };

        foreach (var permissionId in customerPermissionIds)
        {
            rolePermissions.Add(RolePermission.Create(
                customerRoleId,
                permissionId,
                "System",
                null,
                true,
                "System"));
        }

        return rolePermissions;
    }
}