using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain;

namespace WebApp.Areas.AdminArea.ViewModels;

public class AdminAreaBaseViewModel
{
    [Display(ResourceType = typeof(Common), Name = nameof(CreatedAt))]
    public string CreatedAt { get; set; } = default!;
    [Display(ResourceType = typeof(Common), Name = nameof(CreatedBy))]
    public string CreatedBy { get; set; } = default!;
    [Display(ResourceType = typeof(Common), Name = nameof(UpdatedAt))]
    public string UpdatedAt { get; set; } = default!;
    [Display(ResourceType = typeof(Common), Name = nameof(UpdatedBy))]
    public string UpdatedBy { get; set; } = default!;
}