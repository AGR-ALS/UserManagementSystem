using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Application.Abstractions.Repositories;
using UserManagementSystem.DataAccess.Context;
using UserManagementSystem.DataAccess.Entities;
using UserManagementSystem.Domain.Models;
using UserManagementSystem.Domain.Models.Tokens;

namespace UserManagementSystem.DataAccess.Repositories;

public class RefreshTokensRepository : IRefreshTokensRepository
{
    private readonly UserManagementSystemContext _dbContext;
    private readonly IMapper _mapper;

    public RefreshTokensRepository(UserManagementSystemContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<string> CreateSecureTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        var refreshTokenEntity = _mapper.Map<RefreshTokenEntity>(refreshToken);
        await _dbContext.AddAsync(refreshTokenEntity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return refreshTokenEntity.Token;
    }

    public async Task DeleteSecureTokenAsync(string token, CancellationToken cancellationToken)
    {
        await _dbContext.RefreshTokens.Where(r=>r.Token == token).ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<RefreshToken?> GetSecureTokenAsync(string token, CancellationToken cancellationToken)
    {
        var refreshTokenEntity = await _dbContext.RefreshTokens.FirstOrDefaultAsync(r => r.Token == token, cancellationToken);
        
        return refreshTokenEntity == null ? null : _mapper.Map<RefreshToken>(refreshTokenEntity);
    }
}