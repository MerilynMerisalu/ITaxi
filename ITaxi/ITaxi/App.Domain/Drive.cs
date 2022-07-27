using Base.Domain;

namespace App.Domain;

public class Drive : DomainEntityMetaId
{
    public Guid DriverId { get; set; }
    public Driver? Driver { get; set; }

    public Booking? Booking { get; set; }

    public Comment? Comment { get; set; }
}