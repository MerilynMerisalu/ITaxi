using Base.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Enum.Enum;
using Base.Resources;


namespace App.Domain
{
    public class ExtraService : DomainEntityMetaId
    {
        [Required(ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [MaxLength(128, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
        [StringLength(128, ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "ErrorMessageMaxLength")]
        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.ExtraService),
        Name = nameof(ExtraServiceName))]
        public LangStr ExtraServiceName { get; set; } = default!;

        

        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [MaxLength(128, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
        [StringLength(128, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
        public LangStr Description { get; set; } = default!;

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [DataType(DataType.Currency)]
        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.ExtraService),
        Name = nameof(Price))]
        public decimal Price { get; set; }
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.ExtraService),
        Name = nameof(ExtraServiceType))]
        public ExtraServiceType ExtraServiceType { get; set; }

    }
}
