namespace SWBackend.ServiceLayer.Auth.Mail;

public interface IEmailerSender
{
    Task SendEmailAsync(string email, string subject, string htmlMessage);
}