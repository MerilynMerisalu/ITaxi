using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts.Mappers;
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
            return CreateQuery(noTracking: noTracking, noIncludes: noIncludes).OrderBy(e => e.ExtraServiceName).Select(e => Mapper.Map(e)).ToList()!;
        }

        public async Task<IEnumerable<ExtraServiceDTO>> GetAllExtraServicesOrderedByNameAsync(bool noTracking = true, bool noIncludes = false)
        {
            var res = await CreateQuery(noTracking: noTracking, noIncludes: noIncludes).OrderBy(e => e.ExtraServiceName).Select(e => Mapper.Map(e)).ToListAsync()!;
            return res!;
        }

        public ExtraServiceDTO? GetExtraServiceByIdWithIncludes(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
        {
            return Mapper.Map(CreateQuery(noIncludes: noIncludes, noTracking: noTracking)
                .FirstOrDefault(e => e.Id.Equals(id)))!;
        }

        public async Task<ExtraServiceDTO?> GetExtraServiceByIdWithIncludesAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
        {
            return Mapper.Map(await CreateQuery(noIncludes: noIncludes, noTracking: noTracking).FirstOrDefaultAsync(e => e.Id.Equals(id)));
        }

        public ExtraServiceDTO? GetExtraServiceByIdWithoutIncludes(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
        {
            return Mapper.Map(base.CreateQuery(noIncludes: noIncludes, noTracking: noTracking).FirstOrDefault(e => e.Id.Equals(id)));
        }

        public async Task<ExtraServiceDTO?> GetExtraServiceByIdWithoutIncludesAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
        {
       
            return Mapper.Map( await base.
                CreateQuery(noIncludes: noIncludes, noTracking: noTracking).FirstOrDefaultAsync(e => e.Id.Equals(id)));
        }

        protected override IQueryable<ExtraService> CreateQuery(bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false)
        {
            if (!showIgnored)
            {

                return RepoDbSet
                    .Include(c => c.ExtraServiceName)
                        .ThenInclude(c => c.Translations).Where(c => c.IsIgnored == false)
                    .Include(c => c.Description)
                        .ThenInclude(c => c.Translations).Where(c => c.IsIgnored == false)
                    .AsNoTracking();
            }
            if (noTracking)
            {
                return RepoDbSet
                    .Include(c => c.ExtraServiceName)
                        .ThenInclude(c => c.Translations).Where(c => c.IsDeleted == false)
                    .Include(c => c.Description)
                        .ThenInclude(c => c.Translations).Where(c => c.IsIgnored == false)
                    .AsNoTracking();
            }

            if (noIncludes)
            {
                return RepoDbSet;

            }

            return RepoDbSet
                .Include(c => c.ExtraServiceName)
                .ThenInclude(c => c.Translations)
                .Include(c => c.Description)
                    .ThenInclude(c => c.Translations).Where(c => c.IsIgnored == false)
                .AsNoTracking();
        }
    }
}
