using Base.Domain;
using Base.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Text;
using System.Xml.Linq;

namespace App.BLL.DTO.AdminArea
{
    public class MunicipalityDTO : DomainEntityMetaId
    {
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.RequiredAttributeErrorMessage))]
        [MinLength(1, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.ErrorMessageMinLength))]
        [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.ErrorMessageMaxLength))]
        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Municipality), Name = "MunicipalityName")]
        public string Name { get; set; } = default!;
        [Required(ErrorMessageResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Municipality), ErrorMessageResourceName = nameof(EhakUnitType))]
        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Municipality), Name = "EhakUnitType")]
        public Guid EhakUnitTypeId { get; set; }
        public EhakUnitTypeDTO? EhakUnitType { get; set; }

        [Required(ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = nameof(Common.RequiredAttributeErrorMessage))]
        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Municipality), Name = nameof(MunicipalityEHAKCode))]
        public string MunicipalityEHAKCode { get; set; } = default!;
        
        public Guid CountyId { get; set; }
        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Municipality), Name = "County")]
       
        public CountyDTO? County { get; set; }
        public Guid CountryId { get; set; }
        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Municipality), Name = "Country")]
        public CountryDTO? Country { get; set; }
    }
}


