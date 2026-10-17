using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Contracts.BLL.ImportResults;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.BLL;
using Base.Contracts.Mappers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RESTCountries.NET.Models;
using System.Reflection;
using System.Text.Json.Nodes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace App.BLL.Services;

public class MunicipalityService : BaseEntityService<App.BLL.DTO.AdminArea.MunicipalityDTO, DAL.DTO.AdminArea.MunicipalityDTO,
    IMunicipalityRepository>, IMuncipalityService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IAppBLL _appBLL;
    private readonly ILogger<MunicipalityService> _logger;
    public MunicipalityService(IMunicipalityRepository repository, IMapper<MunicipalityDTO, DAL.DTO.AdminArea.MunicipalityDTO> mapper, IHttpClientFactory httpClientFactory, IAppBLL appBLL) : base(repository, mapper)
    {
        _httpClientFactory = httpClientFactory;
        _appBLL = appBLL;
    }

    public async Task<IEnumerable<MunicipalityDTO>> GetAllMunicipalitiesWithoutCountyAsync()
    {
        return (await Repository.GetAllMunicipalitiesWithoutCountyAsync()).Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<MunicipalityDTO>> GetAllOrderedMunicipalitiesWithoutCountyAsync()
    {
        return (await Repository.GetAllOrderedMunicipalitiesWithoutCountyAsync()).Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<MunicipalityDTO>> GetAllOrderedMunipalitiesAsync(bool showDeleted = false, bool showIgnored = false)
    {
        return (await Repository.GetAllOrderedMunipalitiesAsync(showIgnored: showIgnored, showDeleted: showDeleted)).Select(e => Mapper.Map(e))!;
    }

    public async Task<MunicipalityDTO?> FirstOrDefaultMunicipalityWithoutCountyAsync(Guid id, bool showIgnored = false)
    {
        return Mapper.Map(await Repository.FirstOrDefaultMunicipalityWithoutCountyAsync(id, showIgnored: showIgnored));
    }

    public IEnumerable<MunicipalityDTO> GetAllOrderedMunicipalitiesWithoutCounty()
    {
        return Repository.GetAllOrderedMunicipalitiesWithoutCounty().Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<MunicipalityDTO> GetAllOrderedMunicipalities(bool noTracking = true)
    {
        return Repository.GetAllOrderedMunicipalities(noTracking).Select(e => Mapper.Map(e))!;
    }

    public async Task<bool> HasAnyMunicipalitiesAsync(Guid id, bool noTracking = true)
    {
        return await Repository.HasAnyMunicipalitiesAsync(id, noTracking);
    }

    public bool HasAnyMunicipalities(Guid id, bool noTracking = true)
    {
        return Repository.HasAnyMunicipalities(id, noTracking);
    }

    public async Task<MunicipalityDTO?> GetMunicipalityByIdAsync(Guid id, bool noTracking = false, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false)
    {
        return Mapper.Map(await Repository.GetMunicipalityByIdAsync(id: id, noTracking: noTracking, noIncludes: noIncludes, showDeleted: showDeleted, showIgnored: showIgnored));
    }

    public async Task<List<MunicipalityDTO?>> GetMunicipalitiesByCountyIdAsync(Guid countyId, bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false)
    {
        return (await Repository.GetMunicipalitiesByCountyIdAsync(countyId: countyId, noTracking: noTracking, noIncludes: noIncludes, showDeleted: showDeleted, showIgnored: showIgnored))
            .Select(m => Mapper.Map(m)).ToList();
            
    }

    public List<MunicipalityDTO?> GetMunicipalitiesByCountyId(Guid countyId, bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false)
    {
        return ( Repository.GetMunicipalitiesByCountyId(countyId: countyId, noTracking: noTracking, noIncludes: noIncludes, showDeleted: showDeleted, showIgnored: showIgnored)
            .Select(m => Mapper.Map(m)).ToList());
    }

    public async Task<EhakImportResult> ImportingMuncipalitiesFromEHAK()
    {
        var client = _httpClientFactory.CreateClient();
        var importResult = new EhakImportResult();
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
              "&typeName=ehak:omavalitsuste_piirid" +
              "&outputFormat=application/json";
        var response = await client.GetAsync(AADRESSURL);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"EHAK unavailable. Status: {response.StatusCode}");
            importResult.Success = false;
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
                    importResult.Success = false;
                    return importResult;
                }

                var ehakCode = props["ehak_kood"]?.Value<string>();
                var countyEhakCode = props["maakond_kood"]?.Value<string>();
                var type = props["tyyp"]?.Value<string>();
                var municipalityName = props["omavalitsus"]?.Value<string>();

                _logger.LogInformation("Importing municipality {Name}, ehak code {MunicipalityEhakCode}, County Ehak code {CountyEhakCode}, municipality type {type} ", municipalityName, ehakCode, countyEhakCode, type);
                if (string.IsNullOrWhiteSpace(municipalityName))
                {
                    _logger.LogWarning($"Skipping municipality due to missing required data. Name: {municipalityName}");
                    continue;

                }
                else if (string.IsNullOrWhiteSpace(ehakCode))
                {
                    _logger.LogWarning($"Skipping municipality due to missing required data. Ehak code: {ehakCode}");
                    continue;
                }
                else if (string.IsNullOrWhiteSpace(countyEhakCode))
                {
                    _logger.LogWarning($"Skipping municipality due to missing required data. County code: {countyEhakCode}");
                    continue;
                }
                else if (string.IsNullOrWhiteSpace(type))
                {
                    _logger.LogWarning($"Skipping municipality due to missing required data. Type: {type}");
                    continue;
                }
                var county = _appBLL.Counties.GetCountyByEHAKCode(ehakCode: ehakCode);

                if (county == null)
                {
                    _logger.LogWarning($"Skipping municipality due to missing required data. county id");
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(municipalityName) && !string.IsNullOrWhiteSpace(ehakCode))
                {
                    var exists =
                        await _appBLL.Municipalities.DoesMunicipalityExistAsync(ehakCode);
                    if (exists) continue;
                }
                var ehakUnitType =
                var municipality = new MunicipalityDTO { Id = Guid.NewGuid(), CountyId = county.Id, MunicipalityEHAKCode = ehakCode, };
            }
        }
    }

    public async Task<bool> DoesMunicipalityExistAsync( string ehakCode)
    {
        return await Repository.DoesMunicipalityExistAsync( ehakCode: ehakCode);
    }
}