using MyShop.Contracts.DTOs.Identity.HealthCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Contracts.Identity.Services
{
    /// <summary>
    /// Service for managing Identity health checks
    /// </summary>
    public interface IIdentityHealthCheckService
    {
        Task<IdentityHealthStatus> CheckOverallHealthAsync();
        Task<DatabaseHealthStatus> CheckDatabaseHealthAsync();
        Task<AuthenticationHealthStatus> CheckAuthenticationHealthAsync();
        Task<AuthorizationHealthStatus> CheckAuthorizationHealthAsync();
        Task<SecurityHealthStatus> CheckSecurityHealthAsync();
        Task<PerformanceHealthStatus> CheckPerformanceHealthAsync();
        Task<List<HealthIssue>> GetHealthIssuesAsync();
        Task<bool> IsHealthyAsync();
        Task<HealthMetrics> GetHealthMetricsAsync();
    }

}
