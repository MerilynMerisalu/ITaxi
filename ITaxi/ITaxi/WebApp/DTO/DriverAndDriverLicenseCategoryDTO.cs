using Base.Domain;

namespace WebApp.DTO;

public class DriverAndDriverLicenseCategoryDTO
{
    //public Guid DriverId { get; set; }
    //public Guid DriverLicenseCategoryId { get; set; }

    public ICollection<string>? DriverLicenseCategoryNames { get; set; } 
}