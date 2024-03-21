using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Create edit city view model
/// </summary>
public class CreateEditCityViewModel
{
    /// <summary>
    /// City id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// County id
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(City), Name = "CountyName")]
    public Guid CountyId { get; set; }
    
    /// <summary>
    /// List of counties
    /// </summary>
    public SelectList? Counties { get; set; }

    /// <summary>
    /// City name
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(50, MinimumLength = 1)]
    [Display(ResourceType = typeof(City), Name = "CityName")]
    public string CityName { get; set; } = default!;
}