namespace UserManagementSystem.Infrastructure.Mail.Abstractions;

public interface IEmailBuilder
{
    string BuildMessage(params string[] strings);
}