using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Identity.Models;
using MyShop.Identity.Enums;

namespace MyShop.Identity.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("Unique identifier for the audit log entry");

        builder.Property(a => a.UserId)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("ID of the user who performed the action");

        builder.Property(a => a.Action)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Action performed (e.g., Login, Logout, Create, Update, Delete)");

        builder.Property(a => a.EntityType)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Type of entity that was affected (e.g., User, Customer, Order)");

        builder.Property(a => a.EntityId)
            .IsRequired()
            .HasMaxLength(50)
            .HasComment("ID of the entity that was affected");

        builder.Property(a => a.OldValues)
            .HasColumnType("nvarchar(max)")
            .HasComment("JSON representation of the old values before the change");

        builder.Property(a => a.NewValues)
            .HasColumnType("nvarchar(max)")
            .HasComment("JSON representation of the new values after the change");

        builder.Property(a => a.Timestamp)
            .IsRequired()
            .HasComment("When the action was performed");

        builder.Property(a => a.IpAddress)
            .IsRequired()
            .HasMaxLength(45) 
            .HasComment("IP address of the client");

        builder.Property(a => a.UserAgent)
            .HasMaxLength(500)
            .HasComment("User agent string from the client");

        builder.Property(a => a.DeviceInfo)
            .HasMaxLength(200)
            .HasComment("Device information (OS, Browser, etc.)");

        builder.Property(a => a.SessionId)
            .HasMaxLength(100)
            .HasComment("Session identifier");

        builder.Property(a => a.RequestId)
            .HasMaxLength(100)
            .HasComment("Request correlation ID");

        builder.Property(a => a.AdditionalData)
            .HasColumnType("nvarchar(max)")
            .HasComment("Additional context data in JSON format");

        builder.Property(a => a.IsSuccess)
            .IsRequired()
            .HasComment("Whether the operation was successful");

        builder.Property(a => a.ErrorMessage)
            .HasMaxLength(1000)
            .HasComment("Error message if the operation failed");

        builder.Property(a => a.Severity)
            .IsRequired()
            .HasConversion(
                v => v.ToStringValue(),
                v => AuditSeverityExtensions.ParseFromString(v))
            .HasMaxLength(20)
            .HasComment("Severity level of the audit entry");

        
        builder.HasIndex(a => a.UserId)
            .HasDatabaseName("IX_AuditLog_UserId");

        builder.HasIndex(a => a.Timestamp)
            .HasDatabaseName("IX_AuditLog_Timestamp");

        builder.HasIndex(a => a.Action)
            .HasDatabaseName("IX_AuditLog_Action");

        builder.HasIndex(a => a.EntityType)
            .HasDatabaseName("IX_AuditLog_EntityType");

        builder.HasIndex(a => a.EntityId)
            .HasDatabaseName("IX_AuditLog_EntityId");

        builder.HasIndex(a => a.Severity)
            .HasDatabaseName("IX_AuditLog_Severity");

        builder.HasIndex(a => a.IsSuccess)
            .HasDatabaseName("IX_AuditLog_IsSuccess");

       
        builder.HasIndex(a => new { a.UserId, a.Timestamp })
            .HasDatabaseName("IX_AuditLog_UserId_Timestamp");

        builder.HasIndex(a => new { a.EntityType, a.EntityId, a.Timestamp })
            .HasDatabaseName("IX_AuditLog_Entity_TypeId_Timestamp");

        builder.HasIndex(a => new { a.Severity, a.Timestamp })
            .HasDatabaseName("IX_AuditLog_Severity_Timestamp");


    }
}