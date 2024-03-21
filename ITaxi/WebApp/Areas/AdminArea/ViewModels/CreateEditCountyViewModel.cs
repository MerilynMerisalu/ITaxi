using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Create edit county view model
/// </summary>
public class CreateEditCountyViewModel
{
    /// <summary>
    /// County id
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Country id
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(County), Name = "Country")]
    public Guid CountryId { get; set; }

    /// <summary>
    /// List of counties
    /// </summary>
    public SelectList? Countries { get; set; }

    /// <summary>
    /// County name
    /// </summary>
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Required(ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(County),
        Name = nameof(CountyName))]
    public string CountyName { get; set; } = default!;
}