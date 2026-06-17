using Microsoft.Extensions.Options;
using UserManagementSystem.Application.Abstractions.Authentication;
using UserManagementSystem.Application.Abstractions.Authentication.Services;
using UserManagementSystem.Application.Abstractions.Repositories;
using UserManagementSystem.Application.Exceptions;
using UserManagementSystem.Domain.Models;
using UserManagementSystem.Domain.Models.Tokens;

namespace UserManagementSystem.Infrastructure.Authentication.AccountConfirmationTokens;

public class AccountConfirmationTokenService : IAccountConfirmationTokenService
{
    private readonly IAccountConfirmationTokenRepository _accountConfirmationTokenRepository;
    private readonly ISecureTokenGenerator _secureTokenGenerator;
    private readonly AccountConfirmationTokenSettings _accountConfirmationTokenSettings;

    public AccountConfirmationTokenService(IOptions<AccountConfirmationTokenSettings> accountConfirmationTokenSettings, IAccountConfirmationTokenRepository accountConfirmationTokenRepository, ISecureTokenGenerator secureTokenGenerator)
    {
        _accountConfirmationTokenRepository = accountConfirmationTokenRepository;
        _secureTokenGenerator = secureTokenGenerator;
        _accountConfirmationTokenSettings = accountConfirmationTokenSettings.Value;
    }
    public async Task<string> CreateSecureTokenAsync(Guid userId, CancellationToken cancellationToken)
    {
        var token = new AccountConfirmationToken(
            _secureTokenGenerator.GenerateToken(),
            userId,
            DateTime.UtcNow.AddMinutes(_accountConfirmationTokenSettings.ExpiresInMinutes)
            );
        var tokenString = await _accountConfirmationTokenRepository.CreateSecureTokenAsync(token, cancellationToken);
        
        return tokenString;
    }

    public async Task DeleteSecureTokenAsync(string token, CancellationToken cancellationToken)
    {
        await _accountConfirmationTokenRepository.DeleteSecureTokenAsync(token, cancellationToken);
    }

    public async Task<bool> ValidateSecureTokenAsync(string token, CancellationToken cancellationToken)
    {
        var tokenEntity = await _accountConfirmationTokenRepository.GetSecureTokenAsync(token, cancellationToken);
        if (tokenEntity == null)
        {
            throw new EntityNotFoundException("Token entity was not found");
        }

        return _secureTokenGenerator.VerifyToken(tokenEntity);
    }

    public async Task<AccountConfirmationToken> GetSecureTokenModelAsync(string token, CancellationToken cancellationToken)
    {
        var tokenEntity = await _accountConfirmationTokenRepository.GetSecureTokenAsync(token, cancellationToken);
        if (tokenEntity == null)
        {
            throw new EntityNotFoundException("Token entity was not found");
        }

        return tokenEntity;
    }
}