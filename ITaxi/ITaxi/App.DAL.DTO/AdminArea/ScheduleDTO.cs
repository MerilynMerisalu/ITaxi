using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Base.Resources;

namespace App.DAL.DTO.AdminArea;

public class ScheduleDTO : DomainEntityMetaId
{
    
    public Guid DriverId { get; set; }
    
    public DriverDTO? Driver { get; set; }
    public Guid VehicleId { get; set; }
    
    public VehicleDTO? Vehicle { get; set; }
    
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    public DateTime StartDateAndTime { get; set; }
    
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    public DateTime EndDateAndTime { get; set; }
    public string ShiftDurationTime => $"{StartDateAndTime:g} - {EndDateAndTime:g}";

    public ICollection<RideTimeDTO>? RideTimes { get; set; }

    public ICollection<BookingDTO>? Bookings { get; set; }
    
    public int NumberOfRideTimes { get; set; }
    #warning is it right
    public int NumberOfTakenRideTimes => RideTimes.Count(r => r.IsTaken);


}