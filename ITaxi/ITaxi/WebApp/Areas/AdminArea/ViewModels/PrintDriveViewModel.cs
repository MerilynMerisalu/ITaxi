using App.Domain;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Print drive view model
/// </summary>
public class PrintDriveViewModel
{
    /// <summary>
    /// Driver name
    /// </summary>
    public string DriverName { get; set; } = default!;
    
    /// <summary>
    /// Drives
    /// </summary>
    public IEnumerable<Drive>? Drives { get; set; }
}