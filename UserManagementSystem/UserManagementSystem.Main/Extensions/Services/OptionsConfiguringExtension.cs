using UserManagementSystem.Infrastructure.Authentication.AccountConfirmationTokens;
using UserManagementSystem.Infrastructure.Authentication.Jwt;
using UserManagementSystem.Infrastructure.Authentication.RefreshTokens;
using UserManagementSystem.Infrastructure.Authentication.Tokens.Settings;
using UserManagementSystem.Infrastructure.Mail.Content;

namespace UserManagementSystem.Main.Extensions.Services;

public static class OptionsConfiguringExtension
{
    public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        services.Configure<RefreshTokenSettings>(configuration.GetSection(nameof(RefreshTokenSettings)));
        services.Configure<AccountConfirmationTokenSettings>(
            configuration.GetSection(nameof(AccountConfirmationTokenSettings)));
        services.Configure<AccountConfirmationEmailContent>(configuration.GetSection(nameof(AccountConfirmationEmailContent)));
        services.Configure<TokenIdentifiers>(configuration.GetSection(nameof(TokenIdentifiers)));
    }
}