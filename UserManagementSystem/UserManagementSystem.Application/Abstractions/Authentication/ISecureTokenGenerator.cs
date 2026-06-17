using UserManagementSystem.Domain.Models;
using UserManagementSystem.Domain.Models.Tokens;

namespace UserManagementSystem.Application.Abstractions.Authentication;

public interface ISecureTokenGenerator
{
    string GenerateToken();
    bool VerifyToken(SecureToken token);
}