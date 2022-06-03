﻿using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUnitOfWork: IUnitOfWork
{
     ICountyRepository Counties { get; }
     ICityRepository Cities { get; }
     IAdminRepository Admins { get; }
     IBookingRepository Bookings { get; }
     IDriverRepository Drivers { get; }
     IDriverLicenseCategoryRepository DriverLicenseCategories { get; }
     IDriverAndDriverLicenseCategoryRepository DriverAndDriverLicenseCategories { get; }
     IVehicleTypeRepository VehicleTypes { get; }
     IVehicleMarkRepository VehicleMarks{ get; }
     IVehicleModelRepository VehicleModels{ get; }
     IVehicleRepository Vehicles { get; }

}