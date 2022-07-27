using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteDisabilityTypeViewModel : AdminAreaBaseViewModel
{
    public Guid Id { get; set; }

    [Display(ResourceType = typeof(DisabilityType), Name = "DisabilityTypeName")]
    public string DisabilityType { get; set; } = default!;
}