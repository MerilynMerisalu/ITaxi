
using App.Domain.DTO.AdminArea;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface ICountyRepository : IEntityRepository<CountyDTO>
{
    Task<IEnumerable<DTO.AdminArea.CountyDTO>> GetAllCountiesOrderedByCountyNameAsync(bool noTracking = true);
    IEnumerable<DTO.AdminArea.CountyDTO> GetAllCountiesOrderedByCountyName(bool noTracking = true);
}