using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.BLL;
using Base.Contracts.Mappers;

namespace App.BLL.Services;

public class CityService : BaseEntityService<App.BLL.DTO.AdminArea.CityDTO, DAL.DTO.AdminArea.CityDTO,
    ICityRepository>, ICityService
{
    public CityService(ICityRepository repository, IMapper<CityDTO, DAL.DTO.AdminArea.CityDTO> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<CityDTO>> GetAllCitiesWithoutCountyAsync()
    {
        return (await Repository.GetAllCitiesWithoutCountyAsync()).Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<CityDTO>> GetAllOrderedCitiesWithoutCountyAsync()
    {
        return (await Repository.GetAllOrderedCitiesWithoutCountyAsync()).Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<CityDTO>> GetAllOrderedCitiesAsync(bool showDeleted = false, bool showIgnored = false)
    {
        return (await Repository.GetAllOrderedCitiesAsync(showIgnored: showIgnored, showDeleted: showDeleted)).Select(e => Mapper.Map(e))!;
    }

    public async Task<CityDTO?> FirstOrDefaultCityWithoutCountyAsync(Guid id)
    {
        return Mapper.Map(await Repository.FirstOrDefaultCityWithoutCountyAsync(id));
    }

    public IEnumerable<CityDTO> GetAllOrderedCitiesWithoutCounty()
    {
        return Repository.GetAllOrderedCitiesWithoutCounty().Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<CityDTO> GetAllOrderedCities(bool noTracking = true)
    {
        return Repository.GetAllOrderedCities(noTracking).Select(e => Mapper.Map(e))!;
    }

    public async Task<bool> HasAnyCitiesAsync(Guid id, bool noTracking = true)
    {
        return await Repository.HasAnyCitiesAsync(id, noTracking);
    }

    public bool HasAnyCities(Guid id, bool noTracking = true)
    {
        return Repository.HasAnyCities(id, noTracking);
    }

    public async Task<CityDTO?> GetCityByIdAsync(Guid id, bool noTracking = false, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false)
    {
        return Mapper.Map(await Repository.GetCityByIdAsync(id: id, noTracking: noTracking, noIncludes: noIncludes, showDeleted: showDeleted, showIgnored: showIgnored));
    }
}