using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Create edit photo view model
/// </summary>
public class CreateEditPhotoViewModel
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
    public string Title { get; set; } = default!;

    /// <summary>
    /// Photo name
    /// </summary>
    [Required]
    [MaxLength(255)]
    [StringLength(255)]
    [Display(Name = "Photo Name")]
    public string? PhotoName { get; set; }
}