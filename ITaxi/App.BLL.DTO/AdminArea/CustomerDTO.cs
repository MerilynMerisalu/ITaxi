using System.ComponentModel.DataAnnotations;
using App.BLL.DTO.Identity;
using Base.Domain;

namespace App.BLL.DTO.AdminArea;

public class CustomerDTO : DomainEntityMetaId
{
    public Guid AppUserId { get; set; }

    public AppUser? AppUser { get; set; }
    
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Customer), Name = "DisabilityType")]
    public Guid DisabilityTypeId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Customer), Name = "DisabilityType")]
    public DisabilityTypeDTO? DisabilityType { get; set; }

    //public ICollection<BookingDTO>? Bookings { get; set; }
}