using System.Net;
using System.Net.Mail;

namespace SWBackend.ServiceLayer.Auth.Mail;

public class SmtpEmailSender : IEmailerSender
{
    private readonly IConfiguration _configuration;

    public SmtpEmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var smtpClient = new SmtpClient
        {
            Host = _configuration["Smtp:Host"],
            Port = int.Parse(_configuration["Smtp:Port"]),
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(
                _configuration["Smtp:Username"],
                _configuration["Smtp:Password"]
            )
        };
        var message = new MailMessage
        {
            From = new MailAddress(_configuration["Smtp:From"]),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true
        };
        message.To.Add(email);

        await smtpClient.SendMailAsync(message);
    }
}