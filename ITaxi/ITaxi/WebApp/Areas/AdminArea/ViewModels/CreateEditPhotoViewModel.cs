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

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = "Driver")]
    public Guid? DriverId { get; set; }
    
    /// <summary>
    /// Photo name
    /// </summary>
    [Required]
    [MaxLength(255)]
    [StringLength(255)]
    [Display(Name = "Photo Name")]
    public string? PhotoName { get; set; }

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(PhotoURL))]
    public string PhotoURL { get; set; }




}