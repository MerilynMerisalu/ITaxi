using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class VehicleModelService : BaseEntityService<App.BLL.DTO.AdminArea.VehicleModelDTO, 
    App.DAL.DTO.AdminArea.VehicleModelDTO, IVehicleModelRepository>, IVehicleModelService
{
    public VehicleModelService(IVehicleModelRepository repository, IMapper<VehicleModelDTO, DAL.DTO.AdminArea.VehicleModelDTO> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<VehicleModelDTO>> GetAllVehicleModelsWithoutVehicleMarksAsync(bool noTracking = true)
    {
        return (await Repository.GetAllVehicleModelsWithoutVehicleMarksAsync(noTracking))
            .Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<VehicleModelDTO> GetAllVehicleModelsWithoutVehicleMarks(bool noTracking = true)
    {
        return Repository.GetAllVehicleModelsWithoutVehicleMarks(noTracking)
            .Select(e => Mapper.Map(e))!;
    }

    public async Task<VehicleModelDTO?> FirstOrDefaultVehicleModelWithoutVehicleMarkAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await Repository.FirstOrDefaultVehicleModelWithoutVehicleMarkAsync(id, noTracking));
    }

    public VehicleModelDTO? FirstOrDefaultVehicleModelWithoutVehicleMark(Guid id, bool noTracking = true)
    {
        return Mapper.Map(Repository.FirstOrDefaultVehicleModelWithoutVehicleMark(id, noTracking));
    }

    public async Task<IEnumerable<VehicleModelDTO>> GetAllVehicleModelsOrderedByVehicleMarkNameAsync(bool noTracking = true)
    {
        return (await Repository.GetAllVehicleModelsOrderedByVehicleMarkNameAsync(noTracking))
            .Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<VehicleModelDTO> GetAllVehicleModelsOrderedByVehicleMarkName(bool noTracking = true)
    {
        return Repository.GetAllVehicleModelsOrderedByVehicleMarkName(noTracking)
            .Select(e => Mapper.Map(e))!;
    }

    public async Task<List<VehicleModelDTO>> GettingVehicleModelsByMarkIdAsync(Guid markId, bool noTracking = true)
    {
        return ((await Repository.GettingVehicleModelsByMarkIdAsync(markId, noTracking))
            .Select(e => Mapper.Map(e)) as List<VehicleModelDTO>)!;
    }

    public List<VehicleModelDTO> GettingVehicleModels(Guid markId, bool noTracking = true)
    {
        return (Repository.GettingVehicleModels(markId, noTracking)
            .Select(e => Mapper.Map(e)) as List<VehicleModelDTO>)!;
    }
}