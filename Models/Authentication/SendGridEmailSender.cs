using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Options;
using ERP_System_Project.Models.Authentication;
using ERP_System_Project.Services.Interfaces;

public class SendGridEmailSender : IEmailService
{
    private readonly SendGridSettings _settings;

    public SendGridEmailSender(IOptions<SendGridSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var client = new SendGridClient(_settings.ApiKey);
        var from = new EmailAddress(_settings.SenderEmail, _settings.SenderName);
        var to = new EmailAddress(email);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
        var response = await client.SendEmailAsync(msg);
    }
}
