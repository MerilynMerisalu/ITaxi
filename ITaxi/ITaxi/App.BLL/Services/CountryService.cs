using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.BLL;
using Microsoft.IdentityModel.Tokens;
using RESTCountries.NET.Models;
using RESTCountries.NET.Services;
using System.Globalization;
using System.Linq;
using App.Enum.Enum;
using Base.Domain;
using Base.Contracts.Mappers;

namespace App.BLL.Services;

public class CountryService : BaseEntityService<App.BLL.DTO.AdminArea.CountryDTO, DAL.DTO.AdminArea.CountryDTO,
    ICountryRepository>, ICountryService
{
    public CountryService(ICountryRepository repository, IMapper<CountryDTO, DAL.DTO.AdminArea.CountryDTO> mapper) :
        base(repository, mapper)
    {
    }

    public async Task<IEnumerable<CountryDTO>> GetAllCountriesOrderedByCountryNameAsync(
        bool noTracking = true, bool noIncludes = false, bool showIgnored = false)
    {
        return (await Repository.GetAllCountriesOrderedByCountryNameAsync(noTracking: noTracking, noIncludes: noIncludes, showIgnored: showIgnored))
            .Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<CountryDTO> GetAllCountriesOrderedByCountryName(
        bool noTracking = true, bool noIncludes = false, bool showIgnored = false, bool showDeleted = false)
    {
        return Repository.GetAllCountriesOrderedByCountryName(noTracking: noTracking, noIncludes: noIncludes, showIgnored: showIgnored)
            .Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<CountryDTO>> GetAllCountriesOrderedByCountryISOCodeAsync(bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = true)
    {
        return (await Repository.GetAllCountriesOrderedByCountryISOCodeAsync(noTracking, noIncludes, showDeleted,showIgnored ))
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

    public async Task<CountryDTO?> GetCountryByISOCodeAsync(string isoCode, bool noTracking = true, bool noIncludes = false, bool showDeleted = true)
    {
        return Mapper.Map(await Repository.GetCountryByISOCodeAsync(isoCode, noTracking, noIncludes, showDeleted));
    }

    public async Task<CountryDTO?> ToggleIsIgnoredAsync(Guid id, bool noTracking = true, bool noIncludes = false)
    {
        return Mapper.Map(await Repository.ToggleIsIgnoredAsync(id, noTracking, noIncludes));
        
    }

    /*public async Task<DAL.DTO.AdminArea.CountryDTO?> GetCountryByISOCodeAsync(string isoCode, bool noTracking = true, bool noIncludes = false, bool showDeleted = true)
    {
        return await Repository.GetCountryByISOCodeAsync(isoCode, noTracking, noIncludes, showDeleted);
    }*/

    /*public async Task<DAL.DTO.AdminArea.CountryDTO?> ToggleCountryIsIgnoredAsync(Guid id, bool noTracking = true, bool noIncludes = false)
    {
        var country = await FirstOrDefaultAsync(id, noTracking, noIncludes);
        if (country == null)
        {
            return null;
        }
        country.IsIgnored = !country.IsIgnored;

        var countryDto = Mapper.Map(country);

        Repository.Update(countryDto);
        
        return countryDto;
        
        //return Mapper.Map(await Repository.ToggleCountryIsIgnoredAsync(id, noTracking, noIncludes));
    }
    */

   
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
            var supportedCountriesISOCodes = new List<string?>() { "EE" };
            country.Cca3 = country.Cca3.ToUpper();
            var existingCountryDTO = await Repository.GetCountryByISOCodeAsync(country.Cca3);

            var countryDTO = new CountryDTO();

            if (existingCountryDTO != null) // we are updating a country
            {
                if (existingCountryDTO.IsDeleted == true)
                {
                    
                    existingCountryDTO.IsDeleted = false;
                    Repository.Update(existingCountryDTO);
                    
                    
                }
                //Repository.Update(existingCountryDTO);
                countryDTO = Mapper.Map(existingCountryDTO);
                
            }
            else // adding a new country
            {
                countryDTO.Id = Guid.NewGuid();
                countryDTO.ISOCode = country.Cca2;
                countryDTO.DataOrigin = DataOrigin.Api;
                countryDTO.CreatedAt = DateTime.Now.ToUniversalTime();
                countryDTO.IsRegistrationSupported = supportedCountriesISOCodes.Any(s => s!.ToUpperInvariant().Equals(countryDTO.ISOCode.ToUpperInvariant()));
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
            ISOCode = c.Cca2,
            DataOrigin = DataOrigin.Api,
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

    public IEnumerable<CountryDTO> GetAllCountriesOrderedByCountryISOCode(bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = true)
    {
        return(Repository.GetAllCountriesOrderedByCountryISOCode(noTracking, noIncludes, showDeleted, showIgnored)
            .Select(e => Mapper.Map(e)).ToList());
    }

    public async Task<CountryDTO?> GetCountryByISOCodeAsync(string isoCode, bool noTracking = true, bool noIncludes = false, bool showDeleted = true, bool showIgnored = true)
    {
        return Mapper.Map(await Repository.GetCountryByISOCodeAsync(isoCode, noIncludes, showDeleted, showIgnored));
    }

    public CountryDTO? GetCountryByISOCode(string isoCode, bool noTracking = true, bool noIncludes = false, bool showDeleted = true, bool showIgnored = true)
    {
        return Mapper.Map(Repository.GetCountryByISOCode(isoCode, noTracking, noIncludes, showDeleted, showIgnored));
    }
    public async Task<bool> IsThereACorrespondingCountryToTheISO2CodeAsync(string iso2Code, string? userId = null, string? roleName = null,bool showDeleted = true, bool showIgnored = true)
    {
        var result = await Repository.IsThereACorrespondingCountryToTheISO2CodeAsync(iso2Code, userId, roleName, showDeleted: showDeleted, showIgnored: showIgnored);
        return result;
    }

    public bool IsThereACorrespondingCountryToTheISO2Code(string iso2Code, string? userId = null, string? roleName = null, bool showDeleted = true, bool showIgnored = true)
    
    {
        return Repository.IsThereACorrespondingCountryToTheISO2Code(iso2Code, userId, roleName, showDeleted: showDeleted, showIgnored: showIgnored);
    }

    public async Task<Guid?> GetCountryIdByISOCodeAsync(string iso2Code, string? userId = null, string? roleName = null)
    {
        return await Repository.GetCountryIdByISOCodeAsync(iso2Code, userId, roleName);
    }

    public Guid? GetCountryIdByISOCode(string iso2Code, string? userId = null, string? roleName = null)
    {
        return Repository.GetCountryIdByISOCode(iso2Code, userId, roleName);
    }

    public async Task<IEnumerable<CountryDTO?>> GetAllCountriesWhereIsRegisterSupportedAsync(bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false, bool showIsRegisterSupport = false)
    {
        var result = (await Repository.GetAllCountriesWhereIsRegisterSupportedAsync(noTracking: noTracking, noIncludes: noIncludes, showDeleted: showDeleted, showIgnored: showIgnored)).Select(c => Mapper.Map(c));
        return result;
    }

    public IEnumerable<CountryDTO?> GetAllCountriesWhereIsRegisterSupported(bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false, bool showIsRegisterSupport = false)
    {
        var result = (Repository.GetAllCountriesWhereIsRegisterSupported(noTracking: noTracking, noIncludes: noIncludes, showDeleted: showDeleted, showIgnored: showIgnored)).Select(c => Mapper.Map(c));
        return result;
    }
}