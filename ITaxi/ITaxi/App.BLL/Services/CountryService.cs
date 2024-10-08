using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.BLL;
using Base.Contracts;
using Microsoft.IdentityModel.Tokens;
using RESTCountries.NET.Models;
using RESTCountries.NET.Services;

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
    
    // public void UpdateCountriesFromAPI(string[] langCodes)
    //      - get the countries from the api
    //              - get all the translations based on langCodes
    //      - save each one to the db
    //              - does the country existing in the db
    //              - create or update
    
    // public void GetAllCountries(langCode)
    //      - return all from db

    public IEnumerable<CountryDTO?> GetAllCountriesThroughRestAPI(string langCode = "eng")
    {
        var countries = RestCountriesService.GetAllCountries();
        return countries.Select(c => new CountryDTO()
        {
            Id = Guid.NewGuid(),
            CountryName = GetCountryCommonNameTranslated(langCode, c),
            ISOCode = c.Cca3,
            CreatedAt = DateTime.UtcNow.ToLocalTime(),
        });
    }

    public string? GetCountryCommonNameTranslated(string langCode, Country country)
    {
        if (string.IsNullOrEmpty(langCode))
        {
            langCode = "eng";
        }

        if (country == null)
        {
            return null;
        }

        langCode = langCode.ToLower();

        if (country.Translations.ContainsKey(langCode))
        {
            return country.Translations[langCode].Common;
        }

        return country.Name.Common;
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