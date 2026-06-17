namespace UserManagementSystem.Main.Middleware.Authentication;

public class UnauthorizedRedirectMiddleware
{
    private readonly RequestDelegate _next;

    public UnauthorizedRedirectMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
            if (context.Response.StatusCode == 401)
            {
                context.Response.Redirect("/Users/Login");
            }
            else if (context.Response.StatusCode == 403)
            {
                context.Response.Redirect("/Error/Blocked");
            }
        }
        catch (UnauthorizedAccessException)
        {
            context.Response.Redirect("/Users/Login");
        }
    }
}