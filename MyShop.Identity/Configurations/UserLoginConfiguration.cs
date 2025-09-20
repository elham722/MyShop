using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Identity.Models;

namespace MyShop.Identity.Configurations;

/// <summary>
/// Entity Framework configuration for UserLogin entity
/// </summary>
public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
{
    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        // Primary Key (inherited from IdentityUserLogin<string>)
        builder.HasKey(ul => new { ul.LoginProvider, ul.ProviderKey });

        // Properties Configuration
        builder.Property(ul => ul.UserId)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Foreign key to the user");

        builder.Property(ul => ul.LoginProvider)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("The login provider (e.g., Google, Facebook, Local)");

        builder.Property(ul => ul.ProviderKey)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("The provider key (e.g., Google ID, Facebook ID)");

        builder.Property(ul => ul.ProviderDisplayName)
            .HasMaxLength(100)
            .HasComment("The display name of the provider");

        builder.Property(ul => ul.DeviceInfo)
            .HasMaxLength(200)
            .HasComment("Device information (OS, model, etc.)");

        builder.Property(ul => ul.IpAddress)
            .HasMaxLength(45) // IPv6 max length
            .HasComment("IP address of the login");

        builder.Property(ul => ul.UserAgent)
            .HasMaxLength(500)
            .HasComment("User agent string from the browser");

        builder.Property(ul => ul.Location)
            .HasMaxLength(100)
            .HasComment("Geographic location of the login");

        builder.Property(ul => ul.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasComment("Whether this login is currently active");

        builder.Property(ul => ul.IsTrusted)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Whether this device is trusted");

        builder.Property(ul => ul.LastUsedAt)
            .HasComment("When this login was last used");

        builder.Property(ul => ul.CreatedAt)
            .IsRequired()
            .HasComment("When this login was created");

        builder.Property(ul => ul.UpdatedAt)
            .HasComment("When this login was last updated");

        builder.Property(ul => ul.CreatedBy)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Who created this login");

        builder.Property(ul => ul.UpdatedBy)
            .HasMaxLength(100)
            .HasComment("Who last updated this login");

        // Indexes
        builder.HasIndex(ul => ul.UserId)
            .HasDatabaseName("IX_UserLogin_UserId");

        builder.HasIndex(ul => ul.LoginProvider)
            .HasDatabaseName("IX_UserLogin_LoginProvider");

        builder.HasIndex(ul => ul.IsActive)
            .HasDatabaseName("IX_UserLogin_IsActive");

        builder.HasIndex(ul => ul.IsTrusted)
            .HasDatabaseName("IX_UserLogin_IsTrusted");

        builder.HasIndex(ul => ul.LastUsedAt)
            .HasDatabaseName("IX_UserLogin_LastUsedAt");

        builder.HasIndex(ul => ul.CreatedAt)
            .HasDatabaseName("IX_UserLogin_CreatedAt");

        builder.HasIndex(ul => ul.IpAddress)
            .HasDatabaseName("IX_UserLogin_IpAddress");

        builder.HasIndex(ul => ul.Location)
            .HasDatabaseName("IX_UserLogin_Location");

        builder.HasIndex(ul => new { ul.UserId, ul.IsActive })
            .HasDatabaseName("IX_UserLogin_UserId_IsActive");

        builder.HasIndex(ul => new { ul.UserId, ul.IsTrusted })
            .HasDatabaseName("IX_UserLogin_UserId_IsTrusted");

        // Table Configuration
        builder.ToTable("UserLogins", "Identity")
            .HasComment("User login tracking for multi-device management and security monitoring");

        // Seed Data - Commented out for now to avoid design-time issues
        // builder.HasData(GetSeedData());
    }

    private static IEnumerable<UserLogin> GetSeedData()
    {
        var userLogins = new List<UserLogin>();

        // SuperAdmin logins
        userLogins.Add(UserLogin.Create(
            "superadmin-user-id",
            "Local",
            "superadmin@myshop.com",
            "Windows 11 Desktop",
            "192.168.1.100",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            "New York, NY",
            true, // Trusted
            "System"));

        userLogins.Add(UserLogin.Create(
            "superadmin-user-id",
            "Google",
            "superadmin@gmail.com",
            "iPhone 14 Pro",
            "192.168.1.101",
            "Mozilla/5.0 (iPhone; CPU iPhone OS 16_0 like Mac OS X)",
            "New York, NY",
            true, // Trusted
            "System"));

        // Admin logins
        userLogins.Add(UserLogin.Create(
            "admin-user-id",
            "Local",
            "admin@myshop.com",
            "MacBook Pro",
            "192.168.1.102",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36",
            "San Francisco, CA",
            true, // Trusted
            "System"));

        userLogins.Add(UserLogin.Create(
            "admin-user-id",
            "Microsoft",
            "admin@outlook.com",
            "Surface Pro",
            "192.168.1.103",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            "San Francisco, CA",
            false, // Not trusted
            "System"));

        // Manager logins
        userLogins.Add(UserLogin.Create(
            "manager-user-id",
            "Local",
            "manager@myshop.com",
            "Dell Laptop",
            "192.168.1.104",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            "London, UK",
            true, // Trusted
            "System"));

        userLogins.Add(UserLogin.Create(
            "manager-user-id",
            "Facebook",
            "manager@facebook.com",
            "Samsung Galaxy S21",
            "192.168.1.105",
            "Mozilla/5.0 (Linux; Android 11; SM-G991B) AppleWebKit/537.36",
            "London, UK",
            false, // Not trusted
            "System"));

        // CustomerService logins
        userLogins.Add(UserLogin.Create(
            "customerservice-user-id",
            "Local",
            "customerservice@myshop.com",
            "HP Desktop",
            "192.168.1.106",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            "Tokyo, Japan",
            true, // Trusted
            "System"));

        // SalesRep logins
        userLogins.Add(UserLogin.Create(
            "salesrep-user-id",
            "Local",
            "salesrep@myshop.com",
            "iPad Pro",
            "192.168.1.107",
            "Mozilla/5.0 (iPad; CPU OS 16_0 like Mac OS X) AppleWebKit/537.36",
            "Berlin, Germany",
            false, // Not trusted
            "System"));

        // SupportAgent logins
        userLogins.Add(UserLogin.Create(
            "supportagent-user-id",
            "Local",
            "supportagent@myshop.com",
            "Lenovo ThinkPad",
            "192.168.1.108",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            "Sydney, Australia",
            true, // Trusted
            "System"));

        // Customer logins
        userLogins.Add(UserLogin.Create(
            "customer-user-id",
            "Local",
            "customer@myshop.com",
            "iPhone 13",
            "192.168.1.109",
            "Mozilla/5.0 (iPhone; CPU iPhone OS 15_0 like Mac OS X)",
            "Toronto, Canada",
            false, // Not trusted
            "System"));

        userLogins.Add(UserLogin.Create(
            "customer-user-id",
            "Google",
            "customer@gmail.com",
            "Chrome Browser",
            "192.168.1.110",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            "Toronto, Canada",
            false, // Not trusted
            "System"));

        return userLogins;
    }
}