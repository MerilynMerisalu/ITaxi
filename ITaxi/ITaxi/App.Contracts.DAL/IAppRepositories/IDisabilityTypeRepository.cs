using App.Domain;
using App.Domain.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IDisabilityTypeRepository : IEntityRepository<DisabilityType>
{
    Task<IEnumerable<DisabilityTypeDTO>> GetAllDisabilityTypeDtoAsync(string? roleName = null, bool noTracking = true);
    IEnumerable<DisabilityTypeDTO> GetAllDisabilityTypeDto(string? roleName = null, bool noTracking = true);
    Task<IEnumerable<DisabilityType>> GetAllOrderedDisabilityTypesAsync(bool noTracking = true);
    IEnumerable<DisabilityType> GetAllOrderedDisabilityTypes(bool noTracking = true);
}