using App.BLL.DTO.AdminArea;
using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IDriverService: IEntityService<App.BLL.DTO.AdminArea.DriverDTO>,
    IDriverRepositoryCustom<App.BLL.DTO.AdminArea.DriverDTO> // Add custom stuff
{
    Task<DriverDTO> GettingDriverByAppUserIdAsync(Guid Id);
}