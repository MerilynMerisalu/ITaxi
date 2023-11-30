using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.BLL.DTO.AdminArea;
using Base.Domain;

namespace App.Public.DTO.v1.AdminArea;

public class RideTime : DomainEntityMetaId
{
    public Guid DriverId { get; set; }

    public Driver? Driver { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.RideTime), Name = "Schedule")]
    public Guid ScheduleId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.RideTime), Name = "Schedule")]

    public Schedule? Schedule { get; set; } 

    [DataType(DataType.DateTime)]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.RideTime), Name = "RideDateAndTime")]
    [DisplayFormat(DataFormatString = "{0:t}")]
    public DateTime RideDateTime { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.RideTime), Name = "IsTaken")]
    public bool IsTaken { get; set; }
    
    public DateTime? ExpiryTime { get; set; } 
    
    [ForeignKey(nameof(Booking))]
    public Guid? BookingId { get; set; }
    public Booking? Booking { get; set; }
}