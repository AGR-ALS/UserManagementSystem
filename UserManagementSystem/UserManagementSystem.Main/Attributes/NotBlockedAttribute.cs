using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UserManagementSystem.Domain.Abstractions;

namespace UserManagementSystem.Main.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class NotBlockedAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly IUsersService _usersService;

    public NotBlockedAttribute(IUsersService usersService)
    {
        _usersService = usersService;
    }
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var id = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)
            ?.Value;
        if (string.IsNullOrEmpty(id))
        {
            context.Result = new ForbidResult();
            
            return;
        }
        if ((await _usersService.GetUserByIdAsync(new Guid(id))).ActivityStatus == false)
        {
            context.Result = new ForbidResult();
        }
    }
}