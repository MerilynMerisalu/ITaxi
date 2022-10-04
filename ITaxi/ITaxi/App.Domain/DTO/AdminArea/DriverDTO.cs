using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain.DTO.AdminArea;

public class DriverDTO : DomainEntityMetaId
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    [MaxLength(50)] [StringLength(50)] public string? PersonalIdentifier { get; set; }


    public Guid CityId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Admin), Name = "City")]

    public City? City { get; set; }

    [Required]
    [MaxLength(50)]
    [StringLength(50, MinimumLength = 1)]
    public string Address { get; set; } = default!;

    [MaxLength(15)]
    [MinLength(2)]
    [StringLength(15, MinimumLength = 2)]
    public string DriverLicenseNumber { get; set; } = default!;


    [DataType(DataType.DateTime)]
    
    public string DriverLicenseExpiryDate { get; set; } = default!;

    public ICollection<DriverAndDriverLicenseCategory>? DriverLicenseCategories { get; set; }
    public int NumberOfDriverLicenseCategories { get; set; }

}