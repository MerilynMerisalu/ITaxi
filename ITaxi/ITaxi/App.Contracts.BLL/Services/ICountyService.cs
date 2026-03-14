using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.ImportResults;
using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface ICountyService: IEntityService<App.BLL.DTO.AdminArea.CountyDTO>,
    ICountyRepositoryCustom<App.BLL.DTO.AdminArea.CountyDTO> // Add custom stuff
{
    Task<CountyImportResult> ImportCountiesFromEHAKAsync(HttpClient client);
    Task<IEnumerable<CountyDTO>>GetCountiesByCountryIdAsync(Guid countryId);
}