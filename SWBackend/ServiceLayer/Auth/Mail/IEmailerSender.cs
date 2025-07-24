namespace SWBackend.ServiceLayer.Mail;

public interface IEmailerSender
{
    Task SendEmailAsync(string email, string subject, string htmlMessage);
}