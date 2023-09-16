﻿using App.Contracts.DAL;
using App.Contracts.DAL.IAppRepositories;
using App.DAL.EF.Mappers;
using App.DAL.EF.Mappers.Identity;
using App.DAL.EF.Repositories;
using AutoMapper;
using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUOW : BaseUOW<AppDbContext>, IAppUnitOfWork
{
    private readonly IMapper _mapper;
    private IAdminRepository? _admins;
    private IAppUserRepository? _appUsers;
    private IBookingRepository? _bookings;
    private ICityRepository? _cities;
    private ICommentRepository? _comments;
    private ICountryRepository? _countries;
    private ICountyRepository? _counties;
    private ICustomerRepository? _customers;
    private IDisabilityTypeRepository? _disabilityTypes;
    private IDriverAndDriverLicenseCategoryRepository? _driverAndDriverLicenseCategories;
    private IDriverLicenseCategoryRepository? _driverLicenseCategories;
    private IDriverRepository? _drivers;
    private IDriveRepository? _drives;
    private IPhotoRepository? _photos;
    private IRideTimeRepository? _rideTimes;
    private IScheduleRepository? _schedules;
    private IVehicleMarkRepository? _vehicleMarks;
    private IVehicleModelRepository? _vehicleModels;
    private IVehicleRepository? _vehicles;
    private IVehicleTypeRepository? _vehicleTypes;

    public AppUOW(AppDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }

    public virtual ICountryRepository Countries => _countries ??=
        new CountryRepository(UOWDbContext, new CountryMapper(_mapper));
    public virtual ICountyRepository Counties => _counties ??= new CountyRepository(UOWDbContext
    , new CountyMapper(_mapper));
    public virtual ICityRepository Cities => _cities ??= new CityRepository(UOWDbContext,
        new CityMapper(_mapper));
    public virtual IAdminRepository Admins => _admins ??= new AdminRepository(UOWDbContext, new AdminMapper(_mapper));
    public virtual IAppUserRepository AppUsers => _appUsers ??= new AppUserRepository(UOWDbContext, new AppUserMapper(_mapper));
    public IBookingRepository Bookings => _bookings ??= new BookingRepository(UOWDbContext, new BookingMapper(_mapper));
    public IDriverRepository Drivers => _drivers ??= new DriverRepository(UOWDbContext, new DriverMapper(_mapper));

    public virtual IDriverLicenseCategoryRepository DriverLicenseCategories =>
        _driverLicenseCategories ??= new DriverLicenseCategoryRepository(UOWDbContext, new DriverLicenseCategoryMapper(_mapper));

    public IDriverAndDriverLicenseCategoryRepository DriverAndDriverLicenseCategories
        => _driverAndDriverLicenseCategories ??= new DriverAndDriverLicenseCategoryRepository(UOWDbContext, new DriverAndDriverLicenseCategoryMapper(_mapper));


    public IVehicleTypeRepository VehicleTypes
        => _vehicleTypes ??= new VehicleTypeRepository(UOWDbContext, new VehicleTypeMapper(_mapper));

    public IVehicleMarkRepository VehicleMarks => _vehicleMarks ??= new VehicleMarkRepository(UOWDbContext,
        new VehicleMarkMapper(_mapper));

    public IVehicleModelRepository VehicleModels => _vehicleModels ??= new VehicleModelRepository(UOWDbContext, new VehicleModelMapper(_mapper));
    public IVehicleRepository Vehicles => _vehicles ??= new VehicleRepository(UOWDbContext,
        new VehicleMapper(_mapper));
    public IScheduleRepository Schedules => _schedules ??= new ScheduleRepository(UOWDbContext, new ScheduleMapper(_mapper));
    public IRideTimeRepository RideTimes => _rideTimes ??= new RideTimeRepository(UOWDbContext, new RideTimeMapper(_mapper));
    public IDisabilityTypeRepository DisabilityTypes => _disabilityTypes ??=
                                                        new DisabilityTypeRepository(UOWDbContext,
                                                            new DisabilityTypeMapper(_mapper));
    public ICustomerRepository Customers => _customers ??= new CustomerRepository(UOWDbContext
    , new CustomerMapper(_mapper));
    public IDriveRepository Drives => _drives ??= new DriveRepository(UOWDbContext, new DriveMapper(_mapper));
    public ICommentRepository Comments => _comments ??= new CommentRepository(UOWDbContext, new CommentMapper(_mapper));
    public IPhotoRepository Photos => _photos ??= new PhotoRepository(UOWDbContext, new PhotoMapper(_mapper));
}