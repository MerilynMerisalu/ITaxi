using System.ComponentModel;
using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class Customer: DomainEntityMetaId
{

    public Guid AppUserId { get; set; }

    public AppUser? AppUser { get; set; }


    [DisplayName("Disability Type")] public Guid DisabilityTypeId { get; set; }

    [DisplayName("Disability Type Name")] public DisabilityType? DisabilityType { get; set; }

    public ICollection<Booking>? Bookings { get; set; }
}