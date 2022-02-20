using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Base.Domain;
using WebApp.Models.Enum;

namespace App.Domain;

public class Vehicle:DomainEntityMetaId
{
    

    [DisplayName("Driver")] public Guid DriverId { get; set; }

    [DisplayName("Driver")] public Driver? Driver { get; set; }

    [DisplayName("Vehicle Type")] public Guid VehicleTypeId { get; set; }

    [DisplayName("Vehicle Type")] public VehicleType? VehicleType { get; set; }

    [DisplayName("Vehicle Mark")] public Guid VehicleMarkId { get; set; }

    [DisplayName("Vehicle Mark")] public VehicleMark? VehicleMark { get; set; }


    [DisplayName("Vehicle Model")] public Guid VehicleModelId { get; set; }

    [DisplayName("Vehicle Model")] public VehicleModel? VehicleModel { get; set; }

    [Required]
    [MaxLength(25)]
    [StringLength(25, MinimumLength = 1)]
    [DisplayName("Vehicle Plate Number")]
    public string VehiclePlateNumber { get; set; } = default!;

    [Display(Name = "Manufacture Year")]
    [Required]
    public int ManufactureYear { get; set; }

    [Range(1, 6)]
    [Display(Name = "Number Of Seats")]
    [Required]
    public int NumberOfSeats { get; set; }

    [DisplayName("Vehicle Identifier")]
    public string VehicleIdentifier => $"{VehicleMark?.VehicleMarkName} {VehicleModel?.VehicleModelName} " +
                                       $"{VehiclePlateNumber} {VehicleType?.VehicleTypeName}";

    [Display(Name = "Vehicle Availability")]
    public VehicleAvailability VehicleAvailability { get; set; }

    public ICollection<Schedule>? Schedules { get; set; }

    public ICollection<Photo>? VehiclePhotos { get; set; }
}