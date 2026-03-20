
using App.Resources.Areas.App.Domain;
using Base.Domain;
using Base.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace App.DAL.DTO.AdminArea
{
    public class ExtraServiceDTO: DomainEntityMetaId
    {
        [Required(ErrorMessageResourceType = typeof(Common),
           ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [MaxLength(128, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
        [StringLength(128, ErrorMessageResourceType = typeof(Common),
           ErrorMessageResourceName = "ErrorMessageMaxLength")]
        public LangStr ExtraServiceName { get; set; } = default!;


        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [MaxLength(128, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
        [StringLength(128, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
        public LangStr Description { get; set; } = default!;

        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        public ExtraServiceType ExtraServiceType { get; set; }
    }
}
