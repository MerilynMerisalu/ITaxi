using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.Domain;
using App.Enum.Enum;
using Base.Contracts.Mappers;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class VehicleRepository : BaseEntityRepository<VehicleDTO, Vehicle, AppDbContext>, IVehicleRepository
{
    public VehicleRepository(AppDbContext dbContext, IMapper<VehicleDTO, App.Domain.Vehicle> mapper) : base(dbContext,
        mapper)
    {
    }

    public override async Task<IEnumerable<VehicleDTO>> GetAllAsync(bool noTracking = true)
    {
        return (await CreateQuery(null, null, noTracking).ToListAsync())
            .Select(e => Mapper.Map(e))!;
    }

    public override IEnumerable<VehicleDTO> GetAll(bool noTracking = true)
    {
        return CreateQuery(null, null, noTracking).ToList().Select(e => Mapper.Map(e))!;
    }

    public override async Task<VehicleDTO?> FirstOrDefaultAsync(Guid id, bool noTracking = true, bool noIncludes = false, bool showIgnored = false, bool showDeleted = false)
    {
        return Mapper.Map(await CreateQuery(null, null, noTracking, noIncludes, showIgnored: showIgnored, showDeleted: showDeleted).FirstOrDefaultAsync(v => v.Id.Equals(id)));
    }

    public override VehicleDTO? FirstOrDefault(Guid id, bool noTracking = true, bool noIncludes = false, bool showIgnored = false, bool showDeleted = false)
    {
        return Mapper.Map(CreateQuery(null, null, noTracking, noIncludes, showDeleted: showDeleted).FirstOrDefault(v => v.Id.Equals(id)));
    }

    public async Task<IEnumerable<VehicleDTO>> GettingOrderedVehiclesAsync(Guid? userId, string? roleName =
        null, bool noTracking = true)
    {
        var res = await CreateQuery(userId, roleName, noTracking)
            .OrderBy(v => v.VehicleType!.VehicleTypeName)
            .ThenBy(v => v.VehicleMark!.VehicleMarkName)
            .ThenBy(v => v.VehicleModel!.VehicleModelName)
            .ThenBy(v => v.ManufactureYear)
            .ToListAsync();


        return res.Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<VehicleDTO> GettingOrderedVehicles(Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        return CreateQuery(userId, roleName, noTracking)
            .OrderBy(v => v.VehicleType!.VehicleTypeName)
            .ThenBy(v => v.VehicleMark!.VehicleMarkName)
            .ThenBy(v => v.VehicleModel!.VehicleModelName)
            .ThenBy(v => v.ManufactureYear).ToList()
            .Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<VehicleDTO>> GettingVehiclesWithoutIncludesAsync(
        bool noTracking = true)
    {
        return (await base.CreateQuery(noTracking, noIncludes: true).ToListAsync()).Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<VehicleDTO> GettingVehiclesWithoutIncludes(bool noTracking = true)
    {
        return base.CreateQuery(noTracking, noIncludes: true).ToList().Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<VehicleDTO>> GettingOrderedVehiclesWithoutIncludesAsync(Guid? userId = null,
        string? roleName = null, bool noTracking = true)
    {
        if (userId == null)
        {
            return (await CreateQuery(noTracking, noIncludes: false).OrderBy(v => v.ManufactureYear).ToListAsync())
                .Select(e => Mapper.Map(e))!;
        }

        return (await CreateQuery(noTracking, noIncludes: false)
                .Where(d => d.Driver!.AppUserId.Equals(userId))
                .OrderBy(v => v.ManufactureYear).ToListAsync())
            .Select(e => Mapper.Map(e))!;

    }

    public IEnumerable<VehicleDTO> GettingOrderedVehiclesWithoutIncludes(bool noTracking = true)
    {
        return CreateQuery(noTracking, noIncludes: true)
            .OrderBy(v => v.ManufactureYear).ToList()
            .Select(e => Mapper.Map(e))!;
    }

    public async Task<VehicleDTO?>? GettingVehicleWithIncludesByIdAsync(Guid id, Guid? userId = null,
        string? roleName = null, bool noTracking = true)
    {
        var vehicle = await CreateQuery(userId, roleName, noTracking).FirstOrDefaultAsync(v => v.Id.Equals(id));

        return Mapper.Map(vehicle);
    }

    public async Task<VehicleDTO?> GettingVehicleWithoutIncludesByIdAsync(Guid id, Guid? userId = null,
        string? roleName = null, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(userId, roleName, noTracking, true).FirstOrDefaultAsync(v => v.Id.Equals(id)));
    }

    public VehicleDTO? GettingVehicleWithoutIncludesById(Guid id, bool noTracking = true)
    {
        return Mapper.Map(CreateQuery(noTracking, noIncludes: true).FirstOrDefault(v => v.Id.Equals(id)));
    }

    public List<int> GettingManufactureYears()
    {
        var years = new List<int>();

        for (var i = 6; i > 0; i--)
        {
            var year = DateTime.Today.AddYears(1).AddYears(-i).Year;
            years.Add(year);
        }

        years.Reverse();

        return years;
    }

    public async Task<VehicleDTO?> 
        GettingVehicleWithoutIncludesByDriverIdAndVehicleAvailabilityAsync(BookingDTO booking)
    {
        return Mapper.Map(await CreateQuery()
            .Where(v => v.DriverId.Equals(booking.DriverId)
                        && v.VehicleAvailability == VehicleAvailability.Available)
            .FirstAsync());
    }

    public VehicleDTO? GettingVehicleWithoutIncludesByDriverIdAndVehicleAvailability(BookingDTO booking)
    {
        return Mapper.Map(CreateQuery(noIncludes: true)
            .First(v => v.DriverId.Equals(booking.DriverId)
                        && v.VehicleAvailability == VehicleAvailability.Available));
    }

    public async Task<List<VehicleDTO>> GettingVehiclesByDriverIdAsync(Guid driverId, Guid? userId = null,
        string? roleName = null, bool noTracking = true)
    {
        return ((await CreateQuery(userId, roleName, noTracking).Where(v => v.DriverId.Equals(driverId)).ToListAsync())
            .Select(e => Mapper.Map(e)).ToList())!;
    }


    public List<VehicleDTO> GettingVehiclesByDriverId(Guid driverId, bool noTracking = true)
    {
        var vehicles = CreateQuery(noTracking).Where(v => v.DriverId.Equals(driverId)).ToList();
        return vehicles.Select(e => Mapper.Map(e)).ToList()!;
    }

    public async Task<bool> HasAnyVehicleModelsAnyAsync(Guid vehicleModelId, bool noTracking)
    {
        return await CreateQuery(noTracking).AnyAsync(mo => mo.VehicleModelId.Equals(vehicleModelId));
    }

    public bool HasAnyVehicleModelsAny(Guid vehicleModelId, bool noTracking = true)
    {
        return CreateQuery(noTracking).Any(v => v.VehicleModelId.Equals(vehicleModelId));
    }

    public async Task<bool> HasAnySchedulesAnyAsync(Guid vehicleId, bool noTracking = true)
    {
        return await
            RepoDbContext.Schedules
                .AnyAsync(s => s.VehicleId.Equals(vehicleId));
    }

    public bool HasAnySchedulesAny(Guid vehicleId, bool noTracking = true)
    {
        return
            RepoDbContext.Schedules
                .Any(s => s.VehicleId.Equals(vehicleId));
    }

    public async Task<bool> HasAnyBookingsAnyAsync(Guid vehicleId, bool noTracking = true)
    {
        return await RepoDbContext.Bookings.AnyAsync(b => b.VehicleId.Equals(vehicleId));
    }

    public bool HasAnyBookingsAny(Guid vehicleId, bool noTracking = true)
    {
        return RepoDbContext.Bookings.Any(b => b.VehicleId.Equals(vehicleId));
    }

    public async Task<Guid?> GetDriverIdByVehicleIdAsync(Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true,
        bool noIncludes = false)
    {
        return (await CreateQuery(userId, roleName, noTracking)
            .SingleAsync(v => v.Id.Equals(vehicleId))).DriverId;
    }

    public Guid? GetDriverIdByVehicleId(Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true,
        bool noIncludes = false)
    {
        return CreateQuery(userId, roleName, noTracking)
            .Single(v => v.Id.Equals(vehicleId)).DriverId;
    }


    

    protected IQueryable<Vehicle> CreateQuery(Guid? userId = null, string? roleName = null,
        bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false)
    {
        var query = CreateQuery(noTracking, noIncludes, showDeleted, showIgnored: showIgnored);
        if (noTracking) query = query.AsNoTracking();

        if (!noIncludes)
        {
            if (roleName == null)
            {
                query = query.Include(c => c.Driver)
                    .ThenInclude(d => d!.AppUser)
                    .Include(v => v.VehicleMark)
                    .Include(v => v.VehicleModel)
                    .Include(v => v.VehicleType)
                    .ThenInclude(v => v!.VehicleTypeName)
                    .ThenInclude(v => v.Translations)
                    .Include(v => v.VehiclePhotos)
                    .AsSplitQuery();
                return query;
            }
            else if (roleName.Equals("Driver"))
            {
                query = query.Include(c => c.Driver)
                    .ThenInclude(d => d!.AppUser)
                    .Include(v => v.VehicleMark)
                    .Include(v => v.VehicleModel)
                    .Include(v => v.VehicleType)
                    .ThenInclude(v => v!.VehicleTypeName)
                    .ThenInclude(v => v.Translations)
                    .Include(v => v.VehiclePhotos)
                    .Where(u => u.Driver!.AppUserId.Equals(userId));
            }
        }
        return query;
    }

    public async Task<string?> GetVehicleTypeNameByVehicleIdAsync(Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
    {
        var result = await CreateQuery(userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes)
            .FirstOrDefaultAsync(v => v.Id.Equals(vehicleId));
        return result!.VehicleType?.VehicleTypeName;
    }

    public string? GetVehicleTypeNameByVehicleId(Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
    {
        var result = CreateQuery(userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes)
           .FirstOrDefault(v => v.Id.Equals(vehicleId));
        return result!.VehicleType?.VehicleTypeName;
    }

    public async Task<string> GetVehicleMarkNameByVehicleIdAsync(Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
    {
        var result = await CreateQuery(userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes)
            .FirstOrDefaultAsync(v => v.Id.Equals(vehicleId));
        return result!.VehicleMark!.VehicleMarkName;
    }

    public string GetVehicleMarkNameByVehicleId(Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
    {
        var result =  CreateQuery(userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes)
            .FirstOrDefault(v => v.Id.Equals(vehicleId));
        return result!.VehicleMark!.VehicleMarkName;
    }

    public async Task<string> GetVehicleModelNameByVehicleIdAsync(Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
    {
        var result = await CreateQuery(userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes)
            .FirstOrDefaultAsync(v => v.Id.Equals(vehicleId));
        return result!.VehicleModel!.VehicleModelName;
    }

    public string GetVehicleModelNameByVehicleId(Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
    {
        var result = CreateQuery(userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes)
           .FirstOrDefault(v => v.Id.Equals(vehicleId));
        return result!.VehicleModel!.VehicleModelName;
    }

    public async Task<string> GetVehiclePlateNumberByVehicleIdAsync(Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
    {
        var result = await CreateQuery(userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes)
           .FirstOrDefaultAsync(v => v.Id.Equals(vehicleId));
        return result!.VehiclePlateNumber;
    }

    public string GetVehiclePlateNumberByVehicleId(Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
    {
        var result =  CreateQuery(userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes)
           .FirstOrDefault(v => v.Id.Equals(vehicleId));
        return result!.VehiclePlateNumber;
    }

    public async Task<string?> GetVehicleDriverByVehicleIdAsync(Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
    {
        var result = await CreateQuery(userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes)
            .FirstOrDefaultAsync(v => v.Id.Equals(vehicleId));
        return result?.Driver?.AppUser?.FirstAndLastName;
    }

    public string? GetVehicleDriverByVehicleId(Guid vehicleId, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
    {
        var result = CreateQuery(userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes)
            .FirstOrDefault(v => v.Id.Equals(vehicleId));
        return result?.Driver?.AppUser?.FirstAndLastName;
    }
}