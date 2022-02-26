using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateEditCityViewModel
{
    public Guid Id { get; set; }
    [DisplayName("County Name")]
    public Guid CountyId { get; set; }
    public SelectList? Counties { get; set; }

    [StringLength(50, MinimumLength = 1)]
    [DisplayName("City Name")]
    public string CityName { get; set; } = default!;
}