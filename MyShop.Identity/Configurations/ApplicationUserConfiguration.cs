using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Identity.Models;

namespace MyShop.Identity.Configurations;

/// <summary>
/// Entity Framework configuration for ApplicationUser entity
/// </summary>
public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // Primary Key (inherited from IdentityUser)
        builder.HasKey(u => u.Id);

        // Properties Configuration
        builder.Property(u => u.Id)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Unique identifier for the user");

        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Unique username for the user");

        builder.Property(u => u.NormalizedUserName)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Normalized username for case-insensitive lookups");

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Email address of the user");

        builder.Property(u => u.NormalizedEmail)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Normalized email for case-insensitive lookups");

        builder.Property(u => u.EmailConfirmed)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Whether the email address has been confirmed");

        builder.Property(u => u.PasswordHash)
            .HasMaxLength(500)
            .HasComment("Hashed password of the user");

        builder.Property(u => u.SecurityStamp)
            .HasMaxLength(200)
            .HasComment("Security stamp for token validation");

        builder.Property(u => u.ConcurrencyStamp)
            .HasMaxLength(200)
            .HasComment("Concurrency stamp for optimistic concurrency control");

        builder.Property(u => u.PhoneNumber)
            .HasMaxLength(20)
            .HasComment("Phone number of the user");

        builder.Property(u => u.PhoneNumberConfirmed)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Whether the phone number has been confirmed");

        builder.Property(u => u.TwoFactorEnabled)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Whether two-factor authentication is enabled");

        builder.Property(u => u.LockoutEnd)
            .HasComment("When the lockout ends (null = not locked)");

        builder.Property(u => u.LockoutEnabled)
            .IsRequired()
            .HasDefaultValue(true)
            .HasComment("Whether lockout is enabled for this user");

        builder.Property(u => u.AccessFailedCount)
            .IsRequired()
            .HasDefaultValue(0)
            .HasComment("Number of failed access attempts");

        // Custom Properties
        builder.Property(u => u.CustomerId)
            .HasMaxLength(50)
            .HasComment("Foreign key to the customer entity");

        // MFA Properties
        builder.Property(u => u.TotpSecretKey)
            .HasMaxLength(100)
            .HasComment("TOTP secret key for two-factor authentication");

        builder.Property(u => u.TotpEnabled)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Whether TOTP is enabled");

        // Social Login Properties
        builder.Property(u => u.GoogleId)
            .HasMaxLength(100)
            .HasComment("Google OAuth ID");

        builder.Property(u => u.MicrosoftId)
            .HasMaxLength(100)
            .HasComment("Microsoft OAuth ID");

        // Extra states - inline properties
        builder.Property(u => u.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Whether the user is deleted");

        builder.Property(u => u.LastPasswordChangeAt)
            .HasComment("When the password was last changed");

        builder.Property(u => u.LastLoginAt)
            .HasComment("When the user last logged in");

        builder.Property(u => u.CreatedAt)
            .IsRequired()
            .HasComment("When the user was created");

        builder.Property(u => u.BranchId)
            .HasMaxLength(50)
            .HasComment("Branch ID associated with the user");

        // Indexes
        builder.HasIndex(u => u.UserName)
            .IsUnique()
            .HasDatabaseName("IX_ApplicationUser_UserName_Unique");

        builder.HasIndex(u => u.NormalizedUserName)
            .IsUnique()
            .HasDatabaseName("IX_ApplicationUser_NormalizedUserName_Unique");

        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_ApplicationUser_Email_Unique");

        builder.HasIndex(u => u.NormalizedEmail)
            .IsUnique()
            .HasDatabaseName("IX_ApplicationUser_NormalizedEmail_Unique");

        builder.HasIndex(u => u.CustomerId)
            .HasDatabaseName("IX_ApplicationUser_CustomerId");

        builder.HasIndex(u => u.TotpEnabled)
            .HasDatabaseName("IX_ApplicationUser_TotpEnabled");

        builder.HasIndex(u => u.GoogleId)
            .HasDatabaseName("IX_ApplicationUser_GoogleId");

        builder.HasIndex(u => u.MicrosoftId)
            .HasDatabaseName("IX_ApplicationUser_MicrosoftId");

        builder.HasIndex(u => u.LockoutEnd)
            .HasDatabaseName("IX_ApplicationUser_LockoutEnd");

        builder.HasIndex(u => u.AccessFailedCount)
            .HasDatabaseName("IX_ApplicationUser_AccessFailedCount");

        builder.HasIndex(u => u.TwoFactorEnabled)
            .HasDatabaseName("IX_ApplicationUser_TwoFactorEnabled");

        builder.HasIndex(u => u.EmailConfirmed)
            .HasDatabaseName("IX_ApplicationUser_EmailConfirmed");

        builder.HasIndex(u => u.PhoneNumberConfirmed)
            .HasDatabaseName("IX_ApplicationUser_PhoneNumberConfirmed");

        // Extra states indexes
        builder.HasIndex(u => u.IsDeleted)
            .HasDatabaseName("IX_ApplicationUser_IsDeleted");

        builder.HasIndex(u => u.CreatedAt)
            .HasDatabaseName("IX_ApplicationUser_CreatedAt");

        builder.HasIndex(u => u.LastLoginAt)
            .HasDatabaseName("IX_ApplicationUser_LastLoginAt");

        builder.HasIndex(u => u.BranchId)
            .HasDatabaseName("IX_ApplicationUser_BranchId");

        // Composite Indexes
        builder.HasIndex(u => new { u.CustomerId, u.EmailConfirmed })
            .HasDatabaseName("IX_ApplicationUser_CustomerId_EmailConfirmed");

        builder.HasIndex(u => new { u.LockoutEnabled, u.LockoutEnd })
            .HasDatabaseName("IX_ApplicationUser_LockoutEnabled_LockoutEnd");

        builder.HasIndex(u => new { u.IsDeleted, u.CreatedAt })
            .HasDatabaseName("IX_ApplicationUser_IsDeleted_CreatedAt");

        // Table Configuration
        builder.ToTable("ApplicationUsers", "Identity")
            .HasComment("Application users with enhanced security and audit features");

        // Seed Data - Commented out for now to avoid design-time issues
        // builder.HasData(GetSeedData());
    }


    private static IEnumerable<ApplicationUser> GetSeedData()
    {
        var users = new List<ApplicationUser>();

        // SuperAdmin user
        var superAdmin = ApplicationUser.Create(
            "superadmin@myshop.com",
            "superadmin",
            null);
        superAdmin.Id = "superadmin-user-id";
        superAdmin.EmailConfirmed = true;
        superAdmin.TwoFactorEnabled = true;
        superAdmin.SetTotpEnabled(true);
        users.Add(superAdmin);

        // Admin user
        var admin = ApplicationUser.Create(
            "admin@myshop.com",
            "admin",
            null);
        admin.Id = "admin-user-id";
        admin.EmailConfirmed = true;
        admin.TwoFactorEnabled = true;
        admin.SetTotpEnabled(true);
        users.Add(admin);

        // Manager user
        var manager = ApplicationUser.Create(
            "manager@myshop.com",
            "manager",
            null);
        manager.Id = "manager-user-id";
        manager.EmailConfirmed = true;
        manager.TwoFactorEnabled = false;
        users.Add(manager);

        // CustomerService user
        var customerService = ApplicationUser.Create(
            "customerservice@myshop.com",
            "customerservice",
            null);
        customerService.Id = "customerservice-user-id";
        customerService.EmailConfirmed = true;
        customerService.TwoFactorEnabled = false;
        users.Add(customerService);

        // SalesRep user
        var salesRep = ApplicationUser.Create(
            "salesrep@myshop.com",
            "salesrep",
            null);
        salesRep.Id = "salesrep-user-id";
        salesRep.EmailConfirmed = true;
        salesRep.TwoFactorEnabled = false;
        users.Add(salesRep);

        // SupportAgent user
        var supportAgent = ApplicationUser.Create(
            "supportagent@myshop.com",
            "supportagent",
            null);
        supportAgent.Id = "supportagent-user-id";
        supportAgent.EmailConfirmed = true;
        supportAgent.TwoFactorEnabled = false;
        users.Add(supportAgent);

        // Customer user
        var customer = ApplicationUser.Create(
            "customer@myshop.com",
            "customer",
            "customer-domain-id");
        customer.Id = "customer-user-id";
        customer.EmailConfirmed = true;
        customer.TwoFactorEnabled = false;
        users.Add(customer);

        // Auditor user
        var auditor = ApplicationUser.Create(
            "auditor@myshop.com",
            "auditor",
            null);
        auditor.Id = "auditor-user-id";
        auditor.EmailConfirmed = true;
        auditor.TwoFactorEnabled = true;
        auditor.SetTotpEnabled(true);
        users.Add(auditor);

        // ReportViewer user
        var reportViewer = ApplicationUser.Create(
            "reportviewer@myshop.com",
            "reportviewer",
            null);
        reportViewer.Id = "reportviewer-user-id";
        reportViewer.EmailConfirmed = true;
        reportViewer.TwoFactorEnabled = false;
        users.Add(reportViewer);

        return users;
    }
}