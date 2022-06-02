using App.Contracts.DAL;
using App.Contracts.DAL.IAppRepositories;
using App.DAL.EF.Repositories;
using App.Domain;
using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUOW : BaseUOW<AppDbContext>, IAppUnitOfWork
{
    public AppUOW(AppDbContext dbContext) : base(dbContext)
    {
    }

    private ICountyRepository? _counties;
    private ICityRepository? _cities;
    private IAdminRepository? _admins;
    private IBookingRepository? _bookings;
    private IDriverRepository? _drivers;
    private IDriverLicenseCategoryRepository _driverLicenseCategories;
    private IDriverAndDriverLicenseCategoryRepository _driverAndDriverLicenseCategories;
    private IVehicleTypeRepository _vehicleTypes;
    private IVehicleMarkRepository _vehicleMarks;
    private IVehicleModelRepository _vehicleModels;
    private IVehicleRepository _vehicles;
    

    public ICountyRepository Counties => _counties ?? new CountyRepository(UOWDbContext);
    public ICityRepository Cities => _cities ?? new CityRepository(UOWDbContext);
    public IAdminRepository Admins => _admins ?? new AdminRepository(UOWDbContext);
    public IBookingRepository Bookings => _bookings ?? new BookingRepository(UOWDbContext);
    public IDriverRepository Drivers => _drivers ?? new DriverRepository(UOWDbContext);

    public IDriverLicenseCategoryRepository DriverLicenseCategories =>
        _driverLicenseCategories ?? new DriverLicenseCategoryRepository(UOWDbContext);

    public IDriverAndDriverLicenseCategoryRepository DriverAndDriverLicenseCategories
        => _driverAndDriverLicenseCategories ?? new DriverAndDriverLicenseCategoryRepository(UOWDbContext);

    public IVehicleTypeRepository VehicleTypes
        => _vehicleTypes ?? new VehicleTypeRepository(UOWDbContext);

    public IVehicleMarkRepository VehicleMarks => _vehicleMarks ?? new VehicleMarkRepository(UOWDbContext);
    
    public IVehicleModelRepository VehicleModels => _vehicleModels ?? new VehicleModelRepository(UOWDbContext);
    public IVehicleRepository Vehicles => _vehicles ?? new VehicleRepository(UOWDbContext);
    
}

    