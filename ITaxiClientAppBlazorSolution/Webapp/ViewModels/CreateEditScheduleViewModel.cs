﻿using ITaxi.Public.DTO.v1.DriverArea;

namespace Webapp.ViewModels
{
    public class CreateEditScheduleViewModel
    {
        public Guid VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }
        public DateTime? StartDateAndTime { get; set; } 
    }
}
