using System.ComponentModel.DataAnnotations;
using Base.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.DriverArea.ViewModels;

/// <summary>
/// Create edit photo view model
/// </summary>
public class CreateEditPhotoViewModel
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Photo title
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(255, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(255, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.DriverArea.Photo),
        Name = nameof(Title))]
    public string Title { get; set; } = default!;
    
    /// <summary>
    /// Photo url
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(255, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(255, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.DriverArea.Photo), Name = nameof(PhotoURL))]
    public string? PhotoURL { get; set; }
    
    /// <summary>
    /// Vehicle id
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.DriverArea.Photo), Name = "Vehicle")]
    public Guid VehicleId { get; set; }
    
    /// <summary>
    /// List of vehicles
    /// </summary>
    public SelectList? Vehicles { get; set; }
}