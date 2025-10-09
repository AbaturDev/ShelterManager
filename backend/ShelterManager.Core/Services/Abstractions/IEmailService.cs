namespace ShelterManager.Core.Services.Abstractions;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string toName, string subject, string htmlBody);
}