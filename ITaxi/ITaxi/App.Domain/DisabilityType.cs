using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class DisabilityType: DomainEntityMetaId
{
    public Guid DisabilityTypeId { get; set; }

    [Required]
    [MaxLength(80)]
    [DisplayName("Disability Type")]
    public string DisabilityTypeName { get; set; } = default!;
}