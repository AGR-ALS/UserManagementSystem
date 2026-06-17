using System.Security.Authentication;
using UserManagementSystem.Application.Abstractions.Authentication;
using UserManagementSystem.Application.Abstractions.Authentication.Jwt;
using UserManagementSystem.Application.Abstractions.Authentication.Services;
using UserManagementSystem.Application.Abstractions.Repositories;
using UserManagementSystem.Application.Exceptions;
using UserManagementSystem.Domain.Abstractions;
using UserManagementSystem.Domain.Models;

namespace UserManagementSystem.Application.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokensService _refreshTokensService;
    private readonly IAccountConfirmationTokenService _accountConfirmationTokenService;

    public UsersService(
        IUsersRepository usersRepository, 
        IPasswordHasher passwordHasher, 
        IJwtTokenGenerator jwtTokenGenerator, 
        IRefreshTokensService refreshTokensService, 
        IAccountConfirmationTokenService accountConfirmationTokenService
        )
    {
        _usersRepository = usersRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokensService = refreshTokensService;
        _accountConfirmationTokenService = accountConfirmationTokenService;
    }
    
    public async Task<IEnumerable<User>> GetAllUsersSortedAsync(CancellationToken cancellationToken = default)
    {
        return await _usersRepository.GetAllUsersSortedAsync(cancellationToken);
    }

    public async Task<User> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _usersRepository.GetUserByIdAsync(id, cancellationToken);
        if (user == null)
        {
            throw new EntityNotFoundException("User was not found");
        }
        
        return user;
    }

    public async Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _usersRepository.GetUserByEmailAsync(email, cancellationToken);
        if (user == null)
        {
            throw new EntityNotFoundException("User was not found");
        }
        
        return user;
    }

    public async Task RegisterUserAsync(string username, string email, string password, CancellationToken cancellationToken = default)
    {
        
        if ((await _usersRepository.GetUserByEmailAsync(email, cancellationToken)) != null)
        {
            throw new DuplicateValueException("Email address already exists");
        }
        var hashedPassword = _passwordHasher.HashPassword(password);
        var user = new User(username, email, hashedPassword);
        try
        {
            await _usersRepository.AddUserAsync(user, cancellationToken);
        }
        catch (Exception e)
        {
            throw new EntityCreatingException(e.Message);
        }
    }

    public async Task<(string, string)> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var userEntity = await _usersRepository.GetUserByEmailAsync(email, cancellationToken);
        if (userEntity == null)
        {
            throw new EntityNotFoundException("User was not found");
        }

        var loginResult = _passwordHasher.VerifyHashedPassword(password, userEntity.PasswordHash);
        if (!loginResult)
        {
            throw new InvalidCredentialException("Invalid login or password");
        }

        var accessToken = _jwtTokenGenerator.GenerateJwtToken(userEntity);
        var refreshToken = await _refreshTokensService.CreateSecureTokenAsync(userEntity.Id, cancellationToken);
        
        return (accessToken, refreshToken);
    }

    public async Task<string> LoginAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        bool loginResult;
        try
        { 
            loginResult = await _refreshTokensService.ValidateSecureTokenAsync(refreshToken, cancellationToken);
        }
        catch (EntityNotFoundException e)
        {
            throw new UnauthorizedAccessException("Invalid refresh token");
        }
        if (!loginResult)
        {
            await _refreshTokensService.DeleteSecureTokenAsync(refreshToken, cancellationToken);
            throw new UnauthorizedAccessException("Invalid refresh token");
        }

        var refreshTokenModel = await _refreshTokensService.GetSecureTokenModelAsync(refreshToken, cancellationToken);
        var userEntity = await _usersRepository.GetUserByIdAsync(refreshTokenModel.UserId, cancellationToken);
        if (userEntity == null)
        {
            throw new EntityNotFoundException("User was not found");
        }
        var accessToken = _jwtTokenGenerator.GenerateJwtToken(userEntity);
        
        return accessToken;
    }

    public async Task DeleteUsersAsync(Guid[] ids, CancellationToken cancellationToken = default)
    {
        await _usersRepository.DeleteUsersAsync(ids, cancellationToken);
    }

    public async Task DeleteUnverifiedUsersAsync(CancellationToken cancellationToken = default)
    {
        await _usersRepository.DeleteUnverifiedUsersAsync(cancellationToken);
    }

    public async Task BlockUsersAsync(Guid[] ids, CancellationToken cancellationToken = default)
    {
        await _usersRepository.BlockUsersAsync(ids, cancellationToken);
    }

    public async Task UnblockUsersAsync(Guid[] ids, CancellationToken cancellationToken = default)
    {
        await _usersRepository.UnblockUsersAsync(ids, cancellationToken);
    }

    public async Task VerifyUserAsync(string token, CancellationToken cancellationToken = default)
    {
        var accountConfirmationToken = await _accountConfirmationTokenService.GetSecureTokenModelAsync(token, cancellationToken);
        if (accountConfirmationToken == null)
            throw new EntityNotFoundException("Account confirmation token was not found");
        await _usersRepository.VerifyUserAsync(accountConfirmationToken.UserId, cancellationToken);
    }

    public async Task UpdateUsersLastActivityTimeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var userEntity = await _usersRepository.GetUserByIdAsync(id, cancellationToken);
        if (userEntity == null)
        {
            throw new EntityCreatingException("User was not found");
        }
        await _usersRepository.UpdateUsersLastActivityTimeAsync(id, DateTime.UtcNow, cancellationToken);
    }
}