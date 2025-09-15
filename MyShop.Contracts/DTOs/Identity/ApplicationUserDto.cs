namespace MyShop.Contracts.DTOs.Identity
{
    /// <summary>
    /// Data Transfer Object for ApplicationUser
    /// </summary>
    public class ApplicationUserDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        
        // Custom Properties
        public string? CustomerId { get; set; }
        
        // MFA Properties
        public bool TotpEnabled { get; set; }
        public bool SmsEnabled { get; set; }
        
        // Social Login Properties
        public string? GoogleId { get; set; }
        public string? MicrosoftId { get; set; }
        
        // Computed Properties
        public bool IsLocked { get; set; }
        public bool IsAccountLocked { get; set; }
        public bool IsActive { get; set; }
        public bool IsNewUser { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime? LastPasswordChangeAt { get; set; }
        public int LoginAttempts { get; set; }
        public bool RequiresPasswordChange { get; set; }
        
        // Account Info
        public DateTime CreatedAt { get; set; }
        public string? BranchId { get; set; }
        
        // Audit Info
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}