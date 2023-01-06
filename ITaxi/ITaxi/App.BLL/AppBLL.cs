
using App.BLL.Mappers;
using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using AutoMapper;
using Base.BLL;

namespace App.BLL;

public class AppBLL:BaseBLL<IAppUnitOfWork>, IAppBLL
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

    public virtual ICountyService Counties => _counties ??  new CountyService(UnitOfWork.Counties, new CountyMapper(_mapper));
    public virtual ICityService Cities => _cities ?? new CityService(UnitOfWork.Cities, new CityMapper(_mapper));
    public virtual IAdminService Admins => _admins ?? new AdminService(UnitOfWork.Admins, new AdminMapper(_mapper));

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

    private ICountyService? _counties;
    private ICityService? _cities;
    private IAdminService? _admins;
    private IDriverLicenseCategoryService? _driverLicenseCategories;
    private IDriverService? _drivers;
    private IDriverAndDriverLicenseCategoryService? _driverAndDriverLicenseCategories;
    private IVehicleTypeService? _vehicleTypes;
    private IVehicleMarkService? _vehicleMarks;
}