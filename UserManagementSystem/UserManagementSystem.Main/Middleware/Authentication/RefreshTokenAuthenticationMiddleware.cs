using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using UserManagementSystem.Domain.Abstractions;
using UserManagementSystem.Infrastructure.Authentication.Tokens.Settings;

namespace UserManagementSystem.Main.Middleware.Authentication;

public class RefreshTokenAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TokenIdentifiers _tokenIdentifiers;

    public RefreshTokenAuthenticationMiddleware(RequestDelegate next, IOptions<TokenIdentifiers> tokenIdentifiers)
    {
        _next = next;
        _tokenIdentifiers = tokenIdentifiers.Value;
    }

    public async Task InvokeAsync(HttpContext context, IUsersService usersService)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            await _next(context);
            
            return;
        }
        
        var refreshToken = context.Request.Cookies[_tokenIdentifiers.RefreshTokenIdentifier];

        if (!string.IsNullOrEmpty(refreshToken))
        {
            try
            {
                var newAccessToken = await usersService.LoginAsync(refreshToken, context.RequestAborted);
                context.Response.Cookies.Append(_tokenIdentifiers.AccessTokenIdentifier, newAccessToken);
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(newAccessToken);
                var claimsIdentity = new ClaimsIdentity(jwtToken.Claims, _tokenIdentifiers.AccessTokenIdentifier);
                context.User = new ClaimsPrincipal(claimsIdentity);
            }
            catch (UnauthorizedAccessException) 
            {
                context.Response.Cookies.Delete(_tokenIdentifiers.AccessTokenIdentifier);
                context.Response.Cookies.Delete(_tokenIdentifiers.RefreshTokenIdentifier);
            }
        }
        
        await _next(context);
    }
}