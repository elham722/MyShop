using Microsoft.AspNetCore.Authorization;
using MyShop.Contracts.Enums.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Identity.Authorization.Requirements
{
    /// <summary>
    /// Authorization requirement for resource-based authorization
    /// </summary>
    public class ResourceRequirement : IAuthorizationRequirement
    {
        public Resource Resource { get; }
        public ActionEnum Action { get; }

        public ResourceRequirement(Resource resource, ActionEnum action)
        {
            Resource = resource;
            Action = action;
        }
    }


}
