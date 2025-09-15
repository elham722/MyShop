using Microsoft.Extensions.Logging;

namespace MyShop.Contracts.Enums.Identity;

public enum AuditSeverity
{
    Info = 0,
    Warning = 1,
    Error = 2,
    Critical = 3,
    Debug = 4
}

public static class AuditSeverityExtensions
{
    public static string ToStringValue(this AuditSeverity severity)
    {
        return severity switch
        {
            AuditSeverity.Info => "Info",
            AuditSeverity.Warning => "Warning",
            AuditSeverity.Error => "Error",
            AuditSeverity.Critical => "Critical",
            AuditSeverity.Debug => "Debug",
            _ => "Info"
        };
    }

    public static AuditSeverity ParseFromString(string? value)
    {
        return value?.ToLowerInvariant() switch
        {
            "info" => AuditSeverity.Info,
            "warning" => AuditSeverity.Warning,
            "error" => AuditSeverity.Error,
            "critical" => AuditSeverity.Critical,
            "debug" => AuditSeverity.Debug,
            _ => AuditSeverity.Info
        };
    }

    public static bool RequiresImmediateAttention(this AuditSeverity severity)
    {
        return severity == AuditSeverity.Critical || severity == AuditSeverity.Error;
    }

    public static LogLevel ToLogLevel(this AuditSeverity severity)
    {
        return severity switch
        {
            AuditSeverity.Info => LogLevel.Information,
            AuditSeverity.Warning => LogLevel.Warning,
            AuditSeverity.Error => LogLevel.Error,
            AuditSeverity.Critical => LogLevel.Critical,
            AuditSeverity.Debug => LogLevel.Debug,
            _ => LogLevel.Information
        };
    }
}