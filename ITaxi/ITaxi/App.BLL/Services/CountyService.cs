using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Contracts.BLL.ImportResults;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using App.Enum.Enum;
using Base.BLL;
using Base.Contracts.Mappers;
using Base.Domain;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace App.BLL.Services;

public class CountyService: BaseEntityService<App.BLL.DTO.AdminArea.CountyDTO, DAL.DTO.AdminArea.CountyDTO, ICountyRepository>
, ICountyService
{
    private readonly IAppBLL _appBLL;
    private readonly ILogger<CountyService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    public CountyService(ICountyRepository repository, IMapper<CountyDTO, DAL.DTO.AdminArea.CountyDTO> mapper, ILogger<CountyService> logger, IAppBLL appBLL, IHttpClientFactory httpClientFactory) : base(repository, mapper)
    {

        _logger = logger;
        _appBLL = appBLL;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<CountyDTO>> GetAllCountiesOrderedByCountyNameAsync(bool noTracking = true, bool noIncludes = false, bool showIgnored = false)
    {
        return (await Repository.GetAllCountiesOrderedByCountyNameAsync(noTracking, noIncludes, showIgnored: showIgnored))
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

    public Task<bool> HasMunicipalities(Guid countyId)
    {
        throw new NotImplementedException();
    }

    public async Task<EhakImportResult> ImportCountiesFromEHAKAsync()
    {
        var client = _httpClientFactory.CreateClient();
        var importResult = new EhakImportResult();
        string? municipalityNameEn = null;
        string? municipalityNameEst = null;
        const string ESTONIANISO2CODE = "EE";
       
        var result = await _appBLL.Countries.IsThereACorrespondingCountryToTheISO2CodeAsync(iso2Code: ESTONIANISO2CODE);
        if (result == false)
        {
            importResult.ParentUnitNotFound = true;
            importResult.Success = false;
            _logger.LogWarning($"Country with ISO2 code {ESTONIANISO2CODE} could not be found in the database.");
            return importResult;
        }
        var countryId = await _appBLL.Countries.GetCountryIdByISOCodeCca2Async(ESTONIANISO2CODE);
        const string AADRESSURL = "https://gsavalik.envir.ee/geoserver/ehak/wfs" +
              "?service=WFS&version=1.1.0" +
              "&request=GetFeature" +
              "&typeName=ehak:maakondade_piirid" +
              "&outputFormat=application/json";
        var response = await client.GetAsync(AADRESSURL);
        if (!response.IsSuccessStatusCode)
        {
            importResult.Success = false;
            _logger.LogError($"EHAK unavailable. Status: {response.StatusCode}");
            return importResult;
        }
        var jsonResult = await response.Content.ReadAsStringAsync();
        var root = JObject.Parse(jsonResult);
        var features = root["features"];
        if (features != null)
        {
            foreach (var feature in features)
            {
                var props = feature["properties"] as JObject;
                if (props == null)
                {
                    _logger.LogError("Invalid EHAK API response. Feature without properties encountered.");
                    importResult.ApiError = true;
                    importResult.Success = false;
                    return importResult;
                }
                var countyName = props["maakond"]?.Value<string>();
                var ehakCode = props["ehak_kood"]?.Value<string>();
                _logger.LogInformation("Importing county {Name} with EHAK {Code}", countyName, ehakCode);
                if (string.IsNullOrWhiteSpace(countyName))
                {
                    _logger.LogWarning($"Skipping county due to missing required data. Name: {countyName}");
                    continue;
                }
                else if (string.IsNullOrWhiteSpace(ehakCode))
                {
                    _logger.LogWarning($"Skipping county due to missing required data. EhakCode: {ehakCode}");
                    continue;
                }

                if(countryId.HasValue && !string.IsNullOrWhiteSpace(ehakCode))
                {
                    var exists =
                        await _appBLL.Counties.DoesCountyExistsByCountryIdAndEHAKCodeAsync(countryId.Value, ehakCode);
                    if (exists) continue;

                    const string TRANSLATIONADDRESS = "https://klassifikaatorid.stat.ee/item/stat.ee/079ed389-81f0-4171-8442-d8cf7412cdb7/5/report/se.classification-version-data/json";
                    response = await client.GetAsync(TRANSLATIONADDRESS);
                    if (!response.IsSuccessStatusCode)
                    {
                        importResult.Success = false;
                        _logger.LogError($"EHAK unavailable. Status: {response.StatusCode}");
                        return importResult;
                    }
                    
                    }
                var jsonTranslationResult = await response.Content.ReadAsStringAsync();
                var rootTranslation = JObject.Parse(jsonTranslationResult);
                var translationFeatures = root["rootTranslation"];
                if (translationFeatures != null)
                {
                    var item = translationFeatures.FirstOrDefault(i => i["Code"]?.Value<string>() == ehakCode);
                    if (item == null)
                    {
                        _logger.LogWarning("Municipality with EHAK code {EhakCode} was not found in the translation source", ehakCode);
                    }
                    else
                    {
                        municipalityNameEn = item["Label"]?["en-GB"]?.Value<string>();
                        municipalityNameEst = item["Label"]?["et-EE"]?.Value<string>();

                        _logger.LogInformation("Importing municipality english name {municipalityNameEn}, estonian name {municipalityNameEst}", municipalityNameEn, municipalityNameEst);
                    }
                    var countyLangStr = new LangStr();

                    var county = new CountyDTO() { 
                        Id = Guid.NewGuid(), 
                        CountyName = countyName, 
                        CountyEHAKCode = ehakCode,
                        DataOrigin =  DataOrigin.Api,
                        CountryId = countryId.Value, 
                        CreatedBy = "System", 
                        CreatedAt = DateTime.UtcNow,
                    };

                    _appBLL.Counties.Add(county);
                    county.CountyName.SetTranslation(municipalityNameEn, "en-GB");
                    county.CountyName.SetTranslation(municipalityNameEst, "et-EE");
                }
                ;
                
            }
            await _appBLL.SaveChangesAsync();
            
        }
        

        importResult.Success = true;
        return importResult;
    }

    public async Task<IEnumerable<CountyDTO>> GetCountiesByCountryIdAsync(Guid countryId)
    {
        return (await Repository.GetAllCountiesOrderedByCountyNameByCountryIdAsync(countryId))
            .Select(c => Mapper.Map(c))!;
    }

    public async Task<bool> DoesCountyExistsByCountryIdAndEHAKCodeAsync(Guid countryId, string ehakCode)
    {
        return await Repository.DoesCountyExistsByCountryIdAndEHAKCodeAsync(countryId, ehakCode);
    }

    public bool DoesCountyExistsByCountryIdAndEHAKCode(Guid countryId, string ehakCode)
    {
        return Repository.DoesCountyExistsByCountryIdAndEHAKCode(countryId, ehakCode);
    }

    public async Task<CountyDTO?> GetCountyByEHAKCodeAsync(string ehakCode)
    {
        return (Mapper.Map(await Repository.GetCountyByEHAKCodeAsync(ehakCode)));
    }

    public CountyDTO? GetCountyByEHAKCode(string ehakCode)
    {
        return Mapper.Map( Repository.GetCountyByEHAKCode(ehakCode));
    }

    public async Task<List<CountyDTO?>> GetAllCountiesOrderedByCountyNameByCountryIdAsync(Guid countryId, bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false)
    {
        return (await Repository.GetAllCountiesOrderedByCountyNameByCountryIdAsync(countryId: countryId, noTracking: noTracking,
            noIncludes: noIncludes, showDeleted: showDeleted, showIgnored: showIgnored)).Select(c => Mapper.Map(c)).ToList();
    }

    public List<CountyDTO?> GetAllCountiesOrderedByCountyNameByCountryId(Guid countryId, bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false)
    {
        return ( Repository.GetAllCountiesOrderedByCountyNameByCountryId(countryId: countryId, noTracking: noTracking,
            noIncludes: noIncludes, showDeleted: showDeleted, showIgnored: showIgnored)).Select(c => Mapper.Map(c)).ToList();
    }
}