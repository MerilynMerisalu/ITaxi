using Base.Domain;

namespace App.BLL.DTO.AdminArea;

public class DriverAndDriverLicenseCategoryDTO: DomainEntityId
{
    public Guid DriverId { get; set; }

    public DriverDTO? Driver { get; set; }

    public Guid DriverLicenseCategoryId { get; set; }

    public DriverLicenseCategoryDTO? DriverLicenseCategory { get; set; }
    public string DriverLicenseCategoryNames { get; set; } = default!;
}