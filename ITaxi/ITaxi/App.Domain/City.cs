using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using Base.Domain;

namespace App.Domain
{
    public class City : DomainEntityMetaId
    {
        public Guid CountyId { get; set; }

        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.City),
        Name = "CountyName")] 
        public County? County { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.City),
            Name = nameof(CityName))]
        public string CityName { get; set; } = default!;
    }
}