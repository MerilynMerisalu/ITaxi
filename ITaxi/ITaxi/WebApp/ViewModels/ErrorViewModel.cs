namespace WebApp.ViewModels;

/// <summary>
/// Error View Model class
/// </summary>
public class ErrorViewModel
{
    /// <summary>
    /// Id of an request
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Boolean value of an boolean expression show request id
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}