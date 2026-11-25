using App.BLL.DTO;
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Contracts.BLL.ImportResults;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.DAL.EF.Repositories;
using Base.BLL;
using Base.Contracts.Mappers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Nodes;

namespace App.BLL.Services
{

    public class EhakUnitTypeService : BaseEntityService<App.BLL.DTO.AdminArea.EhakUnitTypeDTO, DAL.DTO.AdminArea.EhakUnitTypeDTO,
    IEhakUnitTypeRepository>, IEhakUnitTypeService
    {
        public readonly IAppBLL _appBLL;
        public readonly ILogger<EhakUnitTypeService> _logger;
        public readonly IHttpClientFactory _httpClientFactory;
        public EhakUnitTypeService(IEhakUnitTypeRepository repository, IMapper<DTO.AdminArea.EhakUnitTypeDTO, DAL.DTO.AdminArea.EhakUnitTypeDTO> mapper, IAppBLL appBLL, ILogger<EhakUnitTypeService> logger, IHttpClientFactory httpClientFactory) : base(repository, mapper)
        {
            _appBLL = appBLL;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public Task<bool> DoesEhakUnitTypeExistByEhakCodeAsync(string ehakCode, bool noTracking = true)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DoesEhakUnitTypeExistByNameAsync(string name, bool noTracking = true)
        {
            var result = await Repository.DoesEhakUnitTypeExistByNameAsync(name: name, noTracking: noTracking);
            return result;
        }

        public async Task<Guid?> GetEhakUnitTypeIdAsync(string name, bool noTraking = true)
        {
            return await Repository.GetEhakUnitTypeIdAsync(name: name, noTracking: noTraking);
        }

        public async Task<EhakImportResult> ImportingUnitTypeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var importResult = new EhakImportResult();
            string? municipalityNameEn = null;
            string? municipalityNameEst = null;
            string addressUrl = "https://gsavalik.envir.ee/geoserver/ehak/wfs" +
             "?service=WFS&version=1.1.0" +
             "&request=GetFeature" +
             "&typeName=ehak:omavalitsuste_piirid" +
             "&outputFormat=application/json";
            var res = await client.GetAsync(addressUrl);
            if (!res.IsSuccessStatusCode)
            {
                _logger.LogError($"EHAK unavailable. Status: {res.StatusCode}");
                importResult.Success = false;
                return importResult;
            }
            var jsonResult = await res.Content.ReadAsStringAsync();
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

                    var type = props["tyyp"]?.Value<string>();
                    var typeEhakCode = props["ehak_kood"]?.Value<string>();
                    const string TRANSLATIONADDRESS = "https://klassifikaatorid.stat.ee/item/stat.ee/079ed389-81f0-4171-8442-d8cf7412cdb7/5/report/se.classification-version-data/json";
                    res = await client.GetAsync(TRANSLATIONADDRESS);
                    if (!res.IsSuccessStatusCode)
                    {
                        importResult.Success = false;
                        _logger.LogError($"EHAK unavailable. Status: {res.StatusCode}");
                        return importResult;
                    }


                    var jsonTranslationResult = await res.Content.ReadAsStringAsync();
                    var rootTranslation = JObject.Parse(jsonTranslationResult);
                    var translationFeatures = rootTranslation["Items"];
                    if (translationFeatures != null)
                    {
                        var item = translationFeatures.FirstOrDefault(i => i["Code"]?.Value<string>() == typeEhakCode);
                        if (item == null)
                        {
                            _logger.LogWarning("Municipality with EHAK code {EhakCode} was not found in the translation source", typeEhakCode);
                        }
                        else
                        {
                            municipalityNameEn = item["Label"]?["en-GB"]?.Value<string>();
                            municipalityNameEst = item["Label"]?["et-EE"]?.Value<string>();

                            _logger.LogInformation("Importing municipality english name {municipalityNameEn}, estonian name {municipalityNameEst}", municipalityNameEn, municipalityNameEst);
                        }
                        var typeExists = await _appBLL.EhakUnitTypes.DoesEhakUnitTypeExistByNameAsync(name: type);

                        //if (typeExists)
                        //{

                        //}
                    }
                }
                
            }
            throw new NotImplementedException();
        }
    }
}
