
using App.BLL.Mappers;
using App.BLL.Mappers.Identity;
using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using AutoMapper;
using Base.BLL;

namespace App.BLL;

public class AppBLL : BaseBLL<IAppUnitOfWork>, IAppBLL
{
    private readonly AutoMapper.IMapper _mapper;
    protected IAppUnitOfWork UnitOfWork;
    public AppBLL(IAppUnitOfWork unitOfWork, IMapper mapper)
    {
        UnitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public override async Task<int> SaveChangesAsync()
    {
        return await UnitOfWork.SaveChangesAsync();
    }

    public override int SaveChanges()
    {
        return UnitOfWork.SaveChanges();
    }

    public virtual ICountyService Counties => _counties ?? new CountyService(UnitOfWork.Counties, new CountyMapper(_mapper));
    public virtual ICityService Cities => _cities ?? new CityService(UnitOfWork.Cities, new CityMapper(_mapper));
    public virtual IAdminService Admins => _admins ?? new AdminService(UnitOfWork.Admins, new AdminMapper(_mapper));
    public virtual IAppUserService AppUsers => _appUsers ?? new AppUserService(UnitOfWork.AppUsers, new AppUserMapper(_mapper));

    public virtual IDriverService Drivers => _drivers ?? new DriverService(UnitOfWork.Drivers, new
        DriverMapper(_mapper));
    public virtual IDriverLicenseCategoryService DriverLicenseCategories =>
        _driverLicenseCategories ?? new DriverLicenseCategoryService(UnitOfWork.DriverLicenseCategories,
            new DriverLicenseCategoryMapper(_mapper));

    public virtual IDriverAndDriverLicenseCategoryService DriverAndDriverLicenseCategories
        => _driverAndDriverLicenseCategories ?? new DriverAndDriverLicenseCategoryService(
            UnitOfWork.DriverAndDriverLicenseCategories, new DriverAndDriverLicenseCategoryMapper(_mapper));

    public virtual IVehicleTypeService VehicleTypes => _vehicleTypes ?? new VehicleTypeService(UnitOfWork.VehicleTypes,
        new VehicleTypeMapper(_mapper));

    public virtual IVehicleMarkService VehicleMarks => _vehicleMarks ??
                                                       new VehicleMarkService(UnitOfWork.VehicleMarks,
                                                           new VehicleMarkMapper(_mapper));
    public virtual IVehicleModelService VehicleModels => _vehicleModels ??
                                                         new VehicleModelService(UnitOfWork.VehicleModels,
                                                             new VehicleModelMapper(_mapper));

    public virtual IVehicleService Vehicles => _vehicles ??
                                               new VehicleService(UnitOfWork.Vehicles,
                                                   new VehicleMapper(_mapper));

    public virtual IScheduleService Schedules => _schedules ??
                                                 new ScheduleService(UnitOfWork.Schedules,
                                                    new ScheduleMapper(_mapper));
    public virtual IRideTimeService RideTimes => _rideTimes ?? new RideTimeService(
        UnitOfWork.RideTimes,
        new RideTimeMapper(_mapper));

    public virtual IDisabilityTypeService DisabilityTypes => _disabilityTypes ?? new DisabilityTypeService(
        UnitOfWork.DisabilityTypes,
        new DisabilityTypeMapper(_mapper));
    
    public virtual ICustomerService Customers => _customers ?? new CustomerService(
        UnitOfWork.Customers,
        new CustomerMapper(_mapper));

    public virtual IBookingService Bookings => _bookings ?? new BookingService(UnitOfWork.Bookings,
        new BookingMapper(_mapper));

    public virtual IDriveService Drives => _drives ?? new DriveService(UnitOfWork.Drives,
        new DriveMapper(_mapper));

    
    public virtual ICommentService Comments => _comments ?? new CommentService(UnitOfWork.Comments, new CommentMapper(_mapper));
    public virtual IPhotoService Photos => _photos ?? new PhotoService(UnitOfWork.Photos, new PhotoMapper(_mapper));


    private ICountyService? _counties;
    private ICityService? _cities;
    private IAdminService? _admins;
    private IAppUserService? _appUsers;
    private IDriverLicenseCategoryService? _driverLicenseCategories;
    private IDriverService? _drivers;
    private IDriverAndDriverLicenseCategoryService? _driverAndDriverLicenseCategories;
    private IVehicleTypeService? _vehicleTypes;
    private IVehicleMarkService? _vehicleMarks;
    private IVehicleModelService? _vehicleModels;
    private IVehicleService? _vehicles;
    private IScheduleService? _schedules;
    private IDisabilityTypeService? _disabilityTypes;
    private IRideTimeService? _rideTimes;
    private ICustomerService? _customers;
    private IBookingService? _bookings;
    private IDriveService? _drives;
    private ICommentService? _comments;
    private IPhotoService? _photos;
}