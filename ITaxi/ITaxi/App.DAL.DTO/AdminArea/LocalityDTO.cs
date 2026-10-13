using Base.Domain;
using Base.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace App.DAL.DTO.AdminArea
{
    public class LocalityDTO: DomainEntityMetaId
    {
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.RequiredAttributeErrorMessage))]
        [MinLength(1, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.ErrorMessageMinLength))]
        [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.ErrorMessageMaxLength))]
        [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Locality),
       Name = nameof(Resources.Areas.App.Domain.AdminArea.Locality.LocalityName))]
        public string LocalityName { get; set; } = default!;
        [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Locality),
            Name = nameof(Resources.Areas.App.Domain.AdminArea.Locality.Municipality))]
        public Guid MunicipalityId { get; set; }
        public MunicipalityDTO? Municipality { get; set; }

        [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Locality),
            Name = "EhakUnitTypeName")]
        public Guid EhakUnitTypeId { get; set; }
        public EhakUnitTypeDTO? EhakUnitType { get; set; }

        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.RequiredAttributeErrorMessage))]
        [MinLength(4, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.ErrorMessageMinLength))]
        [MaxLength(4, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.ErrorMessageMaxLength))]
        [Display(ResourceType = typeof(App.Resources
            .Areas.App.Domain.AdminArea.Locality),
            Name = nameof(LocalityEHAKCode))]
        public string LocalityEHAKCode { get; set; } = default!;
    }
}
