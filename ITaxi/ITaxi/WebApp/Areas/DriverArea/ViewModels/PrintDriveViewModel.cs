using App.BLL.DTO.AdminArea;
using App.Domain;

namespace WebApp.Areas.DriverArea.ViewModels;

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
    public IEnumerable<DriveDTO>? Drives { get; set; }
}