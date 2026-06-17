using Microsoft.Extensions.Options;
using UserManagementSystem.Application.Abstractions.Authentication;
using UserManagementSystem.Application.Abstractions.Authentication.Services;
using UserManagementSystem.Application.Abstractions.Repositories;
using UserManagementSystem.Application.Exceptions;
using UserManagementSystem.Domain.Models;
using UserManagementSystem.Domain.Models.Tokens;

namespace UserManagementSystem.Infrastructure.Authentication.RefreshTokens;

public class RefreshTokensService : IRefreshTokensService
{
    private readonly ISecureTokenGenerator _secureTokenGenerator;
    private readonly IRefreshTokensRepository _refreshTokensRepository;
    private readonly RefreshTokenSettings _refreshTokenSettings;

    public RefreshTokensService(ISecureTokenGenerator secureTokenGenerator, IRefreshTokensRepository refreshTokensRepository, IOptions<RefreshTokenSettings> refreshTokenSettings)
    {
        _secureTokenGenerator = secureTokenGenerator;
        _refreshTokensRepository = refreshTokensRepository;
        _refreshTokenSettings = refreshTokenSettings.Value;
    }

    public async Task<string> CreateSecureTokenAsync(Guid userId, CancellationToken cancellationToken)
    {
        var token = new RefreshToken(
            _secureTokenGenerator.GenerateToken(),
            userId,
            DateTime.UtcNow.AddDays(_refreshTokenSettings.ExpiresInDays)
        );
        
        return await _refreshTokensRepository.CreateSecureTokenAsync(token, cancellationToken);
    }

    public async Task DeleteSecureTokenAsync(string token, CancellationToken cancellationToken)
    {
        await _refreshTokensRepository.DeleteSecureTokenAsync(token, cancellationToken);
    }

    public async Task<bool> ValidateSecureTokenAsync(string token, CancellationToken cancellationToken)
    {
        var tokenEntity = await _refreshTokensRepository.GetSecureTokenAsync(token, cancellationToken);
        if (tokenEntity == null)
        {
            throw new EntityNotFoundException("Token entity was not found");
        }

        return _secureTokenGenerator.VerifyToken(tokenEntity);
    }

    public async Task<RefreshToken> GetSecureTokenModelAsync(string token, CancellationToken cancellationToken)
    {
        var tokenEntity = await _refreshTokensRepository.GetSecureTokenAsync(token, cancellationToken);
        if (tokenEntity == null)
        {
            throw new EntityNotFoundException("Token entity was not found");
        }

        return tokenEntity;
    }
}