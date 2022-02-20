using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class County: DomainEntityMetaId
{
    
    [Required]
    [MaxLength(50)]
    [StringLength(50, MinimumLength = 1)]
    [DisplayName("County Name")]
    public string CountyName { get; set; } = default!;

    public ICollection<City>? Cities { get; set; }
}