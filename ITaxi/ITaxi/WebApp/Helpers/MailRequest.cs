namespace WebApp.Helpers;

/// <summary>
/// Mail request
/// </summary>
public class MailRequest
{
    /// <summary>
    /// Email recipient
    /// </summary>
    public string ToEmail { get; set; } = default!;
    
    /// <summary>
    /// Email subject
    /// </summary>
    public string Subject { get; set; } = default!;
    
    /// <summary>
    /// Email body
    /// </summary>
    public string Body { get; set; } = default!;
    
    /// <summary>
    /// Email attachments
    /// </summary>
    public List<IFormFile>? Attachments { get; set; }
}