using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.DAL.EF.Repositories
{
    public class ExtraServiceRepository :
        BaseEntityRepository<ExtraServiceDTO, ExtraService, AppDbContext>
        , IExtraServiceRepository
    {
        public ExtraServiceRepository(AppDbContext dbContext, IMapper<ExtraServiceDTO, ExtraService> mapper) : base(dbContext, mapper)
        {
        }

        public IEnumerable<ExtraServiceDTO> GetAllExtraServicesOrderedByName(bool noTracking = true, bool noIncludes = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ExtraServiceDTO>> GetAllExtraServicesOrderedByNameAsync(bool noTracking = true, bool noIncludes = false)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ExtraServiceDTO> GetExtraServiceByIdWithIncludes(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
        {
            throw new NotImplementedException();
        }

        public Task<ExtraServiceDTO?> GetExtraServiceByIdWithIncludesAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ExtraServiceDTO> GetExtraServiceByIdWithoutIncludes(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
        {
            throw new NotImplementedException();
        }

        public Task<ExtraServiceDTO?> GetExtraServiceByIdWithoutIncludesAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
        {
            throw new NotImplementedException();
        }

        protected override IQueryable<ExtraService> CreateQuery(bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false)
        {
            if (!showIgnored)
            {

                return RepoDbSet
                    .Include(c => c.ExtraServiceName)
                    .ThenInclude(c => c.Translations).Where(c => c.IsIgnored == true)
                    .AsNoTracking();
            }
            if (noTracking)
            {
                return RepoDbSet
                    .Include(c => c.ExtraServiceName)
                    .ThenInclude(c => c.Translations).Where(c => c.IsDeleted == false)
                    .AsNoTracking();
            }

            if (noIncludes)
            {
                return RepoDbSet;

            }

            return RepoDbSet
                .Include(c => c.ExtraServiceName)
                .ThenInclude(c => c.Translations)
                .AsNoTracking();
        }
    }
}
