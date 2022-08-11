namespace WebApp.Helpers;

public interface IMailService
{
    Task<string> SendEmailAsync(MailRequest mailRequest);
}