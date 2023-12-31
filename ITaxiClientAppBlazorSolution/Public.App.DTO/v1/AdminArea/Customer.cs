using Public.App.DTO.v1.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Public.App.DTO.v1.AdminArea
{
    public class Customer: Entity
    {
        public Guid AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Customer), Name = "DisabilityType")]
        public Guid DisabilityTypeId { get; set; }

        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Customer), Name = "DisabilityType")]
        public DisabilityType? DisabilityType { get; set; }
    }
}
