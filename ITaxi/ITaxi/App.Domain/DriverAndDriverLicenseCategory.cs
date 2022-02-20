using Base.Domain;

namespace App.Domain;

public class DriverAndDriverLicenseCategory: DomainEntityId
{
    public Guid DriverId { get; set; }

    public Driver? Driver { get; set; }

    public Guid DriverLicenseCategoryId { get; set; }

    public DriverLicenseCategory? DriverLicenseCategory { get; set; }
}