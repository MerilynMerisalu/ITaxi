using Base.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Public.App.DTO.v1.AdminArea
{
    public class DisabilityType: Entity
    {
        [MaxLength(80, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
        [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.DisabilityType),
        Name = nameof(DisabilityTypeName))]
        public string DisabilityTypeName { get; set; } = default!;
    }
}
