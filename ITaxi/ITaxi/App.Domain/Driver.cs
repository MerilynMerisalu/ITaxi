using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class Driver: DomainEntityMetaId
{

    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    [MaxLength(25)]
    [StringLength(25)]
    [DisplayName("Personal Identifier")]
    public string? PersonalIdentifier { get; set; } = default!;

    [DisplayName("Driver License Categories")]
    public ICollection<DriverAndDriverLicenseCategory>? DriverLicenseCategories { get; set; }

    [MaxLength(15)]
    [MinLength(2)]
    [StringLength(15, MinimumLength = 2)]
    [DisplayName("Driver License Number")]
    public string DriverLicenseNumber { get; set; } = default!;


    [DataType(DataType.DateTime)]
    [DisplayName("Driver License Expiry Date")]
    public DateTime DriverLicenseExpiryDate { get; set; } = default!;

    [DisplayName("City")] public Guid CityId { get; set; }

    public City? City { get; set; }

    [Required]
    [MaxLength(30)]
    [StringLength(30, MinimumLength = 1)]
    public string Address { get; set; } = default!;


    public ICollection<Vehicle>? Vehicles { get; set; }
    public ICollection<Drive>? Drives { get; set; }
    public ICollection<Schedule>? Schedules { get; set; }
}