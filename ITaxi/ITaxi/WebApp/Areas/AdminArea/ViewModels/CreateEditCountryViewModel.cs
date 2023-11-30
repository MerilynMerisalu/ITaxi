using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateEditCountryViewModel: AdminAreaBaseViewModel
{
    public Guid Id { get; set; }

    [Display(ResourceType = typeof(Country), Name = nameof(CountryName))]
    public string CountryName { get; set; } = default!;
}