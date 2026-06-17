using UserManagementSystem.Domain.Models;
using UserManagementSystem.Domain.Models.Tokens;

namespace UserManagementSystem.Application.Abstractions.Repositories;

public interface IRefreshTokensRepository : ISecureTokenRepository<RefreshToken>
{
}