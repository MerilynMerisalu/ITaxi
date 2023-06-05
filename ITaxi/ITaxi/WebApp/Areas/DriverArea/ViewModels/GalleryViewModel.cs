namespace WebApp.Areas.DriverArea.ViewModels;

/// <summary>
/// Gallery view model
/// </summary>
public class GalleryViewModel
{
    /// <summary>
    /// Gallery view model id
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Vehicle identifier
    /// </summary>
    public string VehicleIdentifier { get; set; } = default!;
}