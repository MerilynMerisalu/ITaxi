using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class RideTime: DomainEntityMetaId
{
    
    public Guid ScheduleId { get; set; }

    public Schedule? Schedule { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayName("Ride Time")]
    public DateTime RideDateTime { get; set; }

    [DisplayName("Is Taken")] public bool IsTaken { get; set; }
}