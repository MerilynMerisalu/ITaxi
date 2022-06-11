using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IDisabilityTypeRepository: IEntityRepository<DisabilityType>
{
    Task<IEnumerable<DisabilityType>> GetAllOrderedDisabilityTypesAsync(bool noTracking = true);
    IEnumerable<DisabilityType> GetAllOrderedDisabilityTypes(bool noTracking = true);
    
    
}