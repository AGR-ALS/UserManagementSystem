using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Application.Abstractions.Repositories;
using UserManagementSystem.DataAccess.Context;
using UserManagementSystem.DataAccess.Entities;
using UserManagementSystem.Domain.Models;
using UserManagementSystem.Domain.Models.Tokens;

namespace UserManagementSystem.DataAccess.Repositories;

public class AccountConfirmationTokenRepository : IAccountConfirmationTokenRepository
{
    private readonly UserManagementSystemContext _dbContext;
    private readonly IMapper _mapper;

    public AccountConfirmationTokenRepository(UserManagementSystemContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<string> CreateSecureTokenAsync(AccountConfirmationToken refreshToken, CancellationToken cancellationToken)
    {
        var accountConfirmationTokenEntity = _mapper.Map<AccountConfirmationTokenEntity>(refreshToken);
        await _dbContext.AddAsync(accountConfirmationTokenEntity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return accountConfirmationTokenEntity.Token;
    }

    public async Task DeleteSecureTokenAsync(string token, CancellationToken cancellationToken)
    {
        await _dbContext.AccountConfirmationTokens.Where(r=>r.Token == token).ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<AccountConfirmationToken?> GetSecureTokenAsync(string token, CancellationToken cancellationToken)
    {
        var accountConfirmationTokenEntity = await _dbContext.AccountConfirmationTokens.FirstOrDefaultAsync(r => r.Token == token, cancellationToken);
       
        return accountConfirmationTokenEntity == null ? null : _mapper.Map<AccountConfirmationToken>(accountConfirmationTokenEntity);
    }
}