using System.ComponentModel.DataAnnotations;
using App.DAL.DTO.Identity;
using App.Domain;
using Base.Domain;
using Base.Resources;

namespace App.DAL.DTO.AdminArea;

public class DriverDTO: DomainEntityMetaId
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    [MaxLength(25, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(25, MinimumLength = 0, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    
    public string? PersonalIdentifier { get; set; }

    
    public ICollection<DriverAndDriverLicenseCategory>? DriverLicenseCategories { get; set; }

    [MaxLength(15, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [MinLength(2, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMinLength")]
    [StringLength(15, MinimumLength = 2, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Driver), Name = "DriverLicenseNumber")]
    public string DriverLicenseNumber { get; set; } = default!;


    [DataType(DataType.DateTime)]
    
#warning DateTime input control does not support user changing the language yet.
    public DateTime DriverLicenseExpiryDate { get; set; }

    public int NumberOfDriverLicenseCategories { get; set; }
    public Guid CityId { get; set; }

    
    public CityDTO? City { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(30, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [StringLength(30, MinimumLength = 1,
        ErrorMessageResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Driver),
        ErrorMessageResourceName = "AddressOfResidence")]
    public string Address { get; set; } = default!;


    public ICollection<VehicleDTO>? Vehicles { get; set; }
    public ICollection<DriveDTO>? Drives { get; set; }
    public ICollection<ScheduleDTO>? Schedules { get; set; }
    public ICollection<RideTimeDTO>? RideTimes { get; set; }
}