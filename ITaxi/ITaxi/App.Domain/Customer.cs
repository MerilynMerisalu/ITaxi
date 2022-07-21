using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

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