using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Base.Domain;
using WebApp.Models.Enum;

namespace App.Domain;

public class Booking : DomainEntityMetaId
{
    public Guid ScheduleId { get; set; }

    public Schedule? Schedule { get; set; }

    public Guid DriverId { get; set; }
    public Driver? Driver { get; set; }

    public Guid CustomerId { get; set; }

    public Customer? Customer { get; set; }

    [DisplayName("Vehicle Type")] public Guid VehicleTypeId { get; set; }

    [DisplayName("Vehicle Type")] public VehicleType? VehicleType { get; set; }

    public Guid VehicleId { get; set; }

    public Vehicle? Vehicle { get; set; }

    [DisplayName(nameof(City))] public Guid CityId { get; set; }

    public City? City { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayName("Pickup Date and Time")]
    [DisplayFormat(DataFormatString = "{0:dd.mm.yyyy HH:mm}")]
    public DateTime PickUpDateAndTime { get; set; }

    [Required]
    [MaxLength(50)]
    [StringLength(50, MinimumLength = 1)]
    [DisplayName("Pickup Address")]
    public string PickupAddress { get; set; } = default!;

    [Required]
    [MaxLength(50)]
    [DisplayName("Destination Address")]
    public string DestinationAddress { get; set; } = default!;

    [Required]
    [Range(1, 5)]
    [DisplayName("Number Of Passengers")]
    public int NumberOfPassengers { get; set; }

    [DisplayName("Has an Assistant?")] public bool HasAnAssistant { get; set; }

    [MaxLength(1000)]
    [DataType(DataType.MultilineText)]
    [DisplayName("Additional Info")]
    public string? AdditionalInfo { get; set; }

    [DisplayName("Status of Booking")] public StatusOfBooking StatusOfBooking { get; set; }

    public Guid? DriveId { get; set; }
    public Drive? Drive { get; set; }
}