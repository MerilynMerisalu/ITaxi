using Base.Domain;
using Base.Resources;
using System.ComponentModel.DataAnnotations;

namespace App.DAL.DTO.AdminArea
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
        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.County), Name = "CountyName")]
        public Guid CountyId { get; set; }
        public CountyDTO? County { get; set; }
    }
}

