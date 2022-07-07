using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Domain;
using App.Domain.Enum;
using Base.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

public class EditDriverViewModel
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "StringLengthAttributeErrorMessage"),]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Driver), Name = "FirstName")]
    public string FirstName { get; set; } = default!;
            
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Driver), Name = "LastName")]
    public string LastName { get; set; } = default!;
    
    [EnumDataType(typeof(Gender))] 
    [Display(ResourceType = typeof(Common), Name = "Gender")]
    public Gender Gender { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [DataType(DataType.Date)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Driver), Name = "DateOfBirth")]
    [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:d}")]
    public DateTime DateOfBirth { get; set; } = default!;

    
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

   [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Driver), Name = "DriverLicenseCategories")] 
    public ICollection<Guid>? DriverAndDriverLicenseCategories { get; set; }

    [DataType(DataType.Date)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Driver), Name = "DriverLicenseExpiryDate")]
    [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:d}")]
    #warning DateTime input control does not support user changing the language yet
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