using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Base.Resources;

namespace App.Domain;

public class Schedule : DomainEntityMetaId
{
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Schedule), Name = "Driver")]
    public Guid DriverId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Schedule), Name = nameof(Driver))]
    public Driver? Driver { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Schedule),
        Name = "Vehicle")]
    public Guid VehicleId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Schedule),
        Name = "Vehicle")]
    public Vehicle? Vehicle { get; set; }


    [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Schedule), Name = "ShiftStartDateAndTime")]
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    public DateTime StartDateAndTime { get; set; }

    [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Schedule), Name = "ShiftEndDateAndTime")]
    [Required]
    public DateTime EndDateAndTime { get; set; }

    public string ShiftDurationTime => $"{StartDateAndTime:g} - {EndDateAndTime:g}";

    public ICollection<RideTime>? RideTimes { get; set; }

    public ICollection<Booking>? Bookings { get; set; }
}