using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using App.DAL.EF.Repositories;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class DriverLicenseCategoryService : BaseEntityService<App.BLL.DTO.AdminArea.DriverLicenseCategoryDTO,
    App.DAL.DTO.AdminArea.DriverLicenseCategoryDTO, IDriverLicenseCategoryRepository>,
    IDriverLicenseCategoryService
{
    public DriverLicenseCategoryService(IDriverLicenseCategoryRepository repository, IMapper<DriverLicenseCategoryDTO, DAL.DTO.AdminArea.DriverLicenseCategoryDTO> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<DriverLicenseCategoryDTO>> GetAllDriverLicenseCategoriesOrderedAsync(bool noTracking = true)
    {
        return (await Repository.GetAllDriverLicenseCategoriesOrderedAsync(noTracking))
            .Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<DriverLicenseCategoryDTO> GetAllDriverLicenseCategoriesOrdered(bool noTracking = true)
    {
        return
            Repository.GetAllDriverLicenseCategoriesOrdered(noTracking)
                .Select(e => Mapper.Map(e))!;
    }
}
