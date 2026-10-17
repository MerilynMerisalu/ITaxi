using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.ImportResults;
using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IMuncipalityService : IEntityService<App.BLL.DTO.AdminArea.MunicipalityDTO>,
    IMunicipalityCustom<App.BLL.DTO.AdminArea.MunicipalityDTO>
{
    Task<EhakImportResult> ImportingMuncipalitiesFromEHAK();
}