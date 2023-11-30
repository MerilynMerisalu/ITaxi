using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IScheduleService: IEntityService<App.BLL.DTO.AdminArea.ScheduleDTO>,
    IScheduleRepositoryCustom<App.BLL.DTO.AdminArea.ScheduleDTO>
{
    
}