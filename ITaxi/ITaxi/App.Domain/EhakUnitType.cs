using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Base.Domain;
using Base.Resources;

namespace App.Domain
{
    public class EhakUnitType: DomainEntityMetaId
    {
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.RequiredAttributeErrorMessage))]
        [MinLength(1, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.ErrorMessageMinLength))]
        [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.ErrorMessageMaxLength))]
        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.EhakUnitType), Name = "EhakUnitTypeName")]
        public string EhakUnitTypeName { get; set; } = default!;
        
        
        
    }
}
