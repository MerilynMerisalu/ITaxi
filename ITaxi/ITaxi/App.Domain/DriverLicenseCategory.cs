using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class DriverLicenseCategory : DomainEntityMetaId
{

    [Required]
    [MaxLength(50)]
    [StringLength(50, MinimumLength = 1)]
    [DisplayName("Driver License Category Name")]
    public string DriverLicenseCategoryName { get; set; } = default!;

    public ICollection<DriverAndDriverLicenseCategory>? Drivers { get; set; }
}