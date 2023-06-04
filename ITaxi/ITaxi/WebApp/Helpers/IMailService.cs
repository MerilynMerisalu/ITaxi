namespace WebApp.Helpers;

/// <summary>
/// Mail service interface
/// </summary>
public interface IMailService
{
    /// <summary>
    /// Mail request
    /// </summary>
    /// <param name="mailRequest">Email request</param>
    /// <returns>String</returns>
    Task<string> SendEmailAsync(MailRequest mailRequest);
}