using UserManagementSystem.Domain.Models;
using UserManagementSystem.Domain.Models.Tokens;

namespace UserManagementSystem.Application.Abstractions.Repositories;

public interface ISecureTokenRepository<T> where T: SecureToken
{
    Task<T?> GetSecureTokenAsync(string token, CancellationToken cancellationToken);
    Task<string> CreateSecureTokenAsync(T secureToken, CancellationToken cancellationToken);
    Task DeleteSecureTokenAsync(string token, CancellationToken cancellationToken);
}