namespace WebApp.Helpers;
/// <summary>
/// Class for mail settings
/// </summary>
public class MailSettings
{
    /// <summary>
    /// Mail
    /// </summary>
    public string Mail { get; set; } = default!;
    
    /// <summary>
    /// Display name
    /// </summary>
    
    public string DisplayName { get; set; } = default!;
    
    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set; } = default!;
    
    /// <summary>
    /// Host
    /// </summary>
    public string Host { get; set; } = default!;
    
    /// <summary>
    /// Port
    /// </summary>
    public int Port { get; set; }
}