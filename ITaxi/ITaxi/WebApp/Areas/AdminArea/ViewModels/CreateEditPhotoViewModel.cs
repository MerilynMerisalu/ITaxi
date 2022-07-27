using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateEditPhotoViewModel
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(255)]
    [StringLength(255)]
    public string Title { get; set; } = default!;

    [Required]
    [MaxLength(255)]
    [StringLength(255)]
    [Display(Name = "Photo Name")]
    public string? PhotoName { get; set; }
}