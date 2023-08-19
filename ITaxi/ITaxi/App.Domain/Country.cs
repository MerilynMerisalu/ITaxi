using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Base.Resources;

namespace App.Domain;

public class Country: DomainEntityMetaId
{
    [Required(ErrorMessageResourceType = typeof(Base.Resources.Common), 
        ErrorMessageResourceName = nameof(Common.RequiredAttributeErrorMessage))]
    [MaxLength(50, ErrorMessageResourceType =typeof(Common),
        ErrorMessageResourceName = nameof(Common.ErrorMessageStringLengthMax))]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = nameof(Common.ErrorMessageStringLengthMinMax))]
    public string CountryName { get; set; } = default!;

    public ICollection<County>? Counties { get; set; }
}