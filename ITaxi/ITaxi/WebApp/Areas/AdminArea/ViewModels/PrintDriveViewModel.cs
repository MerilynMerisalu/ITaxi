using App.Domain;

namespace WebApp.Areas.AdminArea.ViewModels;

public class PrintDriveViewModel
{
    
    public string DriverName { get; set; } = default!;
    public IEnumerable<Drive>? Drives { get; set; }

}