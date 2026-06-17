using System.Security.Cryptography;
using UserManagementSystem.Application.Abstractions.Authentication;
using UserManagementSystem.Domain.Models;
using UserManagementSystem.Domain.Models.Tokens;

namespace UserManagementSystem.Infrastructure.Authentication;

public class SecureTokenGenerator : ISecureTokenGenerator
{
    public string GenerateToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }

    public bool VerifyToken(SecureToken token)
    {
        return token.ExpiresAt >= DateTime.UtcNow;
    }
}