using System.ComponentModel.DataAnnotations;
using App.BLL.DTO.AdminArea;
using App.Public.DTO.v1.Identity;
using Base.Domain;

namespace App.Public.DTO.v1.AdminArea;

public class Customer: DomainEntityMetaId
{
    public Guid AppUserId { get; set; }

    public AppUser? AppUser { get; set; }
    
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Customer), Name = "DisabilityType")]
    public Guid DisabilityTypeId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Customer), Name = "DisabilityType")]
    public DisabilityType? DisabilityType { get; set; }

    public ICollection<Booking>? Bookings { get; set; }
}