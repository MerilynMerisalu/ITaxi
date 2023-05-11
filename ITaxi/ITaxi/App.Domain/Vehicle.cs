using System.ComponentModel.DataAnnotations;
using App.Enum.Enum;
using Base.Domain;
using Base.Resources;

namespace App.Domain;

public class Vehicle : DomainEntityMetaId
{
    public Guid DriverId { get; set; }
    public Driver? Driver { get; set; }
    public Guid VehicleTypeId { get; set; }
    
    public VehicleType? VehicleType { get; set; }
    public Guid VehicleMarkId { get; set; }

    public VehicleMark? VehicleMark { get; set; }
    public Guid VehicleModelId { get; set; }

    public VehicleModel? VehicleModel { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(25, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(25, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    public string VehiclePlateNumber { get; set; } = default!;

    [Required]
    public int ManufactureYear { get; set; }

    [Range(1, 6, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
    
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    public int NumberOfSeats { get; set; }
    public string VehicleIdentifier => $"{VehicleMark?.VehicleMarkName} {VehicleModel?.VehicleModelName} " +
                                       $"{VehiclePlateNumber} {VehicleType?.VehicleTypeName}";
    public VehicleAvailability VehicleAvailability { get; set; }

    public ICollection<Schedule>? Schedules { get; set; }

    public ICollection<Photo>? VehiclePhotos { get; set; }
}