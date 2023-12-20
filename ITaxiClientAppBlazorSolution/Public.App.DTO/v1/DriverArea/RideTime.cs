using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Public.App.DTO.v1.CustomerArea;

namespace Public.App.DTO.v1.DriverArea
{
    public class RideTime: Entity
    {
        
        public Guid DriverId { get; set; }

        /*public DriverDTO? Driver { get; set; }
        */

        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.DriverArea.RideTime), Name = "Schedule")]
        public Guid ScheduleId { get; set; }

        /*[Display(ResourceType = typeof(Resources.Areas.App.Domain.DriverArea.RideTime), Name = "Schedule")]*/

        public Schedule? Schedule { get; set; }

        [DataType(DataType.DateTime)]
        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.DriverArea.RideTime), Name = "RideDateAndTime")]
        [DisplayFormat(DataFormatString = "{0:t}")]
        public DateTime RideDateTime { get; set; }

        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.DriverArea.RideTime), Name = "IsTaken")]
        
        public bool IsTaken { get; set; }

        public DateTime? ExpiryTime { get; set; }

        [ForeignKey(nameof(Booking))]
        public Guid? BookingId { get; set; }
        public Booking? Booking { get; set; }
    }
}
