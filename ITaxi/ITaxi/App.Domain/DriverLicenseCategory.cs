using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Base.Resources;

namespace App.Domain;

public class DriverLicenseCategory : DomainEntityMetaId
{
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMinMax")]
    public string DriverLicenseCategoryName { get; set; } = default!;

    public ICollection<DriverAndDriverLicenseCategory>? Drivers { get; set; }
}