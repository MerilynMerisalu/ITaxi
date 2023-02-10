using System.ComponentModel.DataAnnotations;
using Base.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.DriverArea.ViewModels;

public class CreateEditPhotoViewModel
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(255, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(255, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.DriverArea.Photo),
        Name = nameof(Title))]
    public string Title { get; set; } = default!;
    
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(255, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(255, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.DriverArea.Photo), Name = nameof(PhotoURL))]
    public string? PhotoURL { get; set; }
    

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.DriverArea.Photo), Name = "Vehicle")]

    public Guid VehicleId { get; set; }
    
    public SelectList? Vehicles { get; set; }

    
}