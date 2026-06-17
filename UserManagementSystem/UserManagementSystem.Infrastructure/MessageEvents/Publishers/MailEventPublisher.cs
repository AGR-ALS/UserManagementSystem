using MassTransit;
using SharedResources.MessageContracts;
using UserManagementSystem.Application.Abstractions.MessageEvents;

namespace UserManagementSystem.Infrastructure.MessageEvents.Publishers;

public class MailEventPublisher : IMailEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MailEventPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task SendMail(string email, string subject, string body)
    {
        return _publishEndpoint.Publish(
            new MailSendingEvent(email, subject, body));
    }
}