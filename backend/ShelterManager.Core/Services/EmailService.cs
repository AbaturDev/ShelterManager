using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using ShelterManager.Core.Options;
using ShelterManager.Core.Services.Abstractions;

namespace ShelterManager.Core.Services;

public class EmailService : IEmailService
{
    private readonly IOptions<EmailOptions> _options;

    public EmailService(IOptions<EmailOptions> options)
    {
        _options = options;
    }
    
    public async Task SendEmailAsync(string toEmail, string toName, string subject, string htmlBody)
    {
        var message = new MimeMessage();
        
        message.From.Add(new MailboxAddress(_options.Value.From.Name, _options.Value.From.Email));
        message.To.Add(new MailboxAddress(toName, toEmail));
        
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = htmlBody
        };
        message.Body = bodyBuilder.ToMessageBody();
        
        using var client = new SmtpClient();

        await client.ConnectAsync(_options.Value.Host, _options.Value.Port);
        await client.AuthenticateAsync(_options.Value.UserName, _options.Value.Password);

        await client.SendAsync(message);
        
        await client.DisconnectAsync(true);

    }
}