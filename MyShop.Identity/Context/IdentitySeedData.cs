using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyShop.Identity.Models;
using MyShop.Identity.Constants;
using MyShop.Contracts.Enums.Identity;

namespace MyShop.Identity.Context;

/// <summary>
/// Seed data for Identity system - roles, permissions, and initial data
/// </summary>
public static class IdentitySeedData
{
    /// <summary>
    /// Seeds the database with initial roles and permissions
    /// </summary>
    public static async Task SeedAsync(MyShopIdentityDbContext context, RoleManager<Role> roleManager, UserManager<ApplicationUser> userManager)
    {
        await SeedRolesAsync(context, roleManager);
        await SeedPermissionsAsync(context);
        await SeedRolePermissionsAsync(context);
        await SeedDefaultUsersAsync(context, userManager);
    }

    private static async Task SeedRolesAsync(MyShopIdentityDbContext context, RoleManager<Role> roleManager)
    {
        var roles = new List<Role>
        {
            // System Roles
            Role.Create(RoleConstants.System.SuperAdmin, "Super Administrator with full system access", "System", 1, true, "System"),
            Role.Create(RoleConstants.System.SystemAdmin, "System Administrator with administrative privileges", "System", 2, true, "System"),
            
            // Administrative Roles
            Role.Create(RoleConstants.Administrative.Admin, "Administrator with management privileges", "Administrative", 3, false, "System"),
            Role.Create(RoleConstants.Administrative.Manager, "Manager with team management privileges", "Administrative", 4, false, "System"),
            
            // Business Roles
            Role.Create(RoleConstants.Business.CustomerService, "Customer Service Representative", "Business", 5, false, "System"),
            Role.Create(RoleConstants.Business.SalesRep, "Sales Representative", "Business", 6, false, "System"),
            Role.Create(RoleConstants.Business.SupportAgent, "Technical Support Agent", "Business", 7, false, "System"),
            
            // User Roles
            Role.Create(RoleConstants.User.Customer, "Regular Customer", "User", 8, false, "System"),
            Role.Create(RoleConstants.User.Guest, "Guest User with limited access", "User", 9, false, "System"),
            
            // Specialized Roles
            Role.Create(RoleConstants.Specialized.Auditor, "Auditor with read-only access to audit logs", "Specialized", 5, false, "System"),
            Role.Create(RoleConstants.Specialized.ReportViewer, "Report Viewer with access to reports", "Specialized", 6, false, "System")
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role.Name))
            {
                await roleManager.CreateAsync(role);
            }
        }
    }

    private static async Task SeedPermissionsAsync(MyShopIdentityDbContext context)
    {
        var permissions = new List<Permission>
        {
            // System Permissions
            Permission.Create("System.Configure", Resource.System, ActionEnum.Configure, "Configure system settings", "System", 1, true, "System"),
            Permission.Create("System.Monitor", Resource.System, ActionEnum.Monitor, "Monitor system health and performance", "System", 1, true, "System"),
            Permission.Create("System.Backup", Resource.System, ActionEnum.Backup, "Create and manage system backups", "System", 1, true, "System"),
            Permission.Create("System.Migrate", Resource.System, ActionEnum.Migrate, "Perform system migrations", "System", 1, true, "System"),

            // User Management Permissions
            Permission.Create("User.Create", Resource.User, ActionEnum.Create, "Create new users", "User Management", 2, true, "System"),
            Permission.Create("User.Read", Resource.User, ActionEnum.Read, "View user information", "User Management", 2, true, "System"),
            Permission.Create("User.Update", Resource.User, ActionEnum.Update, "Update user information", "User Management", 2, true, "System"),
            Permission.Create("User.Delete", Resource.User, ActionEnum.Delete, "Delete users", "User Management", 2, true, "System"),
            Permission.Create("User.List", Resource.User, ActionEnum.List, "List all users", "User Management", 2, true, "System"),
            Permission.Create("User.Activate", Resource.User, ActionEnum.Activate, "Activate user accounts", "User Management", 2, true, "System"),
            Permission.Create("User.Deactivate", Resource.User, ActionEnum.Deactivate, "Deactivate user accounts", "User Management", 2, true, "System"),
            Permission.Create("User.Suspend", Resource.User, ActionEnum.Suspend, "Suspend user accounts", "User Management", 2, true, "System"),

            // Role Management Permissions
            Permission.Create("Role.Create", Resource.Role, ActionEnum.Create, "Create new roles", "User Management", 2, true, "System"),
            Permission.Create("Role.Read", Resource.Role, ActionEnum.Read, "View role information", "User Management", 2, true, "System"),
            Permission.Create("Role.Update", Resource.Role, ActionEnum.Update, "Update role information", "User Management", 2, true, "System"),
            Permission.Create("Role.Delete", Resource.Role, ActionEnum.Delete, "Delete roles", "User Management", 2, true, "System"),
            Permission.Create("Role.List", Resource.Role, ActionEnum.List, "List all roles", "User Management", 2, true, "System"),
            Permission.Create("Role.Assign", Resource.Role, ActionEnum.Assign, "Assign roles to users", "User Management", 2, true, "System"),
            Permission.Create("Role.Unassign", Resource.Role, ActionEnum.Unassign, "Remove roles from users", "User Management", 2, true, "System"),

            // Permission Management Permissions
            Permission.Create("Permission.Create", Resource.Permission, ActionEnum.Create, "Create new permissions", "User Management", 2, true, "System"),
            Permission.Create("Permission.Read", Resource.Permission, ActionEnum.Read, "View permission information", "User Management", 2, true, "System"),
            Permission.Create("Permission.Update", Resource.Permission, ActionEnum.Update, "Update permission information", "User Management", 2, true, "System"),
            Permission.Create("Permission.Delete", Resource.Permission, ActionEnum.Delete, "Delete permissions", "User Management", 2, true, "System"),
            Permission.Create("Permission.List", Resource.Permission, ActionEnum.List, "List all permissions", "User Management", 2, true, "System"),
            Permission.Create("Permission.Assign", Resource.Permission, ActionEnum.Assign, "Assign permissions to roles", "User Management", 2, true, "System"),
            Permission.Create("Permission.Unassign", Resource.Permission, ActionEnum.Unassign, "Remove permissions from roles", "User Management", 2, true, "System"),

            // Customer Management Permissions
            Permission.Create("Customer.Create", Resource.Customer, ActionEnum.Create, "Create new customers", "Business", 3, true, "System"),
            Permission.Create("Customer.Read", Resource.Customer, ActionEnum.Read, "View customer information", "Business", 3, true, "System"),
            Permission.Create("Customer.Update", Resource.Customer, ActionEnum.Update, "Update customer information", "Business", 3, true, "System"),
            Permission.Create("Customer.Delete", Resource.Customer, ActionEnum.Delete, "Delete customers", "Business", 3, true, "System"),
            Permission.Create("Customer.List", Resource.Customer, ActionEnum.List, "List all customers", "Business", 3, true, "System"),
            Permission.Create("Customer.Activate", Resource.Customer, ActionEnum.Activate, "Activate customer accounts", "Business", 3, true, "System"),
            Permission.Create("Customer.Deactivate", Resource.Customer, ActionEnum.Deactivate, "Deactivate customer accounts", "Business", 3, true, "System"),
            Permission.Create("Customer.Suspend", Resource.Customer, ActionEnum.Suspend, "Suspend customer accounts", "Business", 3, true, "System"),

            // Audit Permissions
            Permission.Create("Audit.Read", Resource.Audit, ActionEnum.Read, "View audit logs", "System", 1, true, "System"),
            Permission.Create("Audit.List", Resource.Audit, ActionEnum.List, "List audit logs", "System", 1, true, "System"),
            Permission.Create("Audit.Execute", Resource.Audit, ActionEnum.Execute, "Execute audit operations", "System", 1, true, "System"),

            // Order Management Permissions
            Permission.Create("Order.Create", Resource.Order, ActionEnum.Create, "Create new orders", "Business", 3, true, "System"),
            Permission.Create("Order.Read", Resource.Order, ActionEnum.Read, "View order information", "Business", 3, true, "System"),
            Permission.Create("Order.Update", Resource.Order, ActionEnum.Update, "Update order information", "Business", 3, true, "System"),
            Permission.Create("Order.Delete", Resource.Order, ActionEnum.Delete, "Delete orders", "Business", 3, true, "System"),
            Permission.Create("Order.List", Resource.Order, ActionEnum.List, "List all orders", "Business", 3, true, "System"),
            Permission.Create("Order.Approve", Resource.Order, ActionEnum.Approve, "Approve orders", "Business", 3, true, "System"),
            Permission.Create("Order.Reject", Resource.Order, ActionEnum.Reject, "Reject orders", "Business", 3, true, "System"),

            // Product Management Permissions
            Permission.Create("Product.Create", Resource.Product, ActionEnum.Create, "Create new products", "Business", 4, true, "System"),
            Permission.Create("Product.Read", Resource.Product, ActionEnum.Read, "View product information", "Business", 4, true, "System"),
            Permission.Create("Product.Update", Resource.Product, ActionEnum.Update, "Update product information", "Business", 4, true, "System"),
            Permission.Create("Product.Delete", Resource.Product, ActionEnum.Delete, "Delete products", "Business", 4, true, "System"),
            Permission.Create("Product.List", Resource.Product, ActionEnum.List, "List all products", "Business", 4, true, "System"),
            Permission.Create("Product.Publish", Resource.Product, ActionEnum.Publish, "Publish products", "Business", 4, true, "System"),
            Permission.Create("Product.Unpublish", Resource.Product, ActionEnum.Unpublish, "Unpublish products", "Business", 4, true, "System"),

            // Payment Permissions
            Permission.Create("Payment.Create", Resource.Payment, ActionEnum.Create, "Process payments", "Financial", 5, true, "System"),
            Permission.Create("Payment.Read", Resource.Payment, ActionEnum.Read, "View payment information", "Financial", 5, true, "System"),
            Permission.Create("Payment.Update", Resource.Payment, ActionEnum.Update, "Update payment information", "Financial", 5, true, "System"),
            Permission.Create("Payment.List", Resource.Payment, ActionEnum.List, "List all payments", "Financial", 5, true, "System"),
            Permission.Create("Payment.Refund", Resource.Payment, ActionEnum.Refund, "Process refunds", "Financial", 5, true, "System"),

            // Report Permissions
            Permission.Create("Report.Read", Resource.Report, ActionEnum.Read, "View reports", "Financial", 5, true, "System"),
            Permission.Create("Report.List", Resource.Report, ActionEnum.List, "List all reports", "Financial", 5, true, "System"),
            Permission.Create("Report.Execute", Resource.Report, ActionEnum.Execute, "Execute reports", "Financial", 5, true, "System")
        };

        foreach (var permission in permissions)
        {
            if (!context.Permissions.Any(p => p.Name == permission.Name))
            {
                context.Permissions.Add(permission);
            }
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedRolePermissionsAsync(MyShopIdentityDbContext context)
    {
        var rolePermissions = new List<(string RoleName, string PermissionName)>
        {
            // SuperAdmin - All permissions
            (RoleConstants.System.SuperAdmin, "System.Configure"),
            (RoleConstants.System.SuperAdmin, "System.Monitor"),
            (RoleConstants.System.SuperAdmin, "System.Backup"),
            (RoleConstants.System.SuperAdmin, "System.Migrate"),
            (RoleConstants.System.SuperAdmin, "User.Create"),
            (RoleConstants.System.SuperAdmin, "User.Read"),
            (RoleConstants.System.SuperAdmin, "User.Update"),
            (RoleConstants.System.SuperAdmin, "User.Delete"),
            (RoleConstants.System.SuperAdmin, "User.List"),
            (RoleConstants.System.SuperAdmin, "User.Activate"),
            (RoleConstants.System.SuperAdmin, "User.Deactivate"),
            (RoleConstants.System.SuperAdmin, "User.Suspend"),
            (RoleConstants.System.SuperAdmin, "Role.Create"),
            (RoleConstants.System.SuperAdmin, "Role.Read"),
            (RoleConstants.System.SuperAdmin, "Role.Update"),
            (RoleConstants.System.SuperAdmin, "Role.Delete"),
            (RoleConstants.System.SuperAdmin, "Role.List"),
            (RoleConstants.System.SuperAdmin, "Role.Assign"),
            (RoleConstants.System.SuperAdmin, "Role.Unassign"),
            (RoleConstants.System.SuperAdmin, "Permission.Create"),
            (RoleConstants.System.SuperAdmin, "Permission.Read"),
            (RoleConstants.System.SuperAdmin, "Permission.Update"),
            (RoleConstants.System.SuperAdmin, "Permission.Delete"),
            (RoleConstants.System.SuperAdmin, "Permission.List"),
            (RoleConstants.System.SuperAdmin, "Permission.Assign"),
            (RoleConstants.System.SuperAdmin, "Permission.Unassign"),
            (RoleConstants.System.SuperAdmin, "Audit.Read"),
            (RoleConstants.System.SuperAdmin, "Audit.List"),
            (RoleConstants.System.SuperAdmin, "Audit.Execute"),

            // SystemAdmin - Administrative permissions
            (RoleConstants.System.SystemAdmin, "User.Create"),
            (RoleConstants.System.SystemAdmin, "User.Read"),
            (RoleConstants.System.SystemAdmin, "User.Update"),
            (RoleConstants.System.SystemAdmin, "User.List"),
            (RoleConstants.System.SystemAdmin, "User.Activate"),
            (RoleConstants.System.SystemAdmin, "User.Deactivate"),
            (RoleConstants.System.SystemAdmin, "Role.Read"),
            (RoleConstants.System.SystemAdmin, "Role.List"),
            (RoleConstants.System.SystemAdmin, "Role.Assign"),
            (RoleConstants.System.SystemAdmin, "Role.Unassign"),
            (RoleConstants.System.SystemAdmin, "Permission.Read"),
            (RoleConstants.System.SystemAdmin, "Permission.List"),
            (RoleConstants.System.SystemAdmin, "Audit.Read"),
            (RoleConstants.System.SystemAdmin, "Audit.List"),

            // Admin - Business management permissions
            (RoleConstants.Administrative.Admin, "User.Read"),
            (RoleConstants.Administrative.Admin, "User.List"),
            (RoleConstants.Administrative.Admin, "Customer.Create"),
            (RoleConstants.Administrative.Admin, "Customer.Read"),
            (RoleConstants.Administrative.Admin, "Customer.Update"),
            (RoleConstants.Administrative.Admin, "Customer.List"),
            (RoleConstants.Administrative.Admin, "Customer.Activate"),
            (RoleConstants.Administrative.Admin, "Customer.Deactivate"),
            (RoleConstants.Administrative.Admin, "Order.Create"),
            (RoleConstants.Administrative.Admin, "Order.Read"),
            (RoleConstants.Administrative.Admin, "Order.Update"),
            (RoleConstants.Administrative.Admin, "Order.List"),
            (RoleConstants.Administrative.Admin, "Order.Approve"),
            (RoleConstants.Administrative.Admin, "Order.Reject"),
            (RoleConstants.Administrative.Admin, "Product.Create"),
            (RoleConstants.Administrative.Admin, "Product.Read"),
            (RoleConstants.Administrative.Admin, "Product.Update"),
            (RoleConstants.Administrative.Admin, "Product.List"),
            (RoleConstants.Administrative.Admin, "Product.Publish"),
            (RoleConstants.Administrative.Admin, "Product.Unpublish"),
            (RoleConstants.Administrative.Admin, "Payment.Read"),
            (RoleConstants.Administrative.Admin, "Payment.List"),
            (RoleConstants.Administrative.Admin, "Report.Read"),
            (RoleConstants.Administrative.Admin, "Report.List"),
            (RoleConstants.Administrative.Admin, "Report.Execute"),

            // Manager - Team management permissions
            (RoleConstants.Administrative.Manager, "Customer.Read"),
            (RoleConstants.Administrative.Manager, "Customer.List"),
            (RoleConstants.Administrative.Manager, "Customer.Update"),
            (RoleConstants.Administrative.Manager, "Order.Read"),
            (RoleConstants.Administrative.Manager, "Order.List"),
            (RoleConstants.Administrative.Manager, "Order.Approve"),
            (RoleConstants.Administrative.Manager, "Product.Read"),
            (RoleConstants.Administrative.Manager, "Product.List"),
            (RoleConstants.Administrative.Manager, "Payment.Read"),
            (RoleConstants.Administrative.Manager, "Payment.List"),
            (RoleConstants.Administrative.Manager, "Report.Read"),
            (RoleConstants.Administrative.Manager, "Report.List"),

            // CustomerService - Customer support permissions
            (RoleConstants.Business.CustomerService, "Customer.Read"),
            (RoleConstants.Business.CustomerService, "Customer.List"),
            (RoleConstants.Business.CustomerService, "Customer.Update"),
            (RoleConstants.Business.CustomerService, "Order.Read"),
            (RoleConstants.Business.CustomerService, "Order.List"),
            (RoleConstants.Business.CustomerService, "Product.Read"),
            (RoleConstants.Business.CustomerService, "Product.List"),

            // SalesRep - Sales permissions
            (RoleConstants.Business.SalesRep, "Customer.Create"),
            (RoleConstants.Business.SalesRep, "Customer.Read"),
            (RoleConstants.Business.SalesRep, "Customer.List"),
            (RoleConstants.Business.SalesRep, "Order.Create"),
            (RoleConstants.Business.SalesRep, "Order.Read"),
            (RoleConstants.Business.SalesRep, "Order.List"),
            (RoleConstants.Business.SalesRep, "Product.Read"),
            (RoleConstants.Business.SalesRep, "Product.List"),

            // SupportAgent - Technical support permissions
            (RoleConstants.Business.SupportAgent, "Customer.Read"),
            (RoleConstants.Business.SupportAgent, "Customer.List"),
            (RoleConstants.Business.SupportAgent, "Order.Read"),
            (RoleConstants.Business.SupportAgent, "Order.List"),
            (RoleConstants.Business.SupportAgent, "Product.Read"),
            (RoleConstants.Business.SupportAgent, "Product.List"),

            // Customer - Basic customer permissions
            (RoleConstants.User.Customer, "Order.Create"),
            (RoleConstants.User.Customer, "Order.Read"),
            (RoleConstants.User.Customer, "Order.List"),
            (RoleConstants.User.Customer, "Product.Read"),
            (RoleConstants.User.Customer, "Product.List"),
            (RoleConstants.User.Customer, "Payment.Create"),
            (RoleConstants.User.Customer, "Payment.Read"),
            (RoleConstants.User.Customer, "Payment.List"),

            // Auditor - Audit permissions
            (RoleConstants.Specialized.Auditor, "Audit.Read"),
            (RoleConstants.Specialized.Auditor, "Audit.List"),
            (RoleConstants.Specialized.Auditor, "User.Read"),
            (RoleConstants.Specialized.Auditor, "User.List"),

            // ReportViewer - Report permissions
            (RoleConstants.Specialized.ReportViewer, "Report.Read"),
            (RoleConstants.Specialized.ReportViewer, "Report.List"),
            (RoleConstants.Specialized.ReportViewer, "Report.Execute")
        };

        foreach (var (roleName, permissionName) in rolePermissions)
        {
            var role = await context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            var permission = await context.Permissions.FirstOrDefaultAsync(p => p.Name == permissionName);

            if (role != null && permission != null)
            {
                var existingRolePermission = await context.RolePermissions
                    .FirstOrDefaultAsync(rp => rp.RoleId == role.Id && rp.PermissionId == permission.Id);

                if (existingRolePermission == null)
                {
                    var rolePermission = RolePermission.Create(role.Id, permission.Id, "System");
                    context.RolePermissions.Add(rolePermission);
                }
            }
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedDefaultUsersAsync(MyShopIdentityDbContext context, UserManager<ApplicationUser> userManager)
    {
        // Create SuperAdmin user
        var superAdminEmail = "superadmin@myshop.com";
        var superAdminUser = await userManager.FindByEmailAsync(superAdminEmail);

        if (superAdminUser == null)
        {
            superAdminUser = ApplicationUser.Create(
                email: superAdminEmail,
                userName: "superadmin",
                createdBy: "System"
            );

            var result = await userManager.CreateAsync(superAdminUser, "SuperAdmin@123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(superAdminUser, RoleConstants.System.SuperAdmin);
                await userManager.ConfirmEmailAsync(superAdminUser, await userManager.GenerateEmailConfirmationTokenAsync(superAdminUser));
            }
        }

        // Create SystemAdmin user
        var systemAdminEmail = "systemadmin@myshop.com";
        var systemAdminUser = await userManager.FindByEmailAsync(systemAdminEmail);

        if (systemAdminUser == null)
        {
            systemAdminUser = ApplicationUser.Create(
                email: systemAdminEmail,
                userName: "systemadmin",
                createdBy: "System"
            );

            var result = await userManager.CreateAsync(systemAdminUser, "SystemAdmin@123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(systemAdminUser, RoleConstants.System.SystemAdmin);
                await userManager.ConfirmEmailAsync(systemAdminUser, await userManager.GenerateEmailConfirmationTokenAsync(systemAdminUser));
            }
        }
    }
}