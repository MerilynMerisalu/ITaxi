using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using App.Domain.Enum;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;


namespace App.DAL.EF.Repositories;

public class VehicleRepository : BaseEntityRepository<Vehicle, AppDbContext>, IVehicleRepository
{
    public VehicleRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<IEnumerable<Vehicle>> GetAllAsync(bool noTracking = true)
    {
        return await CreateQuery(noTracking).ToListAsync();
    }

    public override IEnumerable<Vehicle> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList();
    }

    public override Task<Vehicle?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return CreateQuery(noTracking).FirstOrDefaultAsync(v => v.Id.Equals(id));
    }

    public override Vehicle? FirstOrDefault(Guid id, bool noTracking = true)
    {
        return CreateQuery(noTracking).FirstOrDefault(v => v.Id.Equals(id));
    }

    #warning Use DI to inject the current AppUser object instead of passing manually for each method call 
    public async Task<IEnumerable<Vehicle>> GettingOrderedVehiclesAsync(Guid? userId, string? roleName = 
        null ,bool noTracking = true)
    {
        List<Vehicle> res = await CreateQuery(userId, roleName,noTracking)
            .OrderBy(v => v.VehicleType!.VehicleTypeName)
            .ThenBy(v => v.VehicleMark!.VehicleMarkName)
            .ThenBy(v => v.VehicleModel!.VehicleModelName)
            .ThenBy(v => v.ManufactureYear)
            .ToListAsync();
        
           
        return res;
    }

    public IEnumerable<Vehicle> GettingOrderedVehicles(Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        return CreateQuery(userId, roleName,noTracking)
            .OrderBy(v => v.VehicleType!.VehicleTypeName)
            .ThenBy(v => v.VehicleMark!.VehicleMarkName)
            .ThenBy(v => v.VehicleModel!.VehicleModelName)
            .ThenBy(v => v.ManufactureYear).ToList();
    }

    public async Task<IEnumerable<Vehicle>> GettingVehiclesWithoutIncludesAsync(
        bool noTracking = true)
    {
        return await base.CreateQuery(noTracking).ToListAsync();
    }

    public IEnumerable<Vehicle> GettingVehiclesWithoutIncludes(bool noTracking = true)
    {
        return base.CreateQuery(noTracking).ToList();
    }

    public async Task<IEnumerable<Vehicle>> GettingOrderedVehiclesWithoutIncludesAsync(bool noTracking = true)
    {
        return await base.CreateQuery(noTracking).OrderBy(v => v.ManufactureYear).ToListAsync();
    }

    public IEnumerable<Vehicle> GettingOrderedVehiclesWithoutIncludes(bool noTracking = true)
    {
        return base.CreateQuery(noTracking)
            .OrderBy(v => v.ManufactureYear).ToList();
    }

    public async Task<Vehicle?> GettingVehicleWithIncludesByIdAsync(Guid id, Guid? userId = null, 
        string? roleName=null , bool noTracking = true)
    {


        var vehicle = await CreateQuery(userId, roleName,noTracking).
            FirstOrDefaultAsync(v => v.Id.Equals(id));
        if (vehicle == null)
        {
            return null;

        }
       
        return vehicle;
    }

    public async Task<Vehicle?> GettingVehicleByIdAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return await CreateQuery(userId, roleName, noTracking).FirstOrDefaultAsync(v => v.Id.Equals(id));
    }

    public Vehicle? GettingVehicleById(Guid id, bool noTracking = true)
    {
        return CreateQuery(noTracking).FirstOrDefault(v => v.Id.Equals(id));
    }

    public async Task<Vehicle?> GettingVehicleWithoutIncludesByIdAsync(Guid id, bool noTracking = true)
    {
        return await base.CreateQuery(noTracking).FirstOrDefaultAsync(v => v.Id.Equals(id));
    }

    public Vehicle? GettingVehicleWithoutIncludesById(Guid id, bool noTracking = true)
    {
        return base.CreateQuery(noTracking).FirstOrDefault(v => v.Id.Equals(id));
    }

    public List<int> GettingManufactureYears()
    {
        var years = new List<int>();

        for (var i = 6; i > 0; i--)
        {
            var year = DateTime.Today.AddYears(-i).Year;
            years.Add(year);
        }

        years.Reverse();

        return years;
    }

    public async Task<Vehicle?> GettingVehicleWithoutIncludesByDriverIdAndVehicleAvailabilityAsync(Booking booking)
    {
        return await RepoDbSet
            .Where(v => v.DriverId.Equals(booking.DriverId)
                        && v.VehicleAvailability == VehicleAvailability.Available)
            .FirstAsync();
    }

    public Vehicle? GettingVehicleWithoutIncludesByDriverIdAndVehicleAvailability(Booking booking)
    {
        return RepoDbSet
            .First(v => v.DriverId.Equals(booking.DriverId)
                        && v.VehicleAvailability == VehicleAvailability.Available);
    }

    protected  IQueryable<Vehicle> CreateQuery(Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking) query.AsNoTracking();

        if (roleName is nameof(Admin))
        {
            query = query.Include(c => c.Driver)
                .ThenInclude(d => d!.AppUser)
                .Include(v => v.VehicleMark)
                .Include(v => v.VehicleModel)
                .Include(v => v.VehicleType)
                .ThenInclude(v => v!.VehicleTypeName)
                .ThenInclude(v => v.Translations);
            return query;
        }
        
        query = query.Include(c => c.Driver)
            .ThenInclude(d => d!.AppUser)
            .Include(v => v.VehicleMark)
            .Include(v => v.VehicleModel)
            .Include(v => v.VehicleType)
            .ThenInclude(v => v!.VehicleTypeName)
            .ThenInclude(v => v.Translations).Where(u => u.Driver!.AppUserId.Equals(userId));
        return query;
    }
}