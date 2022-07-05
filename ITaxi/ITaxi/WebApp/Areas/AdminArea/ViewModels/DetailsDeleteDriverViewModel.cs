using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Domain.Enum;
using Base.Resources;
using App.Resources.Areas.App.Domain.AdminArea;
namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteDriverViewModel
{
    public Guid Id { get; set; }

    [Display(ResourceType = typeof(Driver), Name = "FirstName")]
    
    public string FirstName { get; set; } = default!;
   
    [Display(ResourceType = typeof(Driver), Name = "LastName")]
    public string LastName { get; set; } = default!;
    
    [Display(ResourceType = typeof(Driver), Name = "LastAndFirstName")]
    public string LastAndFirstName { get; set; } = default!;

    [Display(ResourceType = typeof(Common), Name = "Gender")]
    public Gender Gender { get; set; } 
    
    [Display(ResourceType = typeof(Driver), Name = "PersonalIdentifier")]
    public string? PersonalIdentifier { get; set; } 
    
    [Display(ResourceType = typeof(Driver), Name = "DriverLicenseNumber")]
    public string DriverLicenseNumber { get; set; } = default!;

    [Display(ResourceType = typeof(Driver), Name = "DriverLicenseCategories")]
    public string? DriverLicenseCategoryNames { get; set; }
    
    [Display(ResourceType = typeof(Driver), Name = "DriverLicenseExpiryDate")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime DriverLicenseExpiryDate { get; set; }
    
    
    [Display(ResourceType = typeof(Driver), Name = "City")] 
    public string CityName { get; set; } = default!;

    [Display(ResourceType = typeof(Driver), Name = "Address")]
    public string Address { get; set; } = default!;

    [Display(ResourceType = typeof(Common), Name = "PhoneNumber")]
    public string PhoneNumber { get; set; } = default!;

    [Display(ResourceType = typeof(Common), Name = "Email")]
    
    public string EmailAddress { get; set; } = default!;


}