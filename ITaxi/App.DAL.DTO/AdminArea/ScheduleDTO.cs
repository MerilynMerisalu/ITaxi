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

    public ICollection<RideTimeDTO>? RideTimes { get; set; } = new HashSet<RideTimeDTO>();

    public ICollection<BookingDTO>? Bookings { get; set; } = new HashSet<BookingDTO>();
    
    public int NumberOfRideTimes { get; set; }
    // calculated field expression
    // Ideally I should put this in the mapping configuration and convert this to a simple property
    public int NumberOfTakenRideTimes => RideTimes?.Count(r => r.IsTaken) ?? 0;


}