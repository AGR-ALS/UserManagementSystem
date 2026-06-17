using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserManagementSystem.Application.Abstractions.Authentication.Jwt;
using UserManagementSystem.Domain.Models;

namespace UserManagementSystem.Infrastructure.Authentication.Jwt;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public string GenerateJwtToken(User user)
    {
        Claim[] claims = [new(ClaimTypes.NameIdentifier, user.Id.ToString()), new(ClaimTypes.Email, user.Email), new(ClaimTypes.Name, user.Name)];
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
            signingCredentials: signingCredentials,
            notBefore: DateTime.UtcNow,
            claims: claims
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        
        return tokenString;
    }
}