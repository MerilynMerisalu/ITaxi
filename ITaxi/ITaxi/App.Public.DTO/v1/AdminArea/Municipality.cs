using Base.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Resources;
using App.BLL.DTO.AdminArea;

namespace App.Public.DTO.v1.AdminArea
{
    public class Municipality: DomainEntityMetaId
    {
        [Required(ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
        [MinLength(1, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMinLength")]
        [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Municipality), Name = "MunicipalityName")]
        public string Name { get; set; } = default!;
        
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        public Guid EhakUnitTypeId { get; set; }

        public EhakUnitType? EhakUnitType { get; set; }

        [Required(ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMinLength")]
        [MinLength(1, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMinLength")]
        [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Municipality), Name = "MunicipalityEHAKCode")]
        public string MunicipalityCode { get; set; } = default!;

        public Guid CountyId { get; set; }
        public CountyDTO? County { get; set; }
    }
}
