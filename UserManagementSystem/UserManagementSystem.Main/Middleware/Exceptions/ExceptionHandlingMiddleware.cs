using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Application.Exceptions;

namespace UserManagementSystem.Main.Middleware.Exceptions;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            if (context.Response.HasStarted)
            {
                return; 
            }
            context.Response.StatusCode = ex switch
            {
                ApplicationException => StatusCodes.Status400BadRequest,
                InvalidCredentialException => StatusCodes.Status401Unauthorized,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                EntityNotFoundException => StatusCodes.Status404NotFound,
                EntityCreatingException => StatusCodes.Status422UnprocessableEntity,
                _ => StatusCodes.Status500InternalServerError,
            };
            
            await context.Response.WriteAsJsonAsync(
                new ProblemDetails
                {
                    Type = ex.GetType().Name,
                    Status = context.Response.StatusCode,
                    Detail = ex.Message,
                    Instance = context.Request.Path,
                });
        }
    }
}