using App.DTO.AdminArea;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface ICountyRepository : IEntityRepository<CountyDTO>
{
    Task<IEnumerable<CountyDTO>> GetAllCountiesOrderedByCountyNameAsync(bool noTracking = true);
    IEnumerable<CountyDTO> GetAllCountiesOrderedByCountyName(bool noTracking = true);
}