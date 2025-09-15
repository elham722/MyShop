using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Contracts.Enums.Identity;
using MyShop.Identity.Models;

namespace MyShop.Identity.Configurations;

/// <summary>
/// Entity Framework configuration for Permission entity
/// </summary>
public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        // Primary Key
        builder.HasKey(p => p.Id);

        // Properties Configuration
        builder.Property(p => p.Id)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Unique identifier for the permission");

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Human-readable name of the permission");

        builder.Property(p => p.Resource)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50)
            .HasComment("The resource this permission applies to");

        builder.Property(p => p.Action)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50)
            .HasComment("The action this permission allows");

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(500)
            .HasComment("Detailed description of what this permission allows");

        builder.Property(p => p.Category)
            .HasMaxLength(50)
            .HasComment("Category for grouping permissions");

        builder.Property(p => p.Priority)
            .IsRequired()
            .HasDefaultValue(5)
            .HasComment("Priority level (1=highest, 10=lowest)");

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasComment("Whether this permission is currently active");

        builder.Property(p => p.IsSystemPermission)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Whether this is a system permission that cannot be modified");

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasComment("When this permission was created");

        builder.Property(p => p.UpdatedAt)
            .HasComment("When this permission was last updated");

        builder.Property(p => p.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Who created this permission");

        builder.Property(p => p.UpdatedBy)
            .HasMaxLength(100)
            .HasComment("Who last updated this permission");

        // Indexes
        builder.HasIndex(p => p.Name)
            .IsUnique()
            .HasDatabaseName("IX_Permission_Name_Unique")
            .HasFilter("[IsActive] = 1");

        builder.HasIndex(p => new { p.Resource, p.Action })
            .IsUnique()
            .HasDatabaseName("IX_Permission_Resource_Action_Unique")
            .HasFilter("[IsActive] = 1");

        builder.HasIndex(p => p.Category)
            .HasDatabaseName("IX_Permission_Category");

        builder.HasIndex(p => p.IsActive)
            .HasDatabaseName("IX_Permission_IsActive");

        builder.HasIndex(p => p.IsSystemPermission)
            .HasDatabaseName("IX_Permission_IsSystemPermission");

        builder.HasIndex(p => p.CreatedAt)
            .HasDatabaseName("IX_Permission_CreatedAt");

        // Table Configuration
        builder.ToTable("Permissions", "Identity")
            .HasComment("System permissions for fine-grained access control");

        // Seed Data
        builder.HasData(GetSeedData());
    }

    private static IEnumerable<Permission> GetSeedData()
    {
        var permissions = new List<Permission>();

        // System Permissions
        permissions.Add(Permission.Create(
            "System.Configure", 
            Resource.System, 
            ActionEnum.Configure, 
            "Configure system settings", 
            "System", 
            1, 
            true, 
            "System"));

        permissions.Add(Permission.Create(
            "System.Monitor", 
            Resource.System, 
            ActionEnum.Monitor, 
            "Monitor system health", 
            "System", 
            1, 
            true, 
            "System"));

        permissions.Add(Permission.Create(
            "System.Backup", 
            Resource.System, 
            ActionEnum.Backup, 
            "Create system backups", 
            "System", 
            1, 
            true, 
            "System"));

        permissions.Add(Permission.Create(
            "System.Migrate", 
            Resource.System, 
            ActionEnum.Migrate, 
            "Migrate system data", 
            "System", 
            1, 
            true, 
            "System"));

        // User Management Permissions
        permissions.Add(Permission.Create(
            "User.Create", 
            Resource.User, 
            ActionEnum.Create, 
            "Create new users", 
            "User Management", 
            2, 
            false, 
            "System"));

        permissions.Add(Permission.Create(
            "User.Read", 
            Resource.User, 
            ActionEnum.Read, 
            "View user information", 
            "User Management", 
            2, 
            false, 
            "System"));

        permissions.Add(Permission.Create(
            "User.Update", 
            Resource.User, 
            ActionEnum.Update, 
            "Update user information", 
            "User Management", 
            2, 
            false, 
            "System"));

        permissions.Add(Permission.Create(
            "User.Delete", 
            Resource.User, 
            ActionEnum.Delete, 
            "Delete users", 
            "User Management", 
            2, 
            false, 
            "System"));

        permissions.Add(Permission.Create(
            "User.List", 
            Resource.User, 
            ActionEnum.List, 
            "List all users", 
            "User Management", 
            2, 
            false, 
            "System"));

        permissions.Add(Permission.Create(
            "User.Activate", 
            Resource.User, 
            ActionEnum.Activate, 
            "Activate user accounts", 
            "User Management", 
            2, 
            false, 
            "System"));

        permissions.Add(Permission.Create(
            "User.Deactivate", 
            Resource.User, 
            ActionEnum.Deactivate, 
            "Deactivate user accounts", 
            "User Management", 
            2, 
            false, 
            "System"));

        permissions.Add(Permission.Create(
            "User.Suspend", 
            Resource.User, 
            ActionEnum.Suspend, 
            "Suspend user accounts", 
            "User Management", 
            2, 
            false, 
            "System"));

        // Role Management Permissions
        permissions.Add(Permission.Create(
            "Role.Create", 
            Resource.Role, 
            ActionEnum.Create, 
            "Create new roles", 
            "User Management", 
            2, 
            false, 
            "System"));

        permissions.Add(Permission.Create(
            "Role.Read", 
            Resource.Role, 
            ActionEnum.Read, 
            "View role information", 
            "User Management", 
            2, 
            false, 
            "System"));

        permissions.Add(Permission.Create(
            "Role.Update", 
            Resource.Role, 
            ActionEnum.Update, 
            "Update role information", 
            "User Management", 
            2, 
            false, 
            "System"));

        permissions.Add(Permission.Create(
            "Role.Delete", 
            Resource.Role, 
            ActionEnum.Delete, 
            "Delete roles", 
            "User Management", 
            2, 
            false, 
            "System"));

        permissions.Add(Permission.Create(
            "Role.List", 
            Resource.Role, 
            ActionEnum.List, 
            "List all roles", 
            "User Management", 
            2, 
            false, 
            "System"));

        permissions.Add(Permission.Create(
            "Role.Assign", 
            Resource.Role, 
            ActionEnum.Assign, 
            "Assign roles to users", 
            "User Management", 
            2, 
            false, 
            "System"));

        permissions.Add(Permission.Create(
            "Role.Unassign", 
            Resource.Role, 
            ActionEnum.Unassign, 
            "Unassign roles from users", 
            "User Management", 
            2, 
            false, 
            "System"));

        // Permission Management Permissions
        permissions.Add(Permission.Create(
            "Permission.Create", 
            Resource.Permission, 
            ActionEnum.Create, 
            "Create new permissions", 
            "User Management", 
            2, 
            false, 
            "System"));

        permissions.Add(Permission.Create(
            "Permission.Read", 
            Resource.Permission, 
            ActionEnum.Read, 
            "View permission information", 
            "User Management", 
            2, 
            false, 
            "System"));

        permissions.Add(Permission.Create(
            "Permission.Update", 
            Resource.Permission, 
            ActionEnum.Update, 
            "Update permission information", 
            "User Management", 
            2, 
            false, 
            "System"));

        permissions.Add(Permission.Create(
            "Permission.Delete", 
            Resource.Permission, 
            ActionEnum.Delete, 
            "Delete permissions", 
            "User Management", 
            2, 
            false, 
            "System"));

        permissions.Add(Permission.Create(
            "Permission.List", 
            Resource.Permission, 
            ActionEnum.List, 
            "List all permissions", 
            "User Management", 
            2, 
            false, 
            "System"));

        permissions.Add(Permission.Create(
            "Permission.Assign", 
            Resource.Permission, 
            ActionEnum.Assign, 
            "Assign permissions to roles", 
            "User Management", 
            2, 
            false, 
            "System"));

        permissions.Add(Permission.Create(
            "Permission.Unassign", 
            Resource.Permission, 
            ActionEnum.Unassign, 
            "Unassign permissions from roles", 
            "User Management", 
            2, 
            false, 
            "System"));

        // Customer Management Permissions
        permissions.Add(Permission.Create(
            "Customer.Create", 
            Resource.Customer, 
            ActionEnum.Create, 
            "Create new customers", 
            "Business", 
            3, 
            false, 
            "System"));

        permissions.Add(Permission.Create(
            "Customer.Read", 
            Resource.Customer, 
            ActionEnum.Read, 
            "View customer information", 
            "Business", 
            3, 
            false, 
            "System"));

        permissions.Add(Permission.Create(
            "Customer.Update", 
            Resource.Customer, 
            ActionEnum.Update, 
            "Update customer information", 
            "Business", 
            3, 
            false, 
            "System"));

        permissions.Add(Permission.Create(
            "Customer.Delete", 
            Resource.Customer, 
            ActionEnum.Delete, 
            "Delete customers", 
            "Business", 
            3, 
            false, 
            "System"));

        permissions.Add(Permission.Create(
            "Customer.List", 
            Resource.Customer, 
            ActionEnum.List, 
            "List all customers", 
            "Business", 
            3, 
            false, 
            "System"));

        permissions.Add(Permission.Create(
            "Customer.Activate", 
            Resource.Customer, 
            ActionEnum.Activate, 
            "Activate customer accounts", 
            "Business", 
            3, 
            false, 
            "System"));

        permissions.Add(Permission.Create(
            "Customer.Deactivate", 
            Resource.Customer, 
            ActionEnum.Deactivate, 
            "Deactivate customer accounts", 
            "Business", 
            3, 
            false, 
            "System"));

        permissions.Add(Permission.Create(
            "Customer.Suspend", 
            Resource.Customer, 
            ActionEnum.Suspend, 
            "Suspend customer accounts", 
            "Business", 
            3, 
            false, 
            "System"));

        // Audit Permissions
        permissions.Add(Permission.Create(
            "Audit.Read", 
            Resource.Audit, 
            ActionEnum.Read, 
            "View audit logs", 
            "System", 
            1, 
            true, 
            "System"));

        permissions.Add(Permission.Create(
            "Audit.List", 
            Resource.Audit, 
            ActionEnum.List, 
            "List audit logs", 
            "System", 
            1, 
            true, 
            "System"));

        permissions.Add(Permission.Create(
            "Audit.Execute", 
            Resource.Audit, 
            ActionEnum.Execute, 
            "Execute audit operations", 
            "System", 
            1, 
            true, 
            "System"));

        return permissions;
    }
}