using App.DAL.DTO.AdminArea;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IDisabilityTypeRepository : IEntityRepository<DisabilityTypeDTO>, IDisabilityTypeRepositoryCustom<DisabilityTypeDTO>
{
    
}
public interface IDisabilityTypeRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllDisabilityTypeDtoAsync(string? roleName = null, bool noTracking = true);
    IEnumerable<TEntity> GetAllDisabilityTypeDto(string? roleName = null, bool noTracking = true);
    Task<IEnumerable<TEntity>> GetAllOrderedDisabilityTypesAsync(bool noTracking = true);
    IEnumerable<TEntity> GetAllOrderedDisabilityTypes(bool noTracking = true);
}