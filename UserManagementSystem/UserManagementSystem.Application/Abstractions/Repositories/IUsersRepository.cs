using UserManagementSystem.Domain.Models;

namespace UserManagementSystem.Application.Abstractions.Repositories;

public interface IUsersRepository
{
    Task<IEnumerable<User>> GetAllUsersSortedAsync(CancellationToken cancellationToken);
    Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<Guid> AddUserAsync(User user, CancellationToken cancellationToken);
    Task DeleteUsersAsync(Guid[] ids, CancellationToken cancellationToken);
    Task DeleteUnverifiedUsersAsync(CancellationToken cancellationToken);
    Task BlockUsersAsync(Guid[] ids, CancellationToken cancellationToken);
    Task UnblockUsersAsync(Guid[] ids, CancellationToken cancellationToken);
    Task VerifyUserAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateUsersLastActivityTimeAsync(Guid id, DateTime time, CancellationToken cancellationToken);
}