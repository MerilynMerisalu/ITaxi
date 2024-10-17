using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.BLL;
using Base.Contracts;
using Microsoft.IdentityModel.Tokens;
using RESTCountries.NET.Models;
using RESTCountries.NET.Services;
using System.Globalization;
using System.Linq;
using Base.Domain;

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

    public async Task<IEnumerable<CountryDTO>> GetAllCountriesOrderedByCountryISOCodeAsync(bool noTracking = true, bool noIncludes = false)
    {
        return (await Repository.GetAllCountriesOrderedByCountryISOCodeAsync(noTracking, noIncludes))
            .Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<CountryDTO> GetAllCountriesOrderedByCountryISOCode(bool noTracking = true, bool noIncludes = false)
    {
        return Repository.GetAllCountriesOrderedByCountryISOCode(noTracking, noIncludes)
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

    public async Task<DAL.DTO.AdminArea.CountryDTO?> GetCountryByISOCodeAsync(string isoCode, bool noTracking = true, bool noIncludes = false)
    {
        return await Repository.GetCountryByISOCodeAsync(isoCode, noTracking, noIncludes);
    }

    // public void UpdateCountriesFromAPI(string[] langCodes)
    //      - get the countries from the api
    //              - get all the translations based on langCodes
    //      - save each one to the db
    //              - does the country existing in the db
    //              - create or update
    
    // public void GetAllCountries(langCode)
    //      - return all from db

    // UpadetCountriesResult { bool Success, List<string> Errors }


    public async Task UpdateCountriesFromAPIAsync(CultureInfo[] cultures)//, string[] langCodes)
    {
        if (cultures == null) //langCodes == null)
        {
            return;
        }

        var countries = RestCountriesService.GetAllCountries();

        if (countries == null)
        {
            // maybe log error
            // 
            return;
        }

        foreach (var country in countries)
        {
            country.Cca3 = country.Cca3.ToUpper();
            var existingCountryDTO = await Repository.GetCountryByISOCodeAsync(country.Cca3);

            var countryDTO = new CountryDTO();

            if (existingCountryDTO != null) // we are updating a country
            {
                countryDTO = Mapper.Map(existingCountryDTO);
            }
            else // adding a new country
            {
                countryDTO.Id = Guid.NewGuid();
                countryDTO.ISOCode = country.Cca3;
                countryDTO.CreatedAt = DateTime.Now.ToUniversalTime();
            }

            foreach (var langCode in cultures) //langCodes)
            {
                if (langCode == null) //string.IsNullOrEmpty(langCode))
                {
                    continue;
                }
                if (countryDTO.CountryName == null)
                {
                    countryDTO.CountryName = new LangStr();
                }
                if (country.Translations.ContainsKey(langCode.ThreeLetterISOLanguageName)) // eng  en-GB
                {
                    var translation = country.Translations[langCode.ThreeLetterISOLanguageName].Common;
                    
                    countryDTO.CountryName.SetTranslation(translation, langCode.Name);
                }
                else if (langCode.ThreeLetterISOLanguageName == "eng")
                {
                    countryDTO.CountryName.SetTranslation(country.Name.Common, langCode.Name);
                }
                else if (country.Name.NativeName?.ContainsKey(langCode.ThreeLetterISOLanguageName) ?? false)
                {
                    var translation = country.Name.NativeName[langCode.ThreeLetterISOLanguageName].Common;

                    countryDTO.CountryName.SetTranslation(translation, langCode.Name);
                }
            }
            countryDTO.UpdatedAt = DateTime.Now.ToUniversalTime();

            if (existingCountryDTO != null)
            {
                Repository.Update(Mapper.Map(countryDTO));
            }
            else
            {
                Repository.Add(Mapper.Map(countryDTO));
            }
        }
        
    }
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