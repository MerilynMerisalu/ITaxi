using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Details delete photo view model
/// </summary>
public class DetailsDeletePhotoViewModel: AdminAreaBaseViewModel
{
    /// <summary>
    /// Photo id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Photo title
    /// </summary>
    [Required]
    [MaxLength(255)]
    [StringLength(255)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(Title))]
    public string Title { get; set; } = default!;

    /// <summary>
    /// Photo name
    /// </summary>
    [Required]
    [MaxLength(255)]
    [StringLength(255)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(PhotoName))]
    public string? PhotoName { get; set; }
    
    /// <summary>
    /// Photo url
    /// </summary>
    [Required]
    [MaxLength(255)]
    [StringLength(255)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(PhotoURL))]
    public string? PhotoURL { get; set; }
}