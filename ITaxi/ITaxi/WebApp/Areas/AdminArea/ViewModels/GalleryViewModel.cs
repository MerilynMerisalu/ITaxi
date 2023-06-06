namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Gallery view model
/// </summary>
public class GalleryViewModel
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Vehicle identifier
    /// </summary>
    public string VehicleIdentifier { get; set; } = default!;
}