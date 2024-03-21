using System.ComponentModel.DataAnnotations;
using App.Domain;
using Base.Domain;
using Base.Resources;

namespace App.BLL.DTO.AdminArea;

public class DriverLicenseCategoryDTO: DomainEntityMetaId
{
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMinMax")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.DriverLicenseCategory),
        Name = "DriverLicenseCategoryName")]
    public string DriverLicenseCategoryName { get; set; } = default!;

    public ICollection<DriverAndDriverLicenseCategory>? Drivers { get; set; }
}