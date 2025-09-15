using Microsoft.AspNetCore.Authorization;
using MyShop.Identity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Identity.Authorization.Requirements;

namespace MyShop.Identity.Authorization.Handlers
{
    /// <summary>
    /// Authorization handler for resource-based authorization
    /// </summary>
    public class ResourceAuthorizationHandler : AuthorizationHandler<ResourceRequirement>
    {
        private readonly Contracts.Identity.Services.IAuthorizationService _authorizationService;

        public ResourceAuthorizationHandler(Contracts.Identity.Services.IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ResourceRequirement requirement)
        {
            var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                context.Fail();
                return;
            }

            var hasPermission = await _authorizationService.HasPermissionAsync(userId, requirement.Resource, requirement.Action);

            if (hasPermission)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }

}
