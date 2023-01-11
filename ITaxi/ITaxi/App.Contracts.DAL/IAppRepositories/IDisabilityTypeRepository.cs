using App.DAL.DTO.AdminArea;
using App.Domain;
using App.Domain.DTO;
using App.Domain.DTO.AdminArea;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IDisabilityTypeRepository : IEntityRepository<DisabilityTypeDTO>
{
    Task<IEnumerable<DisabilityTypeDTO>> GetAllDisabilityTypeDtoAsync(string? roleName = null, bool noTracking = true);
    IEnumerable<DisabilityTypeDTO> GetAllDisabilityTypeDto(string? roleName = null, bool noTracking = true);
    Task<IEnumerable<DisabilityTypeDTO>> GetAllOrderedDisabilityTypesAsync(bool noTracking = true);
    IEnumerable<DisabilityTypeDTO> GetAllOrderedDisabilityTypes(bool noTracking = true);
}