namespace UserManagementSystem.Application.Abstractions.Mail;

public interface IMailService
{
    Task SendVerificationEmailAsync(string email, CancellationToken cancellationToken = default);
}