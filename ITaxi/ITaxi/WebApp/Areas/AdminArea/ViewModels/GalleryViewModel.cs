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

    public IFormFile? Photo1 { get; set; }
    public IFormFile? Photo2 { get; set; }
    public IFormFile? Photo3 { get; set; }
    public IFormFile? Photo4 { get; set; }
}