using App.Contracts.DAL;
using App.Contracts.DAL.IAppRepositories;
using App.DAL.EF.Repositories;
using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUOW: BaseUOW<AppDbContext>, IAppUnitOfWork
{
    public AppUOW(AppDbContext dbContext) : base(dbContext)
    {
    }

    private ICountyRepository? _counties;
    private ICityRepository? _cities;
    private IAdminRepository? _admins;
    private IBookingRepository? _bookings;
    private IDriverRepository? _drivers;

    public ICountyRepository Counties => _counties ?? new CountyRepository(UOWDbContext);
    public ICityRepository Cities => _cities ?? new CityRepository(UOWDbContext);
    public IAdminRepository Admins => _admins ?? new AdminRepository(UOWDbContext);
    public IBookingRepository Bookings => _bookings ?? new BookingRepository(UOWDbContext);
    public IDriverRepository Drivers => _drivers ?? new DriverRepository(UOWDbContext);
}