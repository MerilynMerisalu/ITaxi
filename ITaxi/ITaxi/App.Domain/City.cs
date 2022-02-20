using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain
{
    public class City : DomainEntityMetaId
    {
        
        [DisplayName("County Name")] 
        public Guid CountyId { get; set; }

        public County? County { get; set; }

        [Required]
        [MaxLength(50)]
        [StringLength(50, MinimumLength = 1)]
        [DisplayName("City")]
        public string CityName { get; set; } = default!;
    }
}