using App.Contracts.DAL;
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
    public class LocalityRepository :
        BaseEntityRepository<LocalityDTO, Locality, AppDbContext>
        , ILocalityRepository
    {
        public LocalityRepository(AppDbContext dbContext, IMapper<LocalityDTO, Locality> mapper) : base(dbContext, mapper)
        {
        }

        public IEnumerable<LocalityDTO> GetAllLocalitiesWithIncludesOrderedByName(bool noTracking = true, bool noIncludes = false)
        {
            return CreateQuery(noTracking: noTracking, noIncludes: noIncludes).OrderBy(l => l.LocalityName).Select(l => Mapper.Map(l))!;
        }

        public IEnumerable<LocalityDTO> GetAllLocalitiesWithoutIncludesOrderedByName(bool noTracking = true, bool noIncludes = true)
        {
            return CreateQuery(noTracking: noTracking, noIncludes: noIncludes).OrderBy(l => l.LocalityName).Select(l => Mapper.Map(l))!;
        }

        public async Task<LocalityDTO?> GetLocalityByIdWithIncludesAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = false)
        {
            return Mapper.Map(await CreateQuery(noTracking: noTracking, noIncludes: noIncludes).FirstOrDefaultAsync(l  => l.Id == id));
        }

        public async Task<LocalityDTO?> GetLocalityByIdWithoutIncludesAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true, bool noIncludes = true)
        {
            return Mapper.Map(await CreateQuery(noTracking: noTracking, noIncludes: noIncludes).FirstOrDefaultAsync(l => l.Id == id));
        }
    }
}
