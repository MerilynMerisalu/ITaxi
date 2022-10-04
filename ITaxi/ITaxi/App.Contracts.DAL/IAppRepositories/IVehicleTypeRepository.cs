using App.Domain;
using App.Domain.DTO;
using App.Domain.DTO.AdminArea;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IVehicleTypeRepository : IEntityRepository<VehicleType>
{
    Task<IEnumerable<VehicleType>> GetAllVehicleTypesOrderedAsync(bool noTracking = true);
    IEnumerable<VehicleType> GetAllVehicleTypesOrdered(bool noTracking = true);

    Task<IEnumerable<VehicleTypeDTO>> GetAllVehicleTypesDTOAsync(bool noTracking = true);
    IEnumerable<VehicleTypeDTO> GetAllVehicleTypesDTO(bool noTracking = true);

}