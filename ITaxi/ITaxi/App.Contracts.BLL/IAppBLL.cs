using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL;

public interface IAppBLL : IBLL
{
    ICountyService Counties { get; }
    ICityService Cities { get; }
    IAdminService Admins { get; }
    IAppUserService AppUsers { get; }
    IDriverLicenseCategoryService DriverLicenseCategories { get; }
    IDriverService Drivers { get; }
    IDriverAndDriverLicenseCategoryService DriverAndDriverLicenseCategories { get; }
    IVehicleTypeService VehicleTypes { get; }
    IVehicleMarkService VehicleMarks { get; }
    IVehicleModelService VehicleModels { get; }
    IVehicleService Vehicles { get; }
    IDisabilityTypeService DisabilityTypes { get; }
    IScheduleService Schedules { get; }
    IRideTimeService RideTimes { get; }
    ICustomerService Customers { get; }
    IBookingService Bookings { get; }
    IDriveService Drives { get; }
    ICommentService Comments { get; }
    IPhotoService Photos { get; }
}