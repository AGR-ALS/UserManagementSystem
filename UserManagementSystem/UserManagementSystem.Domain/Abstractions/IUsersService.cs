using UserManagementSystem.Domain.Models;

namespace UserManagementSystem.Domain.Abstractions;

public interface IUsersService
{
    Task<IEnumerable<User>> GetAllUsersSortedAsync(CancellationToken cancellationToken = default);
    Task RegisterUserAsync(string username, string email, string password, CancellationToken cancellationToken = default);
    Task<(string, string)> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<string> LoginAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<User> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task DeleteUsersAsync(Guid[] ids, CancellationToken cancellationToken = default);
    Task DeleteUnverifiedUsersAsync(CancellationToken cancellationToken = default);
    Task BlockUsersAsync(Guid[] ids, CancellationToken cancellationToken = default);
    Task UnblockUsersAsync(Guid[] ids, CancellationToken cancellationToken = default);
    Task VerifyUserAsync(string token, CancellationToken cancellationToken = default);
    Task UpdateUsersLastActivityTimeAsync(Guid id, CancellationToken cancellationToken = default);
}