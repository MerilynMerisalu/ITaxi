using System.Globalization;
using App.BLL.DTO.AdminArea;
using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface ICountryService: IEntityService<App.BLL.DTO.AdminArea.CountryDTO>,
    ICountryRepositoryCustom<App.BLL.DTO.AdminArea.CountryDTO> // Add custom stuff
{
    IEnumerable<CountryDTO?> GetAllCountriesThroughRestAPI(string langCode);
    Task UpdateCountriesFromAPIAsync(CultureInfo[] cultures);
}