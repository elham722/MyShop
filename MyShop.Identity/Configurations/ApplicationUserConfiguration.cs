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

        builder.Property(u => u.SmsEnabled)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Whether SMS two-factor authentication is enabled");

        // Social Login Properties
        builder.Property(u => u.GoogleId)
            .HasMaxLength(100)
            .HasComment("Google OAuth ID");

        builder.Property(u => u.MicrosoftId)
            .HasMaxLength(100)
            .HasComment("Microsoft OAuth ID");

        // Value Objects Configuration
        ConfigureAccountInfo(builder);
        ConfigureSecurityInfo(builder);
        ConfigureAuditInfo(builder);

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

        builder.HasIndex(u => u.SmsEnabled)
            .HasDatabaseName("IX_ApplicationUser_SmsEnabled");

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

        // Composite Indexes
        builder.HasIndex(u => new { u.CustomerId, u.EmailConfirmed })
            .HasDatabaseName("IX_ApplicationUser_CustomerId_EmailConfirmed");

        builder.HasIndex(u => new { u.TotpEnabled, u.SmsEnabled })
            .HasDatabaseName("IX_ApplicationUser_TotpEnabled_SmsEnabled");

        builder.HasIndex(u => new { u.LockoutEnabled, u.LockoutEnd })
            .HasDatabaseName("IX_ApplicationUser_LockoutEnabled_LockoutEnd");

        // Table Configuration
        builder.ToTable("ApplicationUsers", "Identity")
            .HasComment("Application users with enhanced security and audit features");

        // Seed Data - Commented out for now to avoid design-time issues
        // builder.HasData(GetSeedData());
    }

    private static void ConfigureAccountInfo(EntityTypeBuilder<ApplicationUser> builder)
    {
        // AccountInfo Value Object Configuration
        builder.OwnsOne(u => u.Account, account =>
        {
            account.Property(a => a.CreatedAt)
                .IsRequired()
                .HasColumnName("Account_CreatedAt")
                .HasComment("When the account was created");

            account.Property(a => a.LastLoginAt)
                .HasColumnName("Account_LastLoginAt")
                .HasComment("When the user last logged in");

            account.Property(a => a.LastPasswordChangeAt)
                .HasColumnName("Account_LastPasswordChangeAt")
                .HasComment("When the password was last changed");

            account.Property(a => a.LoginAttempts)
                .IsRequired()
                .HasDefaultValue(0)
                .HasColumnName("Account_LoginAttempts")
                .HasComment("Number of login attempts");

            account.Property(a => a.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .HasColumnName("Account_IsActive")
                .HasComment("Whether the account is active");

            account.Property(a => a.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("Account_IsDeleted")
                .HasComment("Whether the account is deleted");

            account.Property(a => a.DeletedAt)
                .HasColumnName("Account_DeletedAt")
                .HasComment("When the account was deleted");

            account.Property(a => a.BranchId)
                .HasMaxLength(50)
                .HasColumnName("Account_BranchId")
                .HasComment("Branch ID associated with the account");

            // Indexes for AccountInfo
            account.HasIndex(a => a.IsActive)
                .HasDatabaseName("IX_ApplicationUser_Account_IsActive");

            account.HasIndex(a => a.IsDeleted)
                .HasDatabaseName("IX_ApplicationUser_Account_IsDeleted");

            account.HasIndex(a => a.CreatedAt)
                .HasDatabaseName("IX_ApplicationUser_Account_CreatedAt");

            account.HasIndex(a => a.LastLoginAt)
                .HasDatabaseName("IX_ApplicationUser_Account_LastLoginAt");

            account.HasIndex(a => a.BranchId)
                .HasDatabaseName("IX_ApplicationUser_Account_BranchId");
        });
    }

    private static void ConfigureSecurityInfo(EntityTypeBuilder<ApplicationUser> builder)
    {
        // SecurityInfo Value Object Configuration
        builder.OwnsOne(u => u.Security, security =>
        {
            security.Property(s => s.TwoFactorEnabled)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("Security_TwoFactorEnabled")
                .HasComment("Whether two-factor authentication is enabled");

            security.Property(s => s.TwoFactorSecret)
                .HasMaxLength(100)
                .HasColumnName("Security_TwoFactorSecret")
                .HasComment("Two-factor authentication secret key");

            security.Property(s => s.TwoFactorEnabledAt)
                .HasColumnName("Security_TwoFactorEnabledAt")
                .HasComment("When two-factor authentication was enabled");

            security.Property(s => s.SecurityQuestion)
                .HasMaxLength(200)
                .HasColumnName("Security_Question")
                .HasComment("Security question for account recovery");

            security.Property(s => s.SecurityAnswer)
                .HasMaxLength(200)
                .HasColumnName("Security_Answer")
                .HasComment("Security answer for account recovery (encrypted)");

            security.Property(s => s.LastSecurityUpdate)
                .HasColumnName("Security_LastSecurityUpdate")
                .HasComment("When the last security update was performed");

            // Indexes for SecurityInfo
            security.HasIndex(s => s.TwoFactorEnabled)
                .HasDatabaseName("IX_ApplicationUser_Security_TwoFactorEnabled");

            security.HasIndex(s => s.TwoFactorEnabledAt)
                .HasDatabaseName("IX_ApplicationUser_Security_TwoFactorEnabledAt");

            security.HasIndex(s => s.LastSecurityUpdate)
                .HasDatabaseName("IX_ApplicationUser_Security_LastSecurityUpdate");
        });
    }

    private static void ConfigureAuditInfo(EntityTypeBuilder<ApplicationUser> builder)
    {
        // AuditInfo Value Object Configuration
        builder.OwnsOne(u => u.Audit, audit =>
        {
            audit.Property(a => a.CreatedBy)
                .HasMaxLength(100)
                .HasColumnName("Audit_CreatedBy")
                .HasComment("Who created the user");

            audit.Property(a => a.CreatedAt)
                .IsRequired()
                .HasColumnName("Audit_CreatedAt")
                .HasComment("When the user was created");

            audit.Property(a => a.ModifiedBy)
                .HasMaxLength(100)
                .HasColumnName("Audit_ModifiedBy")
                .HasComment("Who last modified the user");

            audit.Property(a => a.ModifiedAt)
                .HasColumnName("Audit_ModifiedAt")
                .HasComment("When the user was last modified");

            audit.Property(a => a.IpAddress)
                .HasMaxLength(45)
                .HasColumnName("Audit_IpAddress")
                .HasComment("IP address of the last operation");

            audit.Property(a => a.UserAgent)
                .HasMaxLength(500)
                .HasColumnName("Audit_UserAgent")
                .HasComment("User agent of the last operation");

            // Indexes for AuditInfo
            audit.HasIndex(a => a.CreatedAt)
                .HasDatabaseName("IX_ApplicationUser_Audit_CreatedAt");

            audit.HasIndex(a => a.ModifiedAt)
                .HasDatabaseName("IX_ApplicationUser_Audit_ModifiedAt");

            audit.HasIndex(a => a.CreatedBy)
                .HasDatabaseName("IX_ApplicationUser_Audit_CreatedBy");

            audit.HasIndex(a => a.ModifiedBy)
                .HasDatabaseName("IX_ApplicationUser_Audit_ModifiedBy");

            audit.HasIndex(a => a.IpAddress)
                .HasDatabaseName("IX_ApplicationUser_Audit_IpAddress");
        });
    }

    private static IEnumerable<ApplicationUser> GetSeedData()
    {
        var users = new List<ApplicationUser>();

        // SuperAdmin user
        var superAdmin = ApplicationUser.Create(
            "superadmin@myshop.com",
            "superadmin",
            null,
            "System");
        superAdmin.Id = "superadmin-user-id";
        superAdmin.EmailConfirmed = true;
        superAdmin.TwoFactorEnabled = true;
        superAdmin.SetTotpEnabled(true);
        superAdmin.SetSmsEnabled(true);
        users.Add(superAdmin);

        // Admin user
        var admin = ApplicationUser.Create(
            "admin@myshop.com",
            "admin",
            null,
            "System");
        admin.Id = "admin-user-id";
        admin.EmailConfirmed = true;
        admin.TwoFactorEnabled = true;
        admin.SetTotpEnabled(true);
        users.Add(admin);

        // Manager user
        var manager = ApplicationUser.Create(
            "manager@myshop.com",
            "manager",
            null,
            "System");
        manager.Id = "manager-user-id";
        manager.EmailConfirmed = true;
        manager.TwoFactorEnabled = false;
        users.Add(manager);

        // CustomerService user
        var customerService = ApplicationUser.Create(
            "customerservice@myshop.com",
            "customerservice",
            null,
            "System");
        customerService.Id = "customerservice-user-id";
        customerService.EmailConfirmed = true;
        customerService.TwoFactorEnabled = false;
        users.Add(customerService);

        // SalesRep user
        var salesRep = ApplicationUser.Create(
            "salesrep@myshop.com",
            "salesrep",
            null,
            "System");
        salesRep.Id = "salesrep-user-id";
        salesRep.EmailConfirmed = true;
        salesRep.TwoFactorEnabled = false;
        users.Add(salesRep);

        // SupportAgent user
        var supportAgent = ApplicationUser.Create(
            "supportagent@myshop.com",
            "supportagent",
            null,
            "System");
        supportAgent.Id = "supportagent-user-id";
        supportAgent.EmailConfirmed = true;
        supportAgent.TwoFactorEnabled = false;
        users.Add(supportAgent);

        // Customer user
        var customer = ApplicationUser.Create(
            "customer@myshop.com",
            "customer",
            "customer-domain-id",
            "System");
        customer.Id = "customer-user-id";
        customer.EmailConfirmed = true;
        customer.TwoFactorEnabled = false;
        users.Add(customer);

        // Auditor user
        var auditor = ApplicationUser.Create(
            "auditor@myshop.com",
            "auditor",
            null,
            "System");
        auditor.Id = "auditor-user-id";
        auditor.EmailConfirmed = true;
        auditor.TwoFactorEnabled = true;
        auditor.SetTotpEnabled(true);
        users.Add(auditor);

        // ReportViewer user
        var reportViewer = ApplicationUser.Create(
            "reportviewer@myshop.com",
            "reportviewer",
            null,
            "System");
        reportViewer.Id = "reportviewer-user-id";
        reportViewer.EmailConfirmed = true;
        reportViewer.TwoFactorEnabled = false;
        users.Add(reportViewer);

        return users;
    }
}