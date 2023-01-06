using App.DAL.DTO.AdminArea;
using App.Domain;
using App.Domain.DTO;
using App.Domain.DTO.AdminArea;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IVehicleTypeRepository : IEntityRepository<VehicleTypeDTO>, IVehicleTypeRepositoryCustom<VehicleTypeDTO>
{
    

}

public interface IVehicleTypeRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllVehicleTypesOrderedAsync(bool noTracking = true);
    IEnumerable<TEntity> GetAllVehicleTypesOrdered(bool noTracking = true);
    Task<IEnumerable<TEntity>> GetAllVehicleTypesDTOAsync(bool noTracking = true);
    IEnumerable<TEntity> GetAllVehicleTypesDTO(bool noTracking = true);
}