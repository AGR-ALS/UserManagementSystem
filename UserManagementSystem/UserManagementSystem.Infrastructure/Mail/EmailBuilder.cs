using System.Net;
using System.Text;
using UserManagementSystem.Infrastructure.Mail.Abstractions;
using UserManagementSystem.Infrastructure.Mail.Content;

namespace UserManagementSystem.Infrastructure.Mail;

public class EmailBuilder : IEmailBuilder, ILinkBuilder
{
    public string BuildMessage(params string[] strings)
    {
        var messageBuilder = new StringBuilder();
        foreach (var str in strings)
        {
            messageBuilder.Append(str);
            messageBuilder.Append('\n');
        }
        
        return messageBuilder.ToString();
    }

    public string BuildLink(EmailContent emailContent, string token)
    {
        return $"{emailContent.Link}?token={WebUtility.UrlEncode(token)}";
    }
}