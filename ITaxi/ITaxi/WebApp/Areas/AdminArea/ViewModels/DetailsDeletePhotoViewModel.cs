using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeletePhotoViewModel: AdminAreaBaseViewModel
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(255)]
    [StringLength(255)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(Title))]
    public string Title { get; set; } = default!;

    [Required]
    [MaxLength(255)]
    [StringLength(255)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(PhotoName))]
    public string? PhotoName { get; set; }
    
    [Required]
    [MaxLength(255)]
    [StringLength(255)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(PhotoURL))]
    public string? PhotoURL { get; set; }
}