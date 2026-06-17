using UserManagementSystem.Application.Abstractions.Authentication;
using UserManagementSystem.Application.Abstractions.Authentication.Jwt;
using UserManagementSystem.Application.Abstractions.Authentication.Services;
using UserManagementSystem.Application.Abstractions.Mail;
using UserManagementSystem.Application.Abstractions.MessageEvents;
using UserManagementSystem.Application.Abstractions.Repositories;
using UserManagementSystem.Application.Services;
using UserManagementSystem.DataAccess.Repositories;
using UserManagementSystem.Domain.Abstractions;
using UserManagementSystem.Infrastructure.Authentication;
using UserManagementSystem.Infrastructure.Authentication.AccountConfirmationTokens;
using UserManagementSystem.Infrastructure.Authentication.Jwt;
using UserManagementSystem.Infrastructure.Authentication.RefreshTokens;
using UserManagementSystem.Infrastructure.Mail;
using UserManagementSystem.Infrastructure.Mail.Abstractions;
using UserManagementSystem.Infrastructure.MessageEvents.Publishers;

namespace UserManagementSystem.Main.Extensions.Services;

public static class DiServicesExtension
{
    public static void AddDiServices(this IServiceCollection services)
    {
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IAccountConfirmationTokenRepository, AccountConfirmationTokenRepository>();
        services.AddScoped<IRefreshTokensRepository, RefreshTokensRepository>();
        services.AddScoped<IAccountConfirmationTokenService, AccountConfirmationTokenService>();
        services.AddScoped<IRefreshTokensService, RefreshTokensService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ISecureTokenGenerator, SecureTokenGenerator>();
        services.AddScoped<IMailEventPublisher, MailEventPublisher>();
        services.AddScoped<IMailService, MailService>();
        services.AddScoped<ILinkBuilder, EmailBuilder>();
        services.AddScoped<IEmailBuilder, EmailBuilder>();
    }
}