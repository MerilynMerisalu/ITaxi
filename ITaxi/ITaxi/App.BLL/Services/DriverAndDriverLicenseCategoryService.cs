using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class DriverAndDriverLicenseCategoryService: BaseEntityService<App.BLL.DTO.AdminArea.DriverAndDriverLicenseCategoryDTO,
    App.DAL.DTO.AdminArea.DriverAndDriverLicenseCategoryDTO, IDriverAndDriverLicenseCategoryRepository>
    , IDriverAndDriverLicenseCategoryService
{
    public DriverAndDriverLicenseCategoryService(IDriverAndDriverLicenseCategoryRepository repository, IMapper<DriverAndDriverLicenseCategoryDTO, DAL.DTO.AdminArea.DriverAndDriverLicenseCategoryDTO> mapper) : base(repository, mapper)
    {
    }

    public async Task<string?> GetAllDriverLicenseCategoriesBelongingToTheDriverAsync(Guid id, string separator = ", ")
    {
        return await Repository.GetAllDriverLicenseCategoriesBelongingToTheDriverAsync(id, separator);
    }

    public string GetAllDriverLicenseCategoriesBelongingToTheDriver(Guid id, string separator = ", ")
    {
        return Repository.GetAllDriverLicenseCategoriesBelongingToTheDriver(id, separator);
    }

    public async Task<List<DriverAndDriverLicenseCategoryDTO?>> RemovingAllDriverAndDriverLicenseEntitiesByDriverIdAsync(Guid id)
    {
        return (
                await Repository.RemovingAllDriverAndDriverLicenseEntitiesByDriverIdAsync(id))
            .Select(e => Mapper.Map(e)).ToList();
    }

    public async Task<bool> HasAnyDriversAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return await Repository.HasAnyDriversAsync(id, userId, roleName, noTracking);
    }

    public bool HasAnyDrivers(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return Repository.HasAnyDrivers(id, userId, roleName, noTracking);
    }
}