using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Drive : DomainEntityMetaId
{
    public Guid DriverId { get; set; }
    public Driver? Driver { get; set; }

    public Booking? Booking { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Drive), Name = nameof(Comment))]
    public Comment? Comment { get; set; }
}