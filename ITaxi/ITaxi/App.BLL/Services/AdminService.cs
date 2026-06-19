using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.Identity;
using Base.BLL;
using Base.Contracts.Mappers;
using AppUser = App.BLL.DTO.Identity.AppUser;

namespace App.BLL.Services;

public class AdminService: BaseEntityService<App.BLL.DTO.AdminArea.AdminDTO,App.DAL.DTO.AdminArea.AdminDTO, IAdminRepository>,
    IAdminService
{
    public AdminService(IAdminRepository repository, IMapper<AdminDTO, DAL.DTO.AdminArea.AdminDTO> mapper) : base(repository, mapper)
    {
    }

    

    public AdminDTO? GetAdminByAdminId(Guid id, bool noIncludes = false, bool noTracking = true, bool showDeleted = false, bool showIgnored = false)
    {
        return Mapper.Map(Repository.GetAdminByAdminId(id, noIncludes: noIncludes, noTracking: noTracking, showDeleted: showDeleted, showIgnored: showIgnored));
    }

    public async Task<AdminDTO?> GetAdminByAppUserIdAsync(Guid appuserId, bool noIncludes = false, bool noTracking = true, bool showDeleted = false, bool showIgnored = false)
    {
        return Mapper.Map(await Repository.GetAdminByAppUserIdAsync(appuserId: appuserId, noIncludes: noIncludes, noTracking: noTracking, showDeleted: showDeleted, showIgnored: showIgnored));
    }

    public async Task<AdminDTO?>? GetAdminWithIncludesByAdminIdAsync(Guid adminId, bool noIncludes = false, bool noTracking = true, bool showDeleted = false, bool showIgnored = false)
    {
        return Mapper.Map(await Repository.GetAdminWithIncludesByAdminIdAsync(adminId, noIncludes: noIncludes, noTracking: noTracking, showDeleted: showDeleted, showIgnored: showIgnored));
    }

   

    public async Task<IEnumerable<AdminDTO>> GetAllAdminsOrderedByLastNameAsync(bool noTracking = true)
    {
        return (await Repository.GetAllAdminsOrderedByLastNameAsync(noTracking)).Select(e => Mapper.Map(e))!;
    }

    

    
}