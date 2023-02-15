using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.BLL;
using Base.Contracts;
using BookingDTO = App.DAL.DTO.AdminArea.BookingDTO;

namespace App.BLL.Services;

public class VehicleService : BaseEntityService<App.BLL.DTO.AdminArea.VehicleDTO, App.DAL.DTO.AdminArea.VehicleDTO, 
    IVehicleRepository>, IVehicleService
{
    public VehicleService(IVehicleRepository repository, IMapper<VehicleDTO, DAL.DTO.AdminArea.VehicleDTO> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<VehicleDTO>> GettingOrderedVehiclesAsync(Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        var res =
            await Repository.GettingOrderedVehiclesAsync(userId, roleName, noTracking);
        return res.Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<VehicleDTO> GettingOrderedVehicles(Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return Repository.GettingOrderedVehicles(userId, roleName, noTracking).Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<VehicleDTO>> GettingVehiclesWithoutIncludesAsync(bool noTracking = true)
    {
        return (await Repository.GettingVehiclesWithoutIncludesAsync(noTracking)).Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<VehicleDTO> GettingVehiclesWithoutIncludes(bool noTracking = true)
    {
        return Repository.GettingVehiclesWithoutIncludes(noTracking).Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<VehicleDTO>> GettingOrderedVehiclesWithoutIncludesAsync(Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return (await Repository.GettingOrderedVehiclesWithoutIncludesAsync(userId, roleName, noTracking)).Select(e =>
            Mapper.Map(e))!;
    }

    public IEnumerable<VehicleDTO> GettingOrderedVehiclesWithoutIncludes(bool noTracking = true)
    {
        return Repository.GettingOrderedVehiclesWithoutIncludes(noTracking).Select(e => Mapper.Map(e))!;
    }

    public async Task<VehicleDTO?> GettingVehicleWithIncludesByIdAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return Mapper.Map(await Repository.GettingVehicleWithIncludesByIdAsync(id, userId, roleName, noTracking));
    }

    public async Task<VehicleDTO?> GettingVehicleWithoutIncludesByIdAsync(Guid id, Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        return Mapper.Map(await Repository.GettingVehicleWithoutIncludesByIdAsync(id, userId, roleName, noTracking));
    }

    public VehicleDTO? GettingVehicleWithoutIncludesById(Guid id, bool noTracking = true)
    {
        return Mapper.Map(Repository.GettingVehicleWithoutIncludesById(id, noTracking));
    }

    public List<int> GettingManufactureYears()
    {
        return Repository.GettingManufactureYears();
    }

    public async Task<VehicleDTO?> GettingVehicleWithoutIncludesByDriverIdAndVehicleAvailabilityAsync(BookingDTO booking)
    {
        return Mapper.Map(await Repository.GettingVehicleWithoutIncludesByDriverIdAndVehicleAvailabilityAsync(booking));
    }

    public VehicleDTO? GettingVehicleWithoutIncludesByDriverIdAndVehicleAvailability(BookingDTO booking)
    {
        return Mapper.Map(Repository.GettingVehicleWithoutIncludesByDriverIdAndVehicleAvailability
            (booking));
    }

    
    public async Task<List<VehicleDTO>> GettingVehiclesByDriverIdAsync(Guid driverId, Guid? userId = null, string? roleName = null,
        bool noTracking = true)
    {
        return (await Repository.GettingVehiclesByDriverIdAsync(driverId, userId, roleName, noTracking))
            .Select(e => Mapper.Map(e))
            .ToList()!;
    }

    public List<VehicleDTO> GettingVehiclesByDriverId(Guid driverId, bool noTracking = true)
    {
        return Repository.GettingVehiclesByDriverId(driverId, noTracking).Select(e => Mapper.Map(e)).ToList()!;
    }

    public async Task<bool> HasAnyVehicleModelsAnyAsync(Guid vehicleModelId, bool noTracking)
    {
        return await Repository.HasAnyVehicleModelsAnyAsync(vehicleModelId, noTracking);
    }

    public bool HasAnyVehicleModelsAny(Guid vehicleModelId, bool noTracking = true)
    {
        return Repository.HasAnyVehicleModelsAny(vehicleModelId, noTracking);
    }

    public async Task<bool> HasAnySchedulesAnyAsync(Guid vehicleId, bool noTracking = true)
    {
        return await Repository.HasAnySchedulesAnyAsync(vehicleId, noTracking);
    }

    public bool HasAnySchedulesAny(Guid vehicleId, bool noTracking = true)
    {
        return Repository.HasAnySchedulesAny(vehicleId, noTracking);
    }

    public async Task<bool> HasAnyBookingsAnyAsync(Guid vehicleId, bool noTracking = true)
    {
        return await Repository.HasAnyBookingsAnyAsync(vehicleId, noTracking);
    }

    public bool HasAnyBookingsAny(Guid vehicleId, bool noTracking = true)
    {
        return Repository.HasAnyBookingsAny(vehicleId, noTracking);
    }
}