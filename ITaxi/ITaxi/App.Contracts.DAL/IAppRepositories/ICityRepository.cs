
using App.DTO.AdminArea;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface ICityRepository : IEntityRepository<CityDTO>
{
    Task<IEnumerable<CityDTO>> GetAllCitiesWithoutCountyAsync();
    Task<IEnumerable<CityDTO>> GetAllOrderedCitiesWithoutCountyAsync();
    Task<IEnumerable<CityDTO>> GetAllOrderedCitiesAsync();
    Task<CityDTO?> FirstOrDefaultCityWithoutCountyAsync(Guid id);
    IEnumerable<CityDTO> GetAllOrderedCitiesWithoutCounty();
   
}