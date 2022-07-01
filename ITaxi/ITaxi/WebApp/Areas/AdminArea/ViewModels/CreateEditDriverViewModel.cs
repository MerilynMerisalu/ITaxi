using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Domain;
using Base.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateEditDriverViewModel
{
    public Guid Id { get; set; }
    
    [StringLength(25, MinimumLength = 0, ErrorMessageResourceType = typeof(Common), 
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Driver),
        Name = nameof(PersonalIdentifier))]
    public string? PersonalIdentifier { get; set; } 
    
    [StringLength(15, MinimumLength = 2, ErrorMessageResourceType = typeof(Common)
    , ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Driver), 
        Name = "DriverLicenseNumber")]
    public string DriverLicenseNumber { get; set; } = default!;
    
    
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Driver), Name = "DriverLicenseCategories")]

    public SelectList? DriverLicenseCategories { get; set; }

    
    public ICollection<Guid>? DriverAndDriverLicenseCategories { get; set; }

    [DataType(DataType.Date)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Driver), Name = "DriverLicenseNumber")]
    [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime DriverLicenseExpiryDate { get; set; }

    public SelectList? Cities { get; set; }

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Driver), Name = "City")]
    public Guid CityId { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(30, MinimumLength = 1, ErrorMessageResourceType = typeof(Common), 
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Driver), Name = "Address")]
    public string Address { get; set; } = default!;

}