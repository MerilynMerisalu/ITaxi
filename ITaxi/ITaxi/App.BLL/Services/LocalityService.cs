using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.IAppRepositories;
using App.DAL.EF.Repositories;
using Base.BLL;
using Base.Contracts.Mappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BLL.Services
{
    public class LocalityService : BaseEntityService<App.BLL.DTO.AdminArea.LocalityDTO,
     App.DAL.DTO.AdminArea.LocalityDTO, LocalityRepository>, ILocalityService
    {
        public LocalityService(LocalityRepository repository, IMapper<DTO.AdminArea.LocalityDTO, DAL.DTO.AdminArea.LocalityDTO> mapper) : base(repository, mapper)
        {
        }

        public IEnumerable<LocalityDTO> GetAllLocalitiesWithIncludesOrderedByName(bool noTracking = true, bool noIncludes = false)
        {
            return Repository.GetAllLocalitiesWithIncludesOrderedByName(noTracking: noTracking, noIncludes: noIncludes).Select(l => Mapper.Map(l))!;
        }

        public IEnumerable<LocalityDTO> GetAllLocalitiesWithoutIncludesOrderedByName(bool noTracking = true, bool noIncludes = true)
        {
            return Repository.GetAllLocalitiesWithoutIncludesOrderedByName(noTracking: noTracking, noIncludes: noIncludes).Select(l => Mapper.Map(l))!;
        }

        public async Task<LocalityDTO?> GetLocalityByIdWithIncludesAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
        {
            return Mapper.Map(await Repository.GetLocalityByIdWithIncludesAsync(id: id, userId: userId, roleName: roleName, noTracking: noTracking, noIncludes: noIncludes));
        }

        public async Task<LocalityDTO?> GetLocalityByIdWithoutIncludesAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
        {
            return Mapper.Map(await Repository.GetLocalityByIdWithoutIncludesAsync(id: id, userId: userId, roleName: roleName, noTracking: noTracking));
        }
    }
}
