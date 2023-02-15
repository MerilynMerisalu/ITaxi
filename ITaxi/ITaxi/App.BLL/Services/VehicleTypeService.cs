using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class VehicleTypeService: BaseEntityService<App.BLL.DTO.AdminArea.VehicleTypeDTO, 
    App.DAL.DTO.AdminArea.VehicleTypeDTO, IVehicleTypeRepository>, IVehicleTypeService
{
    public VehicleTypeService(IVehicleTypeRepository repository, IMapper<VehicleTypeDTO, DAL.DTO.AdminArea.VehicleTypeDTO> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<VehicleTypeDTO>> GetAllVehicleTypesOrderedAsync(bool noTracking = true)
    {
        return (await Repository.GetAllVehicleTypesOrderedAsync(noTracking))
            .Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<VehicleTypeDTO> GetAllVehicleTypesOrdered(bool noTracking = true)
    {
        return Repository.GetAllVehicleTypesOrdered(noTracking).Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<VehicleTypeDTO>> GetAllVehicleTypesDTOAsync(bool noTracking = true)
    {
        return (await Repository.GetAllVehicleTypesDTOAsync(noTracking))
            .Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<VehicleTypeDTO> GetAllVehicleTypesDTO(bool noTracking = true)
    {
        return Repository.GetAllVehicleTypesDTO(noTracking).Select(e => Mapper.Map(e))!;
    }

    public async Task<bool> HasVehiclesAnyAsync(Guid vehicleTypeId, bool noTracking = true)
    {
        return await Repository.HasVehiclesAnyAsync(vehicleTypeId, noTracking);
    }

    public bool HasVehiclesAny(Guid vehicleTypeId, bool noTracking = true)
    {
        return Repository.HasVehiclesAny(vehicleTypeId, noTracking);
    }

    public async Task<bool> HasBookingsAnyAsync(Guid vehicleTypeId, bool noTracking = true)
    {
        return await Repository.HasBookingsAnyAsync(vehicleTypeId, noTracking);
    }

    public bool HasBookingsAny(Guid vehicleTypeId, bool noTracking = true)
    {
        return Repository.HasBookingsAny(vehicleTypeId, noTracking);
    }
}