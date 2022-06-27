using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Domain;
using Base.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateEditCityViewModel
{
    public Guid Id { get; set; }
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.City), Name = "CountyName")]
    
    public Guid? CountyId { get; set; }
    
    public SelectList? Counties { get; set; }

    [Required(ErrorMessageResourceType= typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage") ]
    [StringLength(50, MinimumLength = 1)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.City), Name = "CityName")]
    public string CityName { get; set; } = default!;
}