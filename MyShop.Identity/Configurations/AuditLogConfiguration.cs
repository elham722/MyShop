using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Identity.Models;

namespace MyShop.Identity.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.UserId)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("User ID who performed the action");

        builder.Property(a => a.Action)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Action performed (Login, Logout, Update, etc.)");

        builder.Property(a => a.Details)
            .HasMaxLength(1000)
            .HasComment("Additional details about the action");

        builder.Property(a => a.IpAddress)
            .HasMaxLength(45)
            .HasComment("IP address of the user");

        builder.Property(a => a.UserAgent)
            .HasMaxLength(500)
            .HasComment("User agent string");

        builder.Property(a => a.Timestamp)
            .IsRequired()
            .HasComment("When the action was performed");

        // Indexes
        builder.HasIndex(a => a.UserId)
            .HasDatabaseName("IX_AuditLog_UserId");

        builder.HasIndex(a => a.Action)
            .HasDatabaseName("IX_AuditLog_Action");

        builder.HasIndex(a => a.Timestamp)
            .HasDatabaseName("IX_AuditLog_Timestamp");

        builder.HasIndex(a => new { a.UserId, a.Timestamp })
            .HasDatabaseName("IX_AuditLog_UserId_Timestamp");

        // Table Configuration
        builder.ToTable("AuditLogs", "Identity")
            .HasComment("Audit log for user actions");

        // Relationships
        builder.HasOne(a => a.User)
            .WithMany(u => u.AuditLogs)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}