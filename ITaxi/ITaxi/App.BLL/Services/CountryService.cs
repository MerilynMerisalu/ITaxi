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

    public async Task<IEnumerable<CountryDTO>> GetAllCountriesOrderedByCountryNameAsync(bool noTracking = true)
    {
        return (await Repository.GetAllCountriesOrderedByCountryNameAsync(noTracking))
            .Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<CountryDTO> GetAllCountriesOrderedByCountryName(bool noTracking = true)
    {
        return Repository.GetAllCountriesOrderedByCountryName(noTracking)
            .Select(e => Mapper.Map(e))!;
    }
}