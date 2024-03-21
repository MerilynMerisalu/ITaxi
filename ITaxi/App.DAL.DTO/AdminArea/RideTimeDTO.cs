using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.DAL.DTO.AdminArea;

public class RideTimeDTO : DomainEntityMetaId
{
    public Guid DriverId { get; set; }

    public DriverDTO? Driver { get; set; }

    
    public Guid ScheduleId { get; set; }

    
    public ScheduleDTO? Schedule { get; set; } 

    [DataType(DataType.DateTime)]
    
    public DateTime RideDateTime { get; set; }

    
    public bool IsTaken { get; set; }
    
    public DateTime? ExpiryTime { get; set; } 
    
    [ForeignKey(nameof(BookingDTO))]
    public Guid? BookingId { get; set; }
    public BookingDTO? Booking { get; set; }
}