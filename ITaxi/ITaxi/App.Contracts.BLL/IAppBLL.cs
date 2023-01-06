using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL;

public interface IAppBLL: IBLL
{
    ICountyService Counties { get; }
    ICityService Cities { get; }
    IAdminService Admins { get; }
    IDriverLicenseCategoryService DriverLicenseCategories { get; }
    IDriverService Drivers { get; }
    IDriverAndDriverLicenseCategoryService DriverAndDriverLicenseCategories { get; }
    IVehicleTypeService VehicleTypes { get; }
}