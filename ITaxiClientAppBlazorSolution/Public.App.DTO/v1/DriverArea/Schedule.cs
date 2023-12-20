using Base.Resources;
using ITaxi.Public.DTO.v1.AdminArea;
using ITaxi.Public.DTO.v1.DriverArea;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Public.App.DTO.v1.DriverArea
{
    public class Schedule : Entity
    {
        
        public Guid UserId { get; set; }
        public Driver? Driver { get; set; }
        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.DriverArea.Schedule), Name = "Vehicle")]
        public Guid VehicleId { get; set; }

        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.DriverArea.Schedule), Name = "Vehicle")]
        public Vehicle? Vehicle { get; set; }

        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.DriverArea.Schedule), Name = "ShiftStartDateAndTime")]
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        public DateTime StartDateAndTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.DriverArea.Schedule), Name = "ShiftEndDateAndTime")]
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        public DateTime EndDateAndTime { get; set; }

        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.DriverArea.Schedule), Name = "ScheduleName")]
        public string ShiftDurationTime => $"{StartDateAndTime:g} - {EndDateAndTime:g}";

       
        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.DriverArea.Schedule), Name = "NumberOfRideTimesPerSchedule")]
        public int NumberOfRideTimes { get; set; }

        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.DriverArea.Schedule), Name = "NumberOfTakenRideTimesPerSchedule")]
        public int NumberOfTakenRideTimes { get; set; }

    }
}
