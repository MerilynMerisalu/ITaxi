using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.BLL;
using Base.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Services
{
    public class ExtraService : BaseEntityService<App.BLL.DTO.AdminArea.ExtraServiceDTO,
    App.DAL.DTO.AdminArea.ExtraServiceDTO, IExtraServiceRepository>, IExtraService
    {
        public ExtraService(IExtraServiceRepository repository, IMapper<DTO.AdminArea.ExtraServiceDTO, DAL.DTO.AdminArea.ExtraServiceDTO> mapper) : base(repository, mapper)
        {
        }

        public IEnumerable<ExtraServiceDTO> GetAllExtraServicesOrderedByName(bool noTracking = true, bool noIncludes = false)
        {
            return Repository.GetAllExtraServicesOrderedByName(noTracking: noTracking, noIncludes: noIncludes).Select(e => Mapper.Map(e)!);
        }

        public async Task<IEnumerable<ExtraServiceDTO>> GetAllExtraServicesOrderedByNameAsync(bool noTracking = true, bool noIncludes = false)
        {
            return (await Repository.GetAllExtraServicesOrderedByNameAsync(noTracking: noTracking, noIncludes: noIncludes)).Select(e => Mapper.Map(e)!);
        }


        public async Task<ExtraServiceDTO?> GetExtraServiceByIdWithIncludesAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
        {
            return Mapper.Map(await Repository.GetExtraServiceByIdWithIncludesAsync(id: id, userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes));
        }

        

        public async Task<ExtraServiceDTO?> GetExtraServiceByIdWithoutIncludesAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
        {
            return Mapper.Map(await Repository.GetExtraServiceByIdWithoutIncludesAsync(id: id, roleName: roleName, noTracking: false, noIncludes: noIncludes));
        }
    }
}
