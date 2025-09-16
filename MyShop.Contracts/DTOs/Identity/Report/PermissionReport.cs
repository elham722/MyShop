using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.Report
{
    /// <summary>
    /// Permission report model
    /// </summary>
    public class PermissionReport
    {
        public string PermissionId { get; set; } = string.Empty;
        public string PermissionName { get; set; } = string.Empty;
        public string Resource { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsSystemPermission { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int RoleCount { get; set; }
        public int UserCount { get; set; }
        public List<string> Roles { get; set; } = new();
        public List<string> Users { get; set; } = new();
        public PermissionUsageSummary UsageSummary { get; set; } = new();
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    }

}
