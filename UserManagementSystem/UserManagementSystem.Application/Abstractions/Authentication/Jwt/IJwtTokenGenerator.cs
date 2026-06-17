using UserManagementSystem.Domain.Models;

namespace UserManagementSystem.Application.Abstractions.Authentication.Jwt;

public interface IJwtTokenGenerator
{
    string GenerateJwtToken(User user);
}