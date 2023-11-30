using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace App.BLL.DTO.AdminArea;

public class RideTimeDTO: DomainEntityMetaId
{
    public Guid DriverId { get; set; }

    public DriverDTO? Driver { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.RideTime), Name = "Schedule")]
    public Guid ScheduleId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.RideTime), Name = "Schedule")]

    public ScheduleDTO? Schedule { get; set; } 

    [DataType(DataType.DateTime)]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.RideTime), Name = "RideDateAndTime")]
    [DisplayFormat(DataFormatString = "{0:t}")]
    public DateTime RideDateTime { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.RideTime), Name = "IsTaken")]
    public bool IsTaken { get; set; }
    
    public DateTime? ExpiryTime { get; set; } 
    
    [ForeignKey(nameof(BookingDTO))]
    public Guid? BookingId { get; set; }
    public BookingDTO? Booking { get; set; }
}