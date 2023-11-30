using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.Identity;
using Base.BLL;
using Base.Contracts;
using AppUser = App.BLL.DTO.Identity.AppUser;

namespace App.BLL.Services;

public class AdminService: BaseEntityService<App.BLL.DTO.AdminArea.AdminDTO,App.DAL.DTO.AdminArea.AdminDTO, IAdminRepository>,
    IAdminService
{
    public AdminService(IAdminRepository repository, IMapper<AdminDTO, DAL.DTO.AdminArea.AdminDTO> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<AdminDTO>> GetAllAdminsOrderedByLastNameAsync(bool noTracking = true)
    {
        return (await Repository.GetAllAdminsOrderedByLastNameAsync(noTracking)).Select(e => Mapper.Map(e))!;
    }

    

    
}