using UserManagementSystem.Domain.Models;
using UserManagementSystem.Domain.Models.Tokens;

namespace UserManagementSystem.Application.Abstractions.Authentication.Services;

public interface ISecureTokenService<T> where T: SecureToken
{
    Task<string> CreateSecureTokenAsync(Guid userId, CancellationToken cancellationToken);
    Task DeleteSecureTokenAsync(string token, CancellationToken cancellationToken);
    Task<bool> ValidateSecureTokenAsync(string token, CancellationToken cancellationToken);
    Task <T> GetSecureTokenModelAsync(string token, CancellationToken cancellationToken);
}