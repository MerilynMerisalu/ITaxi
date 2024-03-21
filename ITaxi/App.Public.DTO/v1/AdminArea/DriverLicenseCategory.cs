using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Domain;
using Base.Resources;

namespace App.Public.DTO.v1.AdminArea
{
    public class DriverLicenseCategory: DomainEntityMetaId
    {
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
        [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "ErrorMessageStringLengthMinMax")]
        public string DriverLicenseCategoryName { get; set; } = default!;

    }
}
