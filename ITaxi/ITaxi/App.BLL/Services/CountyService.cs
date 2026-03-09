using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class CountyService: BaseEntityService<App.BLL.DTO.AdminArea.CountyDTO, DAL.DTO.AdminArea.CountyDTO, ICountyRepository>
, ICountyService
{
    private readonly AppBLL _appBLL;
    public CountyService(ICountyRepository repository, IMapper<CountyDTO, DAL.DTO.AdminArea.CountyDTO> mapper, AppBLL appBLL) : base(repository, mapper)
    {
        _appBLL = appBLL;
    }

    public async Task<IEnumerable<CountyDTO>> GetAllCountiesOrderedByCountyNameAsync(bool noTracking = true, bool noIncludes = false)
    {
        return (await Repository.GetAllCountiesOrderedByCountyNameAsync(noTracking, noIncludes))
            .Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<CountyDTO> GetAllCountiesOrderedByCountyName(bool noTracking = true, bool noIncludes = false)
    {
        return Repository.GetAllCountiesOrderedByCountyName(noTracking, noIncludes).Select(e => Mapper.Map(e))!;
    }

    /*public async Task<IEnumerable<CountyDTO>> GetAllCountiesOrderedByCountryISOCodeAsync(bool noTracking = true, bool noIncludes = false)
    {
        return (await Repository.GetAllCountiesOrderedByCountryISOCodeAsync(noTracking, noIncludes))
            .Select(e => Mapper.Map(e))!;
    }
    */

    /*public IEnumerable<CountyDTO> GetAllCountiesOrderedByCountryISOCode(bool noTracking = true, bool noIncludes = false)
    {
        return Repository.GetAllCountiesOrderedByCountryISOCode(noTracking, noIncludes).Select(e => Mapper.Map(e))!;
    }*/

    public Task<bool> HasCities(Guid countyId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ImportCountiesFromEHAKAsync(HttpClient client)
    {
        const string ESTONIANISO2CODE = "EE";
        var result = await _appBLL.Countries.IsThereACorrespondingCountryToTheISO2CodeAsync(iso2Code: ESTONIANISO2CODE);
        if (!result)
        {
            return false;
        }

        const string AADRESSURL = "https://gsavalik.envir.ee/geoserver/ehak/wfs" +
              "?service=WFS&version=1.1.0" +
              "&request=GetFeature" +
              "&typeName=ehak:maakondade_piirid" +
              "&outputFormat=application/json";
        var response = await client.GetAsync(AADRESSURL);
        if(!response.IsSuccessStatusCode) return false;

        return true;
    }

    public Task<IEnumerable<CountyDTO>> GetCountiesByCountryIdAsync(Guid countryId)
    {
        throw new NotImplementedException();
    }
}