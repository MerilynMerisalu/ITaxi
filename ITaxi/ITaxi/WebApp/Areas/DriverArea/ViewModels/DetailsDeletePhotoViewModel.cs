using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.DriverArea.ViewModels;

/// <summary>
/// Details delete photo view model
/// </summary>
public class DetailsDeletePhotoViewModel
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Photo title
    /// </summary>
    [Required]
    [MaxLength(255)]
    [StringLength(255)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.DriverArea.Photo), Name = "Title")]
    public string Title { get; set; } = default!;
    
    /// <summary>
    /// Photo url
    /// </summary>
    [Required]
    [MaxLength(255)]
    [StringLength(255)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.DriverArea.Photo), Name = nameof(PhotoURL))]
    public string? PhotoURL { get; set; }
}