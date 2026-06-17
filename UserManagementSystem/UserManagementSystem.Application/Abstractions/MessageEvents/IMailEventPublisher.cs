namespace UserManagementSystem.Application.Abstractions.MessageEvents;

public interface IMailEventPublisher
{
    Task SendMail(string email, string subject, string body);
}