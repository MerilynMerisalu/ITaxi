using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.Domain;

public class RideTime : DomainEntityMetaId
{
    public Guid DriverId { get; set; }
    
    public Guid ScheduleId { get; set; }
    
    public Schedule? Schedule { get; set; } 

    [DataType(DataType.DateTime)]
    //[DisplayFormat(DataFormatString = "{0:t}")]
    public DateTime RideDateTime { get; set; }
    
    public bool IsTaken { get; set; }
    
    public DateTime? ExpiryTime { get; set; } 
    
    [ForeignKey(nameof(Booking))]
    public Guid? BookingId { get; set; }
    public Booking? Booking { get; set; }
}