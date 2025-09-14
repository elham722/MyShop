using Microsoft.AspNetCore.Mvc;

namespace MyShop.API.Filters;

/// <summary>
/// Attribute to apply API response standardization to controllers or actions
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiResponseAttribute : TypeFilterAttribute
{
    public ApiResponseAttribute() : base(typeof(ApiResponseActionFilter))
    {
    }
}