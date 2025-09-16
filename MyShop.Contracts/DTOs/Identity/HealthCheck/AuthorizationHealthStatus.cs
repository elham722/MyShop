using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.DTOs.Identity.HealthCheck
{
    /// <summary>
    /// Authorization health status
    /// </summary>
    public class AuthorizationHealthStatus
    {
        public bool IsHealthy { get; set; }
        public string Status { get; set; } = string.Empty;
        public int TotalRoles { get; set; }
        public int ActiveRoles { get; set; }
        public int TotalPermissions { get; set; }
        public int ActivePermissions { get; set; }
        public int RolesWithUsers { get; set; }
        public int PermissionsWithRoles { get; set; }
        public List<HealthIssue> Issues { get; set; } = new();
    }

}
