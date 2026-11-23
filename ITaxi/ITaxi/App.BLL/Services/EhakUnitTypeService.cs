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

        public async Task<bool> DoesEhakUnitTypeExistByNameAsync(string name, bool noTracking = true)
        {
            var result = await Repository.DoesEhakUnitTypeExistByNameAsync(name: name, noTracking: noTracking);
            return result;
        }

        public async Task<Guid?> GetEhakUnitTypeIdAsync(string name, bool noTraking = true)
        {
            return await Repository.GetEhakUnitTypeIdAsync(name: name, noTracking:noTraking);
        }

        public async Task<EhakImportResult> ImportingUnitTypeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var importResult = new EhakImportResult();
            string? municipalityNameEn = null;
            string? municipalityNameEst = null;
            string AADRESSURL = "https://gsavalik.envir.ee/geoserver/ehak/wfs" +
             "?service=WFS&version=1.1.0" +
             "&request=GetFeature" +
             "&typeName=ehak:omavalitsuste_piirid" +
             "&outputFormat=application/json";
            var res = await client.GetAsync(AADRESSURL);
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
                    var typeExists = await _appBLL.EhakUnitTypes.DoesEhakUnitTypeExistByNameAsync(name: type);
                }
            }
                    throw new NotImplementedException();
        }
    }
}
