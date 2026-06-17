using System.Security.Claims;
using UserManagementSystem.Domain.Abstractions;

namespace UserManagementSystem.Main.Middleware.Activity;

public class UpdateUserLastActivityMiddleware
{
    private readonly RequestDelegate _next;

    public UpdateUserLastActivityMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUsersService usersService)
    {
        await _next(context);
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userIdString = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdString, out Guid userId))
            {
                await usersService.UpdateUsersLastActivityTimeAsync(userId, context.RequestAborted);
            }
        }
    }
}