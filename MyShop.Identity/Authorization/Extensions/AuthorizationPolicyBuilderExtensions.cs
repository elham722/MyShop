using Microsoft.AspNetCore.Authorization;
using MyShop.Contracts.Enums.Identity;
using MyShop.Identity.Constants;
using MyShop.Identity.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Identity.Authorization.Requirements;

namespace MyShop.Identity.Authorization.Extensions
{
    /// <summary>
    /// Authorization policy builder extensions
    /// </summary>
    public static class AuthorizationPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequireResourcePermission(this AuthorizationPolicyBuilder builder,
            Resource resource, ActionEnum action)
        {
            builder.Requirements.Add(new ResourceRequirement(resource, action));
            return builder;
        }

        public static AuthorizationPolicyBuilder RequireSystemAdmin(this AuthorizationPolicyBuilder builder)
        {
            builder.RequireRole(RoleConstants.System.SuperAdmin, RoleConstants.System.SystemAdmin);
            return builder;
        }

        public static AuthorizationPolicyBuilder RequireAdminOrAbove(this AuthorizationPolicyBuilder builder)
        {
            builder.RequireRole(
                RoleConstants.System.SuperAdmin,
                RoleConstants.System.SystemAdmin,
                RoleConstants.Administrative.Admin
            );
            return builder;
        }

        public static AuthorizationPolicyBuilder RequireManagerOrAbove(this AuthorizationPolicyBuilder builder)
        {
            builder.RequireRole(
                RoleConstants.System.SuperAdmin,
                RoleConstants.System.SystemAdmin,
                RoleConstants.Administrative.Admin,
                RoleConstants.Administrative.Manager
            );
            return builder;
        }

        public static AuthorizationPolicyBuilder RequireBusinessUserOrAbove(this AuthorizationPolicyBuilder builder)
        {
            builder.RequireRole(
                RoleConstants.System.SuperAdmin,
                RoleConstants.System.SystemAdmin,
                RoleConstants.Administrative.Admin,
                RoleConstants.Administrative.Manager,
                RoleConstants.Business.CustomerService,
                RoleConstants.Business.SalesRep,
                RoleConstants.Business.SupportAgent
            );
            return builder;
        }
    }
}
