using System.ComponentModel.DataAnnotations;
using Base.Resources;
using Public.App.DTO.v1.Identity;

namespace Public.App.DTO.v1.AdminArea;
public class Driver
{
    public Guid Id { get; set; }
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    [MaxLength(25, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(25, MinimumLength = 0, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Driver), Name = "PersonalIdentifier")]
    public string? PersonalIdentifier { get; set; }

    [MaxLength(15, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [MinLength(2, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMinLength")]
    [StringLength(15, MinimumLength = 2, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Driver), Name = "DriverLicenseNumber")]
    public string DriverLicenseNumber { get; set; } = default!;


    [DataType(DataType.DateTime)]
    [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Driver), Name = "DriverLicenseExpiryDate")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
    public DateTime DriverLicenseExpiryDate { get; set; }

    public int NumberOfDriverLicenseCategories { get; set; }
    public Guid CityId { get; set; }

    [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Driver), Name = "City")]
    public City? City { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(30, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [StringLength(30, MinimumLength = 1,
        ErrorMessageResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Driver),
        ErrorMessageResourceName = "AddressOfResidence")]
    public string Address { get; set; } = default!;




}