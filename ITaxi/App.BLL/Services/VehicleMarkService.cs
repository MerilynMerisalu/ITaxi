using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class VehicleMarkService: BaseEntityService<App.BLL.DTO.AdminArea.VehicleMarkDTO, 
    App.DAL.DTO.AdminArea.VehicleMarkDTO, 
    IVehicleMarkRepository>, IVehicleMarkService
{
    public VehicleMarkService(IVehicleMarkRepository repository, IMapper<VehicleMarkDTO, DAL.DTO.AdminArea.VehicleMarkDTO> mapper) : base(repository, mapper)
    {
    }


    public async Task<IEnumerable<VehicleMarkDTO>> GetAllVehicleMarkOrderedAsync(bool noTracking = true)
    {
        return (await Repository.GetAllVehicleMarkOrderedAsync(noTracking)).Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<VehicleMarkDTO> GetAllVehicleMarkOrdered(bool noTracking = true)
    {
        return Repository.GetAllVehicleMarkOrdered(noTracking).Select(e => Mapper.Map(e))!;
    }
}