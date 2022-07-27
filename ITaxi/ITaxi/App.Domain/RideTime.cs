﻿using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

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
}