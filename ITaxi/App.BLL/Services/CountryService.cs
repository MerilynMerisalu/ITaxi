using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class CountryService : BaseEntityService<App.BLL.DTO.AdminArea.CountryDTO, DAL.DTO.AdminArea.CountryDTO,
    ICountryRepository>, ICountryService
{
    public CountryService(ICountryRepository repository, IMapper<CountryDTO, DAL.DTO.AdminArea.CountryDTO> mapper) :
        base(repository, mapper)
    {
    }

    public async Task<IEnumerable<CountryDTO>> GetAllCountriesOrderedByCountryNameAsync(
        bool noTracking = true, bool noIncludes = false)
    {
        return (await Repository.GetAllCountriesOrderedByCountryNameAsync(noTracking, noIncludes))
            .Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<CountryDTO> GetAllCountriesOrderedByCountryName(
        bool noTracking = true, bool noIncludes = false)
    {
        return Repository.GetAllCountriesOrderedByCountryName(noTracking, noIncludes)
            .Select(e => Mapper.Map(e))!;
    }

    public async Task<bool> HasAnyCountiesAsync(Guid id, bool noTracking = true)
    {
        return await Repository.HasAnyCountiesAsync(id, noTracking);
    }

    public bool HasAnyCounties(Guid id, bool noTracking = true)
    {
        return Repository.HasAnyCounties(id, noTracking);
    }

    public async Task<IEnumerable<CountryDTO>> GetAllCountiesOrderedByCountryNameAsync(bool noTracking = true, bool noIncludes = false)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<CountryDTO> GetAllCountiesOrderedByCountryName(bool noTracking = true, bool noIncludes = false)
    {
        throw new NotImplementedException();
    }
}