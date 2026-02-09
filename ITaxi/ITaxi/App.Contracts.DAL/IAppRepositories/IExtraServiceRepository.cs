using App.DAL.DTO.AdminArea;
using Base.Contracts.DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Contracts.DAL.IAppRepositories
{
    public interface IExtraServiceRepository: IEntityRepository<ExtraServiceDTO>, 
        IExtraServiceCustomRepository<ExtraServiceDTO>
    {
    }

    public interface IExtraServiceCustomRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllExtraServicesOrderedByNameAsync(bool noTracking = true, bool noIncludes = false);
        IEnumerable<TEntity> GetAllExtraServicesOrderedByName(bool noTracking = true, bool noIncludes = false);
        Task<TEntity?> GetExtraServiceByIdWithIncludesAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false);
        Task<TEntity?> GetExtraServiceByIdWithoutIncludesAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false);
        

    }
}
