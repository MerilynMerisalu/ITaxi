using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Base.Resources;

namespace App.Domain
{
    public class City : DomainEntityMetaId
    {
        public Guid CountyId { get; set; }

        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.City),
        Name = "CountyName")] 
        public County? County { get; set; }

        [Required(ErrorMessage = nameof(Common.RequiredAttributeErrorMessage))]
        [MaxLength(50,ErrorMessage = 
            "ErrorMessageMaxLength")]
        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.City),
            Name = nameof(CityName))]
        public string CityName { get; set; } = default!;
    }
}