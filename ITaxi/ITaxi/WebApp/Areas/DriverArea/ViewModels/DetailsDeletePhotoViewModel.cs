using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.DriverArea.ViewModels;

public class DetailsDeletePhotoViewModel
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(255)]
    [StringLength(255)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.DriverArea.Photo), Name = "Title")]
    public string Title { get; set; } = default!;

    
    
    [Required]
    [MaxLength(255)]
    [StringLength(255)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.DriverArea.Photo), Name = nameof(PhotoURL))]
    public string? PhotoURL { get; set; }
}