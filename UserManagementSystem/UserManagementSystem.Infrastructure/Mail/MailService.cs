using Microsoft.Extensions.Options;
using UserManagementSystem.Application.Abstractions.Authentication.Services;
using UserManagementSystem.Application.Abstractions.Mail;
using UserManagementSystem.Application.Abstractions.MessageEvents;
using UserManagementSystem.Application.Exceptions;
using UserManagementSystem.Domain.Abstractions;
using UserManagementSystem.Infrastructure.Mail.Abstractions;
using UserManagementSystem.Infrastructure.Mail.Content;

namespace UserManagementSystem.Infrastructure.Mail;

public class MailService : IMailService
{
    private readonly IUsersService _usersService;
    private readonly IAccountConfirmationTokenService _accountConfirmationTokenService;
    private readonly IMailEventPublisher _mailEventPublisher;
    private readonly IEmailBuilder _emailBuilder;
    private readonly ILinkBuilder _linkBuilder;
    private readonly AccountConfirmationEmailContent _emailContent;

    public MailService(
        IUsersService usersService, 
        IAccountConfirmationTokenService accountConfirmationTokenService, 
        IMailEventPublisher mailEventPublisher, 
        IOptions<AccountConfirmationEmailContent> emailContent,
        IEmailBuilder emailBuilder,
        ILinkBuilder linkBuilder
        )
    {
        _usersService = usersService;
        _accountConfirmationTokenService = accountConfirmationTokenService;
        _mailEventPublisher = mailEventPublisher;
        _emailBuilder = emailBuilder;
        _linkBuilder = linkBuilder;
        _emailContent = emailContent.Value;
    }
    public async Task SendVerificationEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var userEntity = await _usersService.GetUserByEmailAsync(email, cancellationToken);
        if(userEntity == null)
            throw new EntityCreatingException("User was not found");
        var tokenString = await _accountConfirmationTokenService.CreateSecureTokenAsync(userEntity.Id, cancellationToken);
        await _mailEventPublisher.SendMail(userEntity.Email, 
            _emailContent.Subject, 
            _emailBuilder.BuildMessage(_emailContent.Body, 
                _linkBuilder.BuildLink(_emailContent, tokenString)));
    }
}