using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using Base.Contracts.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Contracts.DAL
{
    public interface ILocalityRepository : IEntityRepository<LocalityDTO>,
        ILocalityCustomRepository<LocalityDTO>
    {
    }

    public interface ILocalityCustomRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllLocalitiesOrderedByNameAsync(bool noTracking = true, bool noIncludes = false);
        IEnumerable<TEntity> GetAllLocalitiesOrderedByName(bool noTracking = true, bool noIncludes = false);
        Task<TEntity?> GetLocalityByIdWithIncludesAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false);
        Task<TEntity?> GetLocalityByIdWithoutIncludesAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false);
    }
}
