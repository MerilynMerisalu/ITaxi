using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;
using Base.Resources;

namespace App.Domain;

public class Driver : DomainEntityMetaId
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    [MaxLength(25, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(25, MinimumLength = 0, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    
    public string? PersonalIdentifier { get; set; }
    
    [MaxLength(10, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    public string? ServiceProviderCardIdentifier { get; set; }
    
    public ICollection<DriverAndDriverLicenseCategory>? DriverLicenseCategories { get; set; }

    [MaxLength(15, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [MinLength(2, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMinLength")]
    [StringLength(15, MinimumLength = 2, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    
    public string? DriverLicenseNumber { get; set; }


    [DataType(DataType.DateTime)]
    
    public DateTime? DriverLicenseExpiryDate { get; set; }


    public Guid? CityId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Driver), Name = "City")]
    public City? City { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(30, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [StringLength(30, MinimumLength = 1,
        ErrorMessageResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Driver),
        ErrorMessageResourceName = "AddressOfResidence")]
    public string? Address { get; set; } 


    public ICollection<Vehicle>? Vehicles { get; set; }
    public ICollection<Drive>? Drives { get; set; }
    public ICollection<Schedule>? Schedules { get; set; }
    public ICollection<RideTime>? RideTimes { get; set; }
}