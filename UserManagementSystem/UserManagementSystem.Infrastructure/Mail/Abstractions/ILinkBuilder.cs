using UserManagementSystem.Infrastructure.Mail.Content;

namespace UserManagementSystem.Infrastructure.Mail.Abstractions;

public interface ILinkBuilder
{
    string BuildLink(EmailContent emailContent, string token);
}