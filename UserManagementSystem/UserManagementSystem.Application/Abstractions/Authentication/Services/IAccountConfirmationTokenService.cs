using UserManagementSystem.Domain.Models;
using UserManagementSystem.Domain.Models.Tokens;

namespace UserManagementSystem.Application.Abstractions.Authentication.Services;

public interface IAccountConfirmationTokenService : ISecureTokenService<AccountConfirmationToken>
{
    
}