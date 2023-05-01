using Base.Domain;

namespace App.Public.DTO.v1.AdminArea;

public class DriverAndDriverLicenseCategory: DomainEntityId
{
    public Guid DriverId { get; set; }
    public Guid DriverLicenseCategoryId { get; set; }
}