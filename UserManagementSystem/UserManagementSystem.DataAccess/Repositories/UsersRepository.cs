using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Application.Abstractions.Repositories;
using UserManagementSystem.DataAccess.Context;
using UserManagementSystem.DataAccess.Entities;
using UserManagementSystem.Domain.Models;

namespace UserManagementSystem.DataAccess.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly UserManagementSystemContext _dbContext;
    private readonly IMapper _mapper;

    public UsersRepository(UserManagementSystemContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<User>> GetAllUsersSortedAsync(CancellationToken cancellationToken = default)
    {
        return _mapper.Map<List<User>>(await _dbContext.Users.OrderByDescending(x => x.LastActivityTime).ToListAsync(cancellationToken));
    }

    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var userEntity = await _dbContext.Users.FindAsync([id], cancellationToken);
        
        return userEntity == null ? null : _mapper.Map<User>(userEntity);
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var userEntity = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        
        return userEntity == null ? null : _mapper.Map<User>(userEntity);
    }

    public async Task<Guid> AddUserAsync(User user, CancellationToken cancellationToken = default)
    {
        var userEntity = _mapper.Map<UserEntity>(user);
        await _dbContext.Users.AddAsync(userEntity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return user.Id;
    }

    public async Task DeleteUsersAsync(Guid[] ids, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users.Where(x=>ids.Contains(x.Id))
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task DeleteUnverifiedUsersAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.Users.Where(x=>x.VerificationStatus == false)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task BlockUsersAsync(Guid[] ids, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users
            .Where(x=>ids.Contains(x.Id))
            .ExecuteUpdateAsync(x => x
                .SetProperty(p => p.ActivityStatus, false), cancellationToken);
    }

    public async Task UnblockUsersAsync(Guid[] ids, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users
            .Where(x=>ids.Contains(x.Id))
            .ExecuteUpdateAsync(x => x
                .SetProperty(p => p.ActivityStatus, true), cancellationToken);
    }

    public async Task VerifyUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x
                .SetProperty(p => p.VerificationStatus, true), cancellationToken);
    }

    public async Task UpdateUsersLastActivityTimeAsync(Guid id, DateTime time, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users
            .Where(x=>x.Id == id)
            .ExecuteUpdateAsync(x => x
                .SetProperty(p=>p.LastActivityTime, time), cancellationToken);
    }
}   