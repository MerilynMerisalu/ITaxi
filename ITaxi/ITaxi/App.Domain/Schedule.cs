using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Schedule: DomainEntityMetaId
{
    [DisplayName("Creation Date and Time")]

    public DateTime ScheduleCreationDateAndTime { get; set; }

    [DisplayName("Driver")] public Guid DriverId { get; set; }

    public Driver? Driver { get; set; }

    [DisplayName("Vehicle")] public Guid VehicleId { get; set; }

    public Vehicle? Vehicle { get; set; }


    [Display(Name = "Shift Start Date and Time")]
    [Required]
    public DateTime StartDateAndTime { get; set; }

    [Display(Name = "Shift End Date and Time")]
    [Required]
    public DateTime EndDateAndTime { get; set; }

    [Display(Name = "Shift Duration Time")]
    public string ShiftDurationTime => $"{StartDateAndTime:g} - {EndDateAndTime:g}";

    public ICollection<RideTime>? RideTimes { get; set; }

    public ICollection<Booking>? Bookings { get; set; }
}