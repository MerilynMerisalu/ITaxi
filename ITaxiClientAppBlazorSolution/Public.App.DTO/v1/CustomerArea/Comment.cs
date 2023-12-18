using Public.App.DTO.v1.DriverArea;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Public.App.DTO.v1.CustomerArea
{
    public class Comment
    {
        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Comment),
        Name = "Drive")]
        public Guid? DriveId { get; set; }

        public Drive? Drive { get; set; }
        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Comment),
            Name = "Drive")]
        public string DriveCustomerStr { get; set; } = default!;

        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Booking),
            Name = "Driver")]
        public string DriverName { get; set; } = default!;

        [MaxLength(1000)]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Comment),
            Name = "CommentName")]
        public string? CommentText { get; set; }

        public string DriveTimeAndDriver { get; set; } = default!;
    }
}
