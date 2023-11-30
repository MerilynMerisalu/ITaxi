using System.ComponentModel.DataAnnotations;
using App.Public.DTO.v1.AdminArea;
using App.Public.DTO.v1.Identity;
using Base.Domain;
using Base.Resources;

namespace App.Public.DTO.v1.DriverArea;
public class Schedule : DomainEntityMetaId
{
    public Guid UserId { get; set; }
    public Driver? Driver { get; set; }
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.DriverArea.Schedule), Name = "Vehicle")]
    public Guid VehicleId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.DriverArea.Schedule), Name = "Vehicle")]
    public Vehicle? Vehicle { get; set; }

    [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.DriverArea.Schedule), Name = "ShiftStartDateAndTime")]
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    public DateTime StartDateAndTime { get; set; }

    [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.DriverArea.Schedule), Name = "ShiftEndDateAndTime")]
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    public DateTime EndDateAndTime { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.DriverArea.Schedule), Name = "ScheduleName")]
    public string ShiftDurationTime => $"{StartDateAndTime:g} - {EndDateAndTime:g}";

    // public ICollection<RideTimeDTO>? RideTimes { get; set; }

    // public ICollection<BookingDTO>? Bookings { get; set; }

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.DriverArea.Schedule), Name = "NumberOfRideTimesPerSchedule")]
    public int NumberOfRideTimes { get; set; }

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.DriverArea.Schedule), Name = "NumberOfTakenRideTimesPerSchedule")]
    public int NumberOfTakenRideTimes { get; set; }
    
}